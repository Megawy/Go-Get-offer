import axios from "axios";
import store from "@/Redux/store";
import { logout, setCredentials } from "@/Redux/Slices/authSlice";

// Create axios instance
const axiosRequester = axios.create({
  baseURL: process.env.NEXT_PUBLIC_BASE_URL,
  withCredentials: true, // Allow cookies (for refresh token)
});

// Variables for refresh logic
let isRefreshing = false;
let failedQueue = [];

// Handle queued requests while refresh is in progress
const processQueue = (error, token = null) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });

  failedQueue = [];
};

// Add Authorization header for every request if token exists
axiosRequester.interceptors.request.use((config) => {
  const state = store.getState();
  const token = state.auth.token;

  if (token) {
    config.headers["Authorization"] = `Bearer ${token}`;
  }

  return config;
});

// Response interceptor
axiosRequester.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    // 🛑 Handle Rate Limiting (429)
    if (error.response?.status === 429) {
      return Promise.reject({
        message: "🚨 Too many requests. Please wait a bit before trying again.",
        originalError: error,
      });
    }

    // 🔒 Handle Unauthorized (401)
    if (error.response?.status === 401 && !originalRequest._retry) {
      if (isRefreshing) {
        // If refresh already in progress → wait in queue
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject });
        })
          .then((token) => {
            originalRequest.headers["Authorization"] = "Bearer " + token;
            return axiosRequester(originalRequest);
          })
          .catch((err) => Promise.reject(err));
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        // Request new access token using refresh token (cookie)
        const { data } = await axios.post(
          `${process.env.NEXT_PUBLIC_BASE_URL}/api/Auth/refresh-token`,
          {},
          { withCredentials: true }
        );

        const newAccessToken = data.accessToken;
        if (!newAccessToken) throw new Error("No access token returned");

        // ✅ Update redux state (token فقط، باقي البيانات AppInitializer بيعملها)
        store.dispatch(setCredentials({ token: newAccessToken }));

        // ✅ Update axios default Authorization header
        axiosRequester.defaults.headers.common[
          "Authorization"
        ] = "Bearer " + newAccessToken;

        // Resolve all queued requests with the new token
        processQueue(null, newAccessToken);
        isRefreshing = false;

        // Retry the original request with the new token
        originalRequest.headers["Authorization"] = "Bearer " + newAccessToken;
        return axiosRequester(originalRequest);
      } catch (err) {
        // ❌ If refresh fails → logout user
        processQueue(err, null);
        isRefreshing = false;
        store.dispatch(logout());
        return Promise.reject(err);
      }
    }

    return Promise.reject(error);
  }
);

export default axiosRequester;


import axios from "axios";
import { store } from "@/Redux/store";                
import { logout, setCredentials } from "@/Redux/Slices/authSlice"; // Auth actions

// Create a dedicated Axios instance for the app
const axiosRequester = axios.create({
  baseURL: process.env.NEXT_PUBLIC_BASE_URL,            // e.g. https://api.example.com
  withCredentials: true,                               // Send/receive cookies (Refresh Token in HttpOnly cookie)
  headers: { "Content-Type": "application/json" },
});

// /**
//  * Security model:
//  * - Access Token lives in memory/Redux and goes in the Authorization header.
//  * - Refresh Token is stored by the backend in an HttpOnly cookie (browser auto-sends it).
//  * - To avoid multiple refresh calls and infinite loops, we use a flag + a queue.
//  */
// let isRefreshing = false;   // Marks that a refresh call is in progress
// let pendingQueue = [];      // Requests waiting for the new access token

// // Resolve or reject all waiting requests once refresh finishes
// function resolveQueue(error, newToken = null) {
//   pendingQueue.forEach(({ resolve, reject }) => {
//     if (error) reject(error);      // Propagate the refresh error to the waiting request
//     else resolve(newToken);        // Provide the fresh token to the waiting request
//   });
//   pendingQueue = [];               // Clear the queue
// }

// // Request interceptor: attach Authorization header if we have an access token
// axiosRequester.interceptors.request.use(
//   (config) => {
//     const token = store.getState().auth.token;         // Read token from Redux (in-memory)
//     if (token) config.headers.Authorization = `Bearer ${token}`;
//     return config;
//   },
//   (error) => Promise.reject(error)                     // Bubble up request setup errors
// );

// // Response interceptor: handle 401 by refreshing once, queueing parallel requests
// axiosRequester.interceptors.response.use(
//   (response) => response,                              // Pass through successful responses
//   async (error) => {
//     const status = error?.response?.status;
//     const originalRequest = error.config;

//     // Non-401 errors → just reject
//     if (status !== 401) return Promise.reject(error);

//     // Prevent infinite retry loops
//     if (originalRequest._retry) return Promise.reject(error);
//     originalRequest._retry = true;

//     try {
//       // If a refresh is already in-flight, wait in the queue for the new token
//       if (isRefreshing) {
//         const newToken = await new Promise((resolve, reject) => {
//           pendingQueue.push({ resolve, reject });
//         });
//         if (newToken) {
//           originalRequest.headers.Authorization = `Bearer ${newToken}`;
//         }
//         return axiosRequester(originalRequest); // Retry with the fresh token
//       }

//       // Start a new refresh cycle
//       isRefreshing = true;

//       // Use raw axios to avoid re-triggering this same interceptor chain
//       const refreshRes = await axiosRequester.post(
//         `${process.env.NEXT_PUBLIC_BASE_URL}/auth/refresh`,
//         {},
//         { withCredentials: true }                       // Required so the HttpOnly refresh cookie is sent
//       );

//       const newAccessToken = refreshRes.data?.accessToken; // Depends on your API shape
//       if (!newAccessToken) throw new Error("No accessToken from refresh");

//       // Update Redux with the new token (keep the old user object if needed)
//       const prevUser = store.getState().auth.user;
//       store.dispatch(setCredentials({ token: newAccessToken, user: prevUser }));

//       // Release all queued requests with the new token
//       isRefreshing = false;
//       resolveQueue(null, newAccessToken);

//       // Retry the original request with the fresh token
//       originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
//       return axiosRequester(originalRequest);
//     } catch (err) {
//       // Refresh failed → flush the queue with error and log the user out
//       isRefreshing = false;
//       resolveQueue(err, null);
//       store.dispatch(logout());
//       return Promise.reject(err);
//     }
//   }
// );


/**
 * Security model:
 * - Access Token should be passed in config.headers.Authorization
 * - Refresh Token is stored in HttpOnly cookie
 * - Queue system avoids multiple refresh calls
 */
let isRefreshing = false;
let pendingQueue = [];

function resolveQueue(error, newToken = null) {
  pendingQueue.forEach(({ resolve, reject }) => {
    if (error) reject(error);
    else resolve(newToken);
  });
  pendingQueue = [];
}

// Request interceptor: attach Authorization header if provided in config
axiosRequester.interceptors.request.use(
  (config) => {
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor: handle 401 by refreshing once, queueing parallel requests
axiosRequester.interceptors.response.use(
  (response) => response,
  async (error) => {
    const status = error?.response?.status;
    const originalRequest = error.config;

    if (status !== 401) return Promise.reject(error);
    if (originalRequest._retry) return Promise.reject(error);
    originalRequest._retry = true;

    try {
      if (isRefreshing) {
        const newToken = await new Promise((resolve, reject) => {
          pendingQueue.push({ resolve, reject });
        });
        if (newToken) originalRequest.headers.Authorization = `Bearer ${newToken}`;
        return axiosRequester(originalRequest);
      }

      isRefreshing = true;

      const refreshRes = await axiosRequester.post(
        "/auth/refresh",
        {},
        { withCredentials: true }
      );

      const newAccessToken = refreshRes.data?.accessToken;
      if (!newAccessToken) throw new Error("No accessToken from refresh");

      isRefreshing = false;
      resolveQueue(null, newAccessToken);

      originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
      return axiosRequester(originalRequest);
    } catch (err) {
      isRefreshing = false;
      resolveQueue(err, null);
      return Promise.reject(err);
    }
  }
);




export default axiosRequester;
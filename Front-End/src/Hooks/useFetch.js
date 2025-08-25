// Hooks/useFetch.js
import axiosRequester from "@/lib/Axios/axios";

// Suspense wrapper (for GET only)
function wrapPromise(promise) {
    let status = "pending";
    let result;

    const suspender = promise.then(
        (res) => {
            status = "success";
            result = res;
        },
        (err) => {
            status = "error";
            result = err;
        }
    );

    return {
        read() {
            if (status === "pending") throw suspender;
            if (status === "error") throw result;
            if (status === "success") return result;
        },
    };
}

/**
 * useFetch - Flexible data fetcher
 * - GET requests → Suspense compatible
 * - POST/PUT/DELETE → Promise (for mutations)
 */
export function useFetch(url, options = {}) {
    const isAbsolute = /^https?:\/\//i.test(url);
    const method = (options.method || "GET").toUpperCase();

    const request = axiosRequester({
        url,
        method,
        ...(isAbsolute ? { baseURL: "" } : {}),
        ...options,
    }).then((res) => res.data);
    // console.log("🚀 Axios Request Config:", {
    //     url: request.url,
    //     method: request.method,
    //     headers: request.headers,
    //     data: request.data,
    //     params: request.params,
    // });

    if (method === "GET") {
        return wrapPromise(request);
    }

    return request; // mutations return normal Promise
}

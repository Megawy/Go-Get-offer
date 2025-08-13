import axiosRequester from "@/lib/Axios/axios";

function wrapPromise(promise) {
    let status = "pending";
    let result;

    let suspender = promise.then(
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


// export function useFetch(url, options = {}) {
//     const promise = axiosRequester({ url, ...options })
//         .then((res) => res.data);
//     return wrapPromise(promise);

// }



export function useFetch(url, options = {}) {

    const isAbsolute = /^https?:\/\//i.test(url);

    const promise = axiosRequester({
        url,
        ...(isAbsolute ? { baseURL: '' } : {}),
        ...options
    }).then((res) => res.data);

    return wrapPromise(promise);
}

import axiosRequester from "@/lib/Axios/axios";
import { useQuery, useMutation } from "@tanstack/react-query";

// ======================
// QUERY FETCH (GET requests)
// ======================
async function fetchData(url, options = {}) {
    const isAbsolute = /^https?:\/\//i.test(url);

    const { data } = await axiosRequester({
        url,
        ...(isAbsolute ? { baseURL: "" } : {}),
        ...options,
    });

    console.log({ dir: "query", data });
    return data;
}

export function useQueryFetch(queryKey, url, options = {}, queryOptions = {}) {
    return useQuery({
        queryKey: Array.isArray(queryKey) ? queryKey : [queryKey],
        queryFn: () => fetchData(url, options),
        suspense: true,
        ...queryOptions,
    });
}

// ======================
// MUTATION FETCH (POST/PUT/DELETE requests)
// ======================
export function useMutationFetch(url, options = {}, mutationOptions = {}) {
    return useMutation({
        mutationFn: async (body) => {
            const isAbsolute = /^https?:\/\//i.test(url);

            const { data } = await axiosRequester({
                url,
                method: options.method || "POST", // default to POST
                ...(isAbsolute ? { baseURL: "" } : {}),
                data: body,
                ...options,
            });

            console.log({ dir: "mutation", data });
            return data;
        },
        ...mutationOptions,
    });
}

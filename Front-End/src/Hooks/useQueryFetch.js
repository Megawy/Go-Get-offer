import axiosRequester from "@/lib/Axios/axios";
import { useQuery } from "@tanstack/react-query";



async function fetchData(url, options = {}) {
    const isAbsolute = /^https?:\/\//i.test(url);

    const { data } = await axiosRequester({
        url,
        ...(isAbsolute ? { baseURL: "" } : {}),
        ...options,
    });
console.log({dir:'query',data})
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

    import axios from "axios";


    // const axiosRequester = axios.create({
    //     baseURL: process.env.NEXT_PUBLIC_BASE_URL,
    //     withCredentials: true,
    //     headers: {
    //         "Content-Type": "application/json",
    //     },
    // });
    const axiosRequester = axios.create({
        baseURL: 'https://jsonplaceholder.typicode.com/users',
        withCredentials: true,
        headers: {
            "Content-Type": "application/json",
        },
    });

    
    export default axiosRequester;
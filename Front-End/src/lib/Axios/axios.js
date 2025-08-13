import axios from "axios";

const axiosRequester = axios.create({
    baseURL: 'https://jsonplaceholder.typicode.com',
    withCredentials: true,
    headers: {
        "Content-Type": "application/json",
    },
});


export default axiosRequester;



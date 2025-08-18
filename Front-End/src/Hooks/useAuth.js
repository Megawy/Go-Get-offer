import { useDispatch, useSelector } from "react-redux";
import { setCredentials, logout } from "@/Redux/Slices/authSlice";
import {
    selectCurrentUser,
    selectCurrentToken,
    selectIsAuthenticated
} from "@/Redux/Slices/authSlice";
import { useCallback } from "react";
import axiosRequester from "@/Utils/axiosRequester";

const useAuth = () => {
    const dispatch = useDispatch();

    const user = useSelector(selectCurrentUser);
    const token = useSelector(selectCurrentToken);
    const isAuthenticated = useSelector(selectIsAuthenticated);

    // login
    const login = useCallback(async (credentials) => {
        const res = await axiosRequester.post("/auth/login", credentials, {
            withCredentials: true,
        });

        const { accessToken, user } = res.data;

        dispatch(setCredentials({ token: accessToken, user }));

        return res.data;
    }, [dispatch]);

    // logout
    const handleLogout = useCallback(() => {
        dispatch(logout());
        axiosRequester.post("/auth/logout", {}, { withCredentials: true });
    }, [dispatch]);

    return {
        user,
        token,
        isAuthenticated,
        login,
        logout: handleLogout,
    };
};

export default useAuth;

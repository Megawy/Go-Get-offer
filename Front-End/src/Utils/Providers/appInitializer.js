"use client";

import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import axiosRequester from "@/lib/Axios/axios";
import { setCredentials, logout } from "@/Redux/Slices/authSlice";
import { jwtDecode } from "jwt-decode";
import { useRouter } from "next/navigation";

const AppInitializer = ({ children }) => {
    const dispatch = useDispatch();
    const router = useRouter();
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const rehydrateAuth = async () => {
            try {
                const { data } = await axiosRequester.post(
                    "/api/Auth/refresh-token",
                    {},
                    { withCredentials: true }
                );

                console.log("🔄 Refresh Response:", data);

                // ✅ الوصول الصحيح للـ accessToken
                const newAccessToken = data.data?.accessToken;
                if (!newAccessToken) {
                    dispatch(logout());
                    return;
                }

                const decoded = jwtDecode(newAccessToken);
                console.log("✅ Decoded:", decoded);

                dispatch(
                    setCredentials({
                        token: newAccessToken,
                        user: decoded.user || null,
                        companyName: decoded.companyName || null,
                        emailAddress: decoded.emailAddress || null,
                        phoneNumber: decoded.phoneNumber || null,
                        role:
                            decoded[
                            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                            ] || null,
                    })
                );

            } catch (error) {
                console.log("❌ Rehydration failed:", error.response?.data || error.message);
                dispatch(logout());
                router.push("/login");
            } finally {
                setLoading(false);
            }
        };

        rehydrateAuth();
    }, [dispatch, router]);

    if (loading) {
        return <div className="w-full h-screen flex items-center justify-center">⏳ Loading...</div>;
    }

    return <>{children}</>;
};

export default AppInitializer;

import { useDispatch, useSelector } from "react-redux";
import {
    setCredentials,
    logout,
    selectCurrentUser,
    selectCurrentToken,
    selectIsAuthenticated,
    selectCurrentCompanyName,
    selectCurrentEmailAddress,
    selectCurrentPhoneNumber,
    selectCurrentRole,
} from "@/Redux/Slices/authSlice";
import { useCallback } from "react";
import { useMutation } from "@tanstack/react-query";
import axiosRequester from "@/lib/Axios/axios";
import { jwtDecode } from "jwt-decode";
import { useRouter } from "next/navigation";
import { appRoles } from "@/Services/routeGate";

const useAuth = () => {
    const dispatch = useDispatch();
    const router = useRouter();

    // Selectors
    const user = useSelector(selectCurrentUser);
    const token = useSelector(selectCurrentToken);
    const isAuthenticated = useSelector(selectIsAuthenticated);
    const companyName = useSelector(selectCurrentCompanyName);
    const emailAddress = useSelector(selectCurrentEmailAddress);
    const phoneNumber = useSelector(selectCurrentPhoneNumber);
    const role = useSelector(selectCurrentRole);

    // ✅ Login Mutation
    const loginMutation = useMutation({
        mutationFn: async (credentials) => {
            const res = await axiosRequester.post("/api/Auth/login", credentials, {
                withCredentials: true,
            });
            return res.data;
        },
        onSuccess: (data) => {
    const accessToken = data?.data?.accessToken;
    if (!accessToken) return;

    const decoded = jwtDecode(accessToken);

    dispatch(
        setCredentials({
            token: accessToken,
            user: decoded.user || null,
            companyName: decoded.companyName || null,
            emailAddress: decoded.emailAddress || null,
            phoneNumber: decoded.phoneNumber || null,
            role:
                decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
                null,
            isEmailConfirmed: decoded.isEmailConfirmed ?? null,
        })
    );

    const roleFromToken =
        decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    const isEmailConfirmed = decoded.isEmailConfirmed;

    if (!roleFromToken || roleFromToken === appRoles.User) {
        if (!isEmailConfirmed) {
            router.replace("/email-verification-confirmation");
        } else {
            router.replace("/role-select");
        }
        return;
    }

    if (roleFromToken && roleFromToken !== "User") {
        switch (roleFromToken) {
            case appRoles.Admin:
                router.replace("/admin/");
                break;
            case appRoles.Supplier:
                router.replace("/supplier/");
                break;
            case appRoles.Client:
                router.replace("/client/");
                break;
            default:
                router.replace("/");
        }
    }
}

    });

    // ✅ Signup Mutation
    const signupMutation = useMutation({
        mutationFn: async (newUser) => {
            const res = await axiosRequester.post("/api/Auth/register", newUser, {
                withCredentials: true,
            });
            return res.data;
        },
        onSuccess: (data) => {
            const accessToken = data?.data?.accessToken;
            if (!accessToken) return;

            const decoded = jwtDecode(accessToken);
            console.log("Decoded Token after Signup:", decoded);
            dispatch(
                setCredentials({
                    token: accessToken,
                    user: decoded.user || null,
                    companyName: decoded.companyName || null,
                    emailAddress: decoded.emailAddress || null,
                    phoneNumber: decoded.phoneNumber || null,
                    role:
                        decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
                        null,
                    isEmailConfirmed: decoded.isEmailConfirmed ?? null,
                })
            );
            router.push("/email-verification-confirmation");
            console.log("🟢 Signup Success. User is now logged in, redirecting to email verification confirmation.");
        },
    });

    // ✅ Logout Mutation
    const logoutMutation = useMutation({
        mutationFn: async () => {
            await axiosRequester.post("/api/Auth/logout", {}, { withCredentials: true });
        },
        onSuccess: () => {
            dispatch(logout());
            router.push("/login");
            console.log("🟢 Logged out successfully.", logoutMutation);
        },
    });

    // Wrappers
    const login = useCallback(
        async (credentials) => loginMutation.mutateAsync(credentials),
        [loginMutation]
    );

    const signup = useCallback(
        async (newUser) => signupMutation.mutateAsync(newUser),
        [signupMutation]
    );

    const handleLogout = useCallback(() => {
        logoutMutation.mutate();
    }, [logoutMutation]);

    return {
        user,
        token,
        isAuthenticated,
        companyName,
        emailAddress,
        phoneNumber,
        role,

        login,
        signup,
        logout: handleLogout,

        // Login states
        loginLoading: loginMutation.isLoading,
        loginError: loginMutation.error,

        // Signup states
        signupLoading: signupMutation.isLoading,
        signupError: signupMutation.error,

        // Logout state
        logoutLoading: logoutMutation.isLoading,
    };
};

export default useAuth;

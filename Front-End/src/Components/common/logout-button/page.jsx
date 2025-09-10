"use client";
import useAuth from "@/Hooks/useAuth";

const LogoutButton = () => {
    const { logout, logoutLoading } = useAuth();

    return (
        <button
            onClick={logout}
            disabled={logoutLoading}
            className="px-4 py-2 bg-go-primary-g f text-go-primary-o rounded"
        >
            {logoutLoading ? "Logging out..." : "Logout"}
        </button>
    );
};

export default LogoutButton;

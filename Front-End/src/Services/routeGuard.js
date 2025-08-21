// ✅ RouteGuard.jsx
"use client";
import { useEffect } from "react";
import { useRouter, usePathname } from "next/navigation";
import useAuth from "@/Hooks/useAuth";
import { routesConfig } from "../Services/routeGate"; // نفس اللي عندك

const RouteGuard = ({ children }) => {
    const { isAuthenticated } = useAuth();
    const router = useRouter();
    const pathname = usePathname();

    const currentRoute = routesConfig[pathname];

    useEffect(() => {
        // لو الصفحة مش موجودة في config اعتبرها Protected by default
        if (currentRoute?.isProtected !== false && !isAuthenticated) {
            router.push("/login");
        }
    }, [isAuthenticated, router, currentRoute]);

    if (currentRoute?.isProtected !== false && !isAuthenticated) {
        return <p className="text-center mt-10">🔒 Redirecting to login...</p>;
    }

    return children;
};

export default RouteGuard;

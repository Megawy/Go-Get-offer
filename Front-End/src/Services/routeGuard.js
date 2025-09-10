"use client";
import { useEffect, useState } from "react";
import { useRouter, usePathname } from "next/navigation";
import useAuth from "@/Hooks/useAuth";
import { routeGate, appRoles } from "@/Services/routeGate";

const RouteGuard = ({ children }) => {
    const { isAuthenticated, role, isEmailConfirmed} = useAuth();
    const router = useRouter();
    const pathname = usePathname();
    const [checking, setChecking] = useState(true);

    useEffect(() => {
        // 1️⃣ Public routes → Always accessible
        if (routeGate.public.includes(pathname)) {
            setChecking(false);
            return;
        }

        // 2️⃣ User not logged in → redirect to login
        if (!isAuthenticated) {
            router.replace("/login");
            setChecking(false);
            return;
        }

        // 3️⃣ User with no valid role → must select role
        if (role === null || role === appRoles.User) {
            if (!isEmailConfirmed) {
                router.replace("/email-verification-confirmation");
            } else if (pathname !== "/role-select") {
                router.replace("/role-select");
            }
            setChecking(false);
            return;
        }

        // 4️⃣ Exceptions (exact paths)
        const exception = routeGate.exceptions.find((ex) => ex.path === pathname);
        if (exception) {
            if (exception.isProtected && !exception.roles.includes(role)) {
                router.replace("/");
            }
            setChecking(false);
            return;
        }

        // 5️⃣ Prefix rules (role-based sections)
        const prefixRule = routeGate.prefixes.find((pr) =>
            pathname.startsWith(pr.prefix)
        );
        if (prefixRule && !prefixRule.roles.includes(role)) {
            router.replace("/");
            setChecking(false);
            return;
        }

        // 6️⃣ Default: if not matched → protected
        setChecking(false);
    }, [isAuthenticated, role, pathname, router]);

    if (checking) {
        return <p className="text-center mt-10">🔄 Loading...</p>;
    }

    return children;
};

export default RouteGuard;

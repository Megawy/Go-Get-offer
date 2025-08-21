'use client'

import useAuth from "@/Hooks/useAuth";

export default function UsersList() {
    const { user, token, isAuthenticated, companyName, emailAddress, role } = useAuth();

    console.log("👤 user:", user);
    console.log("🔑 token:", token);
    console.log("✅ isAuthenticated:", isAuthenticated);

    return (
        <div>
            <h1>Hi {companyName || "Guest"}</h1>
            {isAuthenticated && <p>Welcome back {emailAddress} (role: {role})</p>}
        </div>
    )
}

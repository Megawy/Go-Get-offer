"use client";
import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import { useFetch } from "@/Hooks/useFetch";
import { appRoles } from "@/Services/routeGate";
import useAuth from "@/Hooks/useAuth";

export default function RoleSelectPage() {
    const roleName = useAuth().role;
    const id = useAuth().user;
    const [selectedRole, setSelectedRole] = useState("");
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState(null);
    const router = useRouter();
    const { logout, logoutLoading } = useAuth();

    // 🚀 Redirect if role already exists

    // useEffect(() => {
    //     if (roleName && roleName !== "User") {
    //         switch (roleName) {
    //             case appRoles.Admin:
    //                 router.replace("/admin/");
    //                 break;
    //             case appRoles.Supplier:
    //                 router.replace("/supplier/");
    //                 break;
    //             case appRoles.Client:
    //                 router.replace("/client/");
    //                 break;
    //             default:
    //                 router.replace("/");
    //         }
    //     }
    // }, [roleName, router]);

    // ✅ Handle role change (PUT request with Promise)

    const handleSubmit = async () => {
        if (!selectedRole) return;
        setSubmitting(true);
        setError(null);

        try {
            const result = await useFetch("/api/Admin/profile/role-change", {
                method: "PUT",
                data: { id, roleName: selectedRole },
            });
            console.log(result)
            if (result.status) {
                logout();
                router.replace("/login");
            } else {
                setError(result.message || "Failed to update role");
            }
        } catch (err) {
            setError(err.message || "Something went wrong");
        } finally {
            setSubmitting(false);
        }
    };

    // Show UI if still "User"

    if (roleName && roleName !== "User") {
        return <p className="text-center mt-10">Redirecting based on your role...</p>;
    }

    return (
        <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 px-4">
            <div className="bg-white shadow-lg rounded-2xl p-6 w-full max-w-md">
                <h1 className="text-2xl font-bold mb-4 text-center">Select Your Role</h1>

                {/* Roles Dropdown */}

                <select
                    className="border rounded-lg px-4 py-2 w-full"
                    defaultValue=""
                    onChange={(e) => setSelectedRole(e.target.value)}
                >
                    <option value="" disabled>
                        Select your role
                    </option>
                    {Object.values(appRoles)
                        .filter((r) => r === appRoles.Client || r === appRoles.Supplier)
                        .map((r) => (
                            <option key={r} value={r}>
                                {r}
                            </option>
                        ))}
                </select>

                {/* Error message */}

                {error && <p className="text-red-500 text-sm mt-2">{error}</p>}

                {/* Submit Button */}
                <button
                    onClick={handleSubmit}
                    disabled={submitting || !selectedRole}
                    className="mt-4 w-full bg-blue-600 text-white py-2 px-4 rounded-lg hover:bg-blue-700 
                    disabled:opacity-50"
                >
                    {submitting ? "Saving..." : "Confirm Role"}
                </button>
            </div>
        </div>
    );
}

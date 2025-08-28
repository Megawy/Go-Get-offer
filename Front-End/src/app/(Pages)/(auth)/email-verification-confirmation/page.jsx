"use client";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { useFetch } from "@/Hooks/useFetch";
import { getErrorMessage } from "@/Services/errorHandler"; 

export default function EmailVerificationConfirmation() {
    const router = useRouter();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const handleSendVerification = async () => {
        setLoading(true);
        setError(null);

        try {
            const result = await useFetch("/api/EmailVerification/send", {
                method: "POST",
            });

            if (result.status) {
                router.push("/verify-otp");
                console.log("✅ Verification sent:", result);
            } else {
                setError(result.message || "Failed to send verification email");
            }
        } catch (err) {
            // 👇 نفس المعالجة في أي صفحة تانية
            setError(getErrorMessage(err));
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 px-4">
            <div className="bg-white shadow-lg rounded-2xl p-6 w-full max-w-md text-center">
                <h1 className="text-2xl font-bold mb-4">Verify Your Email</h1>
                <p className="mb-6 text-gray-600">
                    Please Verify your email address to continue.
                </p>

                {error && <p className="text-red-500 text-sm mb-4">{error}</p>}

                <button
                    onClick={handleSendVerification}
                    disabled={loading}
                    className="w-full bg-blue-600 text-white py-2 px-4 rounded-lg hover:bg-blue-700 disabled:opacity-50"
                >
                    {loading ? "Sending..." : "Send Verification Email"}
                </button>

                <button
                    onClick={() => router.push("/sign-up")}
                    className="mt-4 w-full bg-gray-200 text-gray-800 py-2 px-4 rounded-lg hover:bg-gray-300"
                >
                    Back to Signup
                </button>
            </div>
        </div>
    );
}

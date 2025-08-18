"use client";

import { useState } from "react";
import { useDispatch } from "react-redux";
import { setCredentials } from "@/Redux/Slices/authSlice";
import { useQueryFetch } from "@/Hooks/useQueryFetch";

export default function LoginPage() {
    const dispatch = useDispatch();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [trigger, setTrigger] = useState(false); // لتفعيل query بعد الضغط على Login
    const [error, setError] = useState("");

    const { data, refetch, isFetching } = useQueryFetch(
        ["login", email, password],
        "/auth/login",
        { method: "POST", data: { email, password } },
        { enabled: false, suspense: true }
    );

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const res = await refetch(); 
            const { user, accessToken } = res.data;

            dispatch(setCredentials({ user, token: accessToken }));
            setError(""); // لو نجح، امسح أي error
        } catch (err) {
            console.error(err);
            setError(err.response?.data?.message || "Login failed");
        }
    };

    return (
        <div className="flex flex-col items-center justify-center min-h-screen">
            <form className="flex flex-col gap-2 w-80" onSubmit={handleLogin}>
                <input
                    type="email"
                    placeholder="Email"
                    value={email}
                    onChange={e => setEmail(e.target.value)}
                    className="border p-2 rounded"
                    required
                />
                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                    className="border p-2 rounded"
                    required
                />
                <button
                    type="submit"
                    className="bg-blue-500 text-white p-2 rounded"
                    disabled={isFetching}
                >
                    {isFetching ? "Logging in..." : "Login"}
                </button>
                {error && <p className="text-red-500">{error}</p>}
            </form>

            {data && (
                <div className="mt-4 p-2 border rounded">
                    <p>Logged in as: {data.user?.name}</p>
                    <p>Token: {data.accessToken}</p>
                </div>
            )}
        </div>
    );
}

"use client";

import { useState } from "react";
import useForm from "@/Hooks/useForm.js";
import { loginSchema } from "@/Utils/Validation/ValidationSchemas.js";
import { RiEyeCloseLine } from "react-icons/ri";
import { BsEyeFill } from "react-icons/bs";
import useAuth from "@/Hooks/useAuth.js";
import { getErrorMessage } from "@/Services/errorHandler.js"; // ✅ استدعاء util
import { Button } from "@/Components/chadcn-ui/button";
import { useRouter } from "next/navigation";

export default function LoginPage() {
    const initialValues = {
        email: "",
        passwordHash: "",
    };

    const [showPassword, setShowPassword] = useState(false);
    const [serverError, setServerError] = useState({ email: "", passwordHash: "" });
    const router = useRouter();
    const { login } = useAuth();

    const onSubmit = async (values) => {
        try {
            setServerError({ email: "", passwordHash: "" });

            const res = await login(values);
            console.log("✅ Login success:", res);

        } catch (err) {
            console.error("❌ Login error:", err);

            const message = getErrorMessage(err);

            if (err.response?.status === 400) {
                setServerError((prev) => ({ ...prev, email: message }));
            } else if (err.response?.status === 401) {
                setServerError((prev) => ({ ...prev, passwordHash: message }));
            } else {
                setServerError((prev) => ({
                    ...prev,
                    passwordHash: message,
                }));
            }
        }
    };

    const formik = useForm(initialValues, loginSchema, onSubmit);

    return (
        <div className="flex flex-col items-center justify-center min-h-screen">
            <form
                className="flex flex-col gap-4 w-80"
                onSubmit={formik.handleSubmit}
                noValidate
            >
                {/* Email */}
                <div>
                    <input
                        type="email"
                        name="email"
                        placeholder="Email"
                        value={formik.values.email}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        className="border p-2 rounded w-full"
                    />
                    {formik.errors.email && formik.touched.email && (
                        <p className="text-red-500 text-sm">{formik.errors.email}</p>
                    )}
                    {serverError.email && (
                        <p className="text-red-500 text-sm">{serverError.email}</p>
                    )}
                </div>

                {/* Password with visibility toggle */}
                <div className="relative">
                    <input
                        type={showPassword ? "text" : "password"}
                        name="passwordHash"
                        placeholder="Password"
                        value={formik.values.passwordHash}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        className="border p-2 rounded w-full pr-10"
                    />
                    <span
                        className="absolute right-3 top-1/2 -translate-y-1/2 cursor-pointer text-gray-600"
                        onClick={() => setShowPassword((prev) => !prev)}
                    >
                        {showPassword ? (
                            <BsEyeFill size={20} />
                        ) : (
                            <RiEyeCloseLine size={20} />
                        )}
                    </span>
                </div>

                {/* Errors */}
                {formik.errors.passwordHash && formik.touched.passwordHash && (
                    <p className="text-red-500 text-sm">{formik.errors.passwordHash}</p>
                )}
                {serverError.passwordHash && (
                    <p className="text-red-500 text-sm">{serverError.passwordHash}</p>
                )}

                {/* Submit */}
                <button
                    type="submit"
                    className="bg-blue-500 text-white p-2 rounded"
                >
                    Submit
                </button>
                {/* Reset Password Button */}
                <Button variant="link" type="button" className="text-blue-500 hover:bg-blue-500 hover:text-white p-0" onClick={() => (router.push("/forget-password"))}>
                    Forgot Password?
                </Button>
            </form>
        </div>
    );
}

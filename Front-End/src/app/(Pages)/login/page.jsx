"use client";

import { useState } from "react";
import useForm from "@/Hooks/useForm.js";
import { loginSchema } from "@/Utils/Validation/ValidationSchemas.js";
import { RiEyeCloseLine } from "react-icons/ri";
import { BsEyeFill } from "react-icons/bs";
import useAuth from "@/Hooks/useAuth.js";
import { useDispatch } from "react-redux";
import { setCredentials } from "@/Redux/Slices/authSlice";

export default function LoginPage() {
    const initialValues = {
        email: "",
        passwordHash: "",
    };

    const [showPassword, setShowPassword] = useState(false);
    const [serverError, setServerError] = useState({ email: "", passwordHash: "" });

    const { login } = useAuth();
    const dispatch = useDispatch();

const onSubmit = async (values) => {
    try {
        setServerError({ email: "", passwordHash: "" });

        // ⬅️ login بيرجع response.data
        const data = await login(values);

        // backend المفروض يديك accessToken
        const { accessToken, user } = data;  

        dispatch(setCredentials({
            user: user || null,
            token: accessToken
        }));

        console.log("✅ Login success:", data);
    } catch (err) {
        console.error("❌ Login error:", err);

        if (err.response?.status === 400) {
            setServerError((prev) => ({
                ...prev,
                email: err.response?.data?.message || "Invalid email",
            }));
        } else if (err.response?.status === 401) {
            setServerError((prev) => ({
                ...prev,
                passwordHash: err.response?.data?.message || "Invalid password",
            }));
        } else {
            setServerError((prev) => ({
                ...prev,
                passwordHash: "Something went wrong. Try again later.",
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

                {/* خلي الـ errors بره الـ relative */}
                {formik.errors.passwordHash && formik.touched.passwordHash && (
                    <p className="text-red-500 text-sm">{formik.errors.passwordHash}</p>
                )}
                {serverError.password && (
                    <p className="text-red-500 text-sm">{serverError.passwordHash}</p>
                )}

                {/* Submit */}
                <button
                    type="submit"
                    className="bg-blue-500 text-white p-2 rounded"
                >
                    Submit
                </button>
            </form>
        </div>
    );
}

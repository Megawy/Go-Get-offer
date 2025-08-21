"use client";

import { useState } from "react";
import useForm from "@/Hooks/useForm.js";
import { signupSchema } from "@/Utils/Validation/ValidationSchemas.js";
import { RiEyeCloseLine } from "react-icons/ri";
import { BsEyeFill } from "react-icons/bs";
import useAuth from "@/Hooks/useAuth.js";

export default function SignupPage() {
    const initialValues = {
        email: "",
        phoneNumber: "",
        companyName: "",
        passwordHash: "",
        confirmPassword: "", // 🆕 أضفنا confirmPassword
    };

    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false); // 🆕 زر إظهار/إخفاء confirmPassword
    const [serverError, setServerError] = useState({
        email: "",
        phoneNumber: "",
        companyName: "",
        passwordHash: "",
        confirmPassword: "",
    });

    const { signup } = useAuth();

    const onSubmit = async (values) => {
        const { confirmPassword, ...payload } = values;
        try {
            setServerError({
                email: "",
                phoneNumber: "",
                companyName: "",
                passwordHash: "",
                confirmPassword: "",
            });

            console.log("🚀 Signup Submit Triggered:", values);
            await signup(payload);

            console.log("✅ Signup success:", values);
        } catch (err) {
            console.error("❌ Signup error:", err);

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

    const { values, errors, touched, handleChange, handleBlur, handleSubmit } =
        useForm(initialValues, signupSchema, onSubmit);

    return (
        <div className="flex flex-col items-center justify-center min-h-screen">
            <form
                className="flex flex-col gap-4 w-80"
                onSubmit={handleSubmit}
                noValidate
            >
                {/* Email */}
                <div>
                    <input
                        type="email"
                        name="email"
                        placeholder="Email"
                        value={values.email}
                        onChange={handleChange}
                        onBlur={handleBlur}
                        className="border p-2 rounded w-full"
                    />
                    {errors.email && touched.email && (
                        <p className="text-red-500 text-sm">{errors.email}</p>
                    )}
                    {serverError.email && (
                        <p className="text-red-500 text-sm">{serverError.email}</p>
                    )}
                </div>

                {/* Phone Number */}
                <div>
                    <input
                        type="text"
                        name="phoneNumber"
                        placeholder="Phone Number"
                        value={values.phoneNumber}
                        onChange={handleChange}
                        onBlur={handleBlur}
                        className="border p-2 rounded w-full"
                    />
                    {errors.phoneNumber && touched.phoneNumber && (
                        <p className="text-red-500 text-sm">{errors.phoneNumber}</p>
                    )}
                    {serverError.phoneNumber && (
                        <p className="text-red-500 text-sm">{serverError.phoneNumber}</p>
                    )}
                </div>

                {/* Company Name */}
                <div>
                    <input
                        type="text"
                        name="companyName"
                        placeholder="Company Name"
                        value={values.companyName}
                        onChange={handleChange}
                        onBlur={handleBlur}
                        className="border p-2 rounded w-full"
                    />
                    {errors.companyName && touched.companyName && (
                        <p className="text-red-500 text-sm">{errors.companyName}</p>
                    )}
                    {serverError.companyName && (
                        <p className="text-red-500 text-sm">{serverError.companyName}</p>
                    )}
                </div>

                {/* Password */}
                <div className="relative">
                    <input
                        type={showPassword ? "text" : "password"}
                        name="passwordHash"
                        placeholder="Password"
                        value={values.passwordHash}
                        onChange={handleChange}
                        onBlur={handleBlur}
                        className="border p-2 rounded w-full pr-10"
                    />
                    <span
                        className="absolute right-3 top-1/2 -translate-y-1/2 cursor-pointer text-gray-600"
                        onClick={() => setShowPassword((prev) => !prev)}
                    >
                        {showPassword ? <BsEyeFill size={20} /> : <RiEyeCloseLine size={20} />}
                    </span>
                </div>
                {errors.passwordHash && touched.passwordHash && (
                    <p className="text-red-500 text-sm">{errors.passwordHash}</p>
                )}
                {serverError.passwordHash && (
                    <p className="text-red-500 text-sm">{serverError.passwordHash}</p>
                )}

                {/* Confirm Password 🆕 */}
                <div className="relative">
                    <input
                        type={showConfirmPassword ? "text" : "password"}
                        name="confirmPassword"
                        placeholder="Confirm Password"
                        value={values.confirmPassword}
                        onChange={handleChange}
                        onBlur={handleBlur}
                        className="border p-2 rounded w-full pr-10"
                    />
                    <span
                        className="absolute right-3 top-1/2 -translate-y-1/2 cursor-pointer text-gray-600"
                        onClick={() => setShowConfirmPassword((prev) => !prev)}
                    >
                        {showConfirmPassword ? (
                            <BsEyeFill size={20} />
                        ) : (
                            <RiEyeCloseLine size={20} />
                        )}
                    </span>
                </div>
                {errors.confirmPassword && touched.confirmPassword && (
                    <p className="text-red-500 text-sm">{errors.confirmPassword}</p>
                )}
                {serverError.confirmPassword && (
                    <p className="text-red-500 text-sm">{serverError.confirmPassword}</p>
                )}

                {/* Submit */}
                <button type="submit" className="bg-blue-600 text-white p-2 rounded">
                    Sign Up
                </button>
            </form>
        </div>
    );
}

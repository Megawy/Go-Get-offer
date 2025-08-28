"use client";

import { useState } from "react";
import useForm from "@/Hooks/useForm.js";
import { signupSchema} from "@/Utils/Validation/ValidationSchemas.js";
import { RiEyeCloseLine } from "react-icons/ri";
import { BsEyeFill } from "react-icons/bs";
import useAuth from "@/Hooks/useAuth.js";
import { normalizeErrors } from "@/Services/errorNormalizer";

export default function SignupPage() {
    const initialValues = {
        email: "",
        phoneNumber: "",
        companyName: "",
        passwordHash: "",
        confirmPassword: "",
    };

    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);

    const { signup, signupError, signupLoading } = useAuth();

    const onSubmit = async (values) => {
        const { confirmPassword, ...payload } = values;
        await signup(payload); // ✅ useAuth بيتكفل بالـ dispatch + token + redirect
    };

    const { values, errors, touched, handleChange, handleBlur, handleSubmit } =
        useForm(initialValues, signupSchema, onSubmit);

    // 🛠️ Normalize server errors
    const serverErrors = normalizeErrors(signupError);

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
                    {serverErrors.email && (
                        <p className="text-red-500 text-sm">{serverErrors.email}</p>
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
                    {serverErrors.phoneNumber && (
                        <p className="text-red-500 text-sm">{serverErrors.phoneNumber}</p>
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
                    {serverErrors.companyName && (
                        <p className="text-red-500 text-sm">{serverErrors.companyName}</p>
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
                {serverErrors.passwordHash && (
                    <p className="text-red-500 text-sm">{serverErrors.passwordHash}</p>
                )}

                {/* Confirm Password */}
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

                {/* General Error */}
                {serverErrors.general && (
                    <p className="text-red-500 text-sm text-center">{serverErrors.general}</p>
                )}

                {/* Submit */}
                <button
                    type="submit"
                    disabled={signupLoading}
                    className="bg-blue-600 text-white p-2 rounded disabled:opacity-50"
                >
                    {signupLoading ? "Loading..." : "Sign Up"}
                </button>
            </form>
        </div>
    );
}

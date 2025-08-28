"use client";

import { useState } from "react";
import useForm from "@/Hooks/useForm";
import { useFetch } from "@/Hooks/useFetch";
import { resetPasswordSchema } from "@/Utils/Validation/ValidationSchemas";
import { getErrorMessage } from "@/Services/errorHandler";
import { normalizeErrors } from "@/Services/errorNormalizer";
import OTPInput from "@/Components/chadcn-ui/otp/otpInput";
import { useModal, ModalActions } from "@/Hooks/useModal";
import { BsEyeFill } from "react-icons/bs";
import { RiEyeCloseLine } from "react-icons/ri";
import { useSelector } from "react-redux";

export default function ResetPasswordPage() {
    const { openModal } = useModal();
    const [serverError, setServerError] = useState("");
    const [showNewPassword, setShowNewPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);
    const storedEmail = useSelector((state) => state.auth.resetEmail);

    const initialValues = { email: "", Otp: "", NewPassword: "", confirmPassword: "" };

    // Handle submit
    const onSubmit = async (values) => {
        try {
            setServerError("");

            if (storedEmail && values.email !== storedEmail) {
                return setServerError("❌ The email must match the one used to request reset code.");
            }

            const response = await useFetch("/api/profile/password/reset", {
                method: "POST",
                data: {
                    email: values.email,
                    ResetCode: values.Otp,        // <-- API expects ResetCode
                    NewPassword: values.NewPassword,
                },
            });

            if (response?.status) {
                openModal({
                    type: "success",
                    title: "Success",
                    message: "Password reset successfully!",
                    actionType: ModalActions.LOGIN, // GlobalModal will route on OK
                });
            } else {
                openModal({
                    type: "failure",
                    title: "Error",
                    message: response?.message || "Something went wrong",
                });
            }
        } catch (err) {
            // Normalize server errors and map to form fields
            const normalized = normalizeErrors(err) || {};
            const newErrors = {};

            // Map possible keys from backend to our form fields
            if (normalized.email) newErrors.email = normalized.email;
            if (normalized.Otp || normalized.ResetCode)
                newErrors.Otp = normalized.Otp || normalized.ResetCode;
            if (normalized.NewPassword || normalized.passwordHash)
                newErrors.NewPassword = normalized.NewPassword || normalized.passwordHash;
            if (normalized.confirmPassword || normalized.ConfirmPassword)
                newErrors.confirmPassword = normalized.confirmPassword || normalized.ConfirmPassword;

            if (Object.keys(newErrors).length > 0) {
                formik.setErrors(newErrors);
            } else if (normalized.general) {
                setServerError(normalized.general);
            } else {
                setServerError(getErrorMessage(err, "Something went wrong"));
            }
        }
    };

    const formik = useForm(initialValues, resetPasswordSchema, onSubmit);

    return (
        <div className="flex flex-col items-center justify-center min-h-screen px-4">
            <form
                className="bg-white shadow-lg rounded-2xl p-6 w-full max-w-md flex flex-col gap-6"
                onSubmit={formik.handleSubmit}
                noValidate
            >
                <h1 className="text-2xl font-bold text-center">Reset Password</h1>

                {/* Email */}
                <input
                    type="email"
                    name="email"
                    placeholder="Enter your email"
                    value={formik.values.email}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    className="border rounded-lg px-4 py-2"
                />
                {formik.errors.email && formik.touched.email && (
                    <p className="text-red-500 text-sm">{formik.errors.email}</p>
                )}

                {/* New Password with toggler */}
                <div className="relative">
                    <input
                        type={showNewPassword ? "text" : "password"}
                        name="NewPassword"
                        placeholder="New Password"
                        value={formik.values.NewPassword}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        className="border rounded-lg px-4 py-2 pr-10 w-full"
                    />
                    <span
                        className="absolute right-3 top-1/2 -translate-y-1/2 cursor-pointer text-gray-600"
                        onClick={() => setShowNewPassword((p) => !p)}
                    >
                        {showNewPassword ? <BsEyeFill size={20} /> : <RiEyeCloseLine size={20} />}
                    </span>
                </div>
                {formik.errors.NewPassword && formik.touched.NewPassword && (
                    <p className="text-red-500 text-sm">{formik.errors.NewPassword}</p>
                )}

                {/* Confirm Password with toggler */}
                <div className="relative">
                    <input
                        type={showConfirmPassword ? "text" : "password"}
                        name="confirmPassword"
                        placeholder="Confirm New Password"
                        value={formik.values.confirmPassword}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        className="border rounded-lg px-4 py-2 pr-10 w-full"
                    />
                    <span
                        className="absolute right-3 top-1/2 -translate-y-1/2 cursor-pointer text-gray-600"
                        onClick={() => setShowConfirmPassword((p) => !p)}
                    >
                        {showConfirmPassword ? <BsEyeFill size={20} /> : <RiEyeCloseLine size={20} />}
                    </span>
                </div>
                {formik.errors.confirmPassword && formik.touched.confirmPassword && (
                    <p className="text-red-500 text-sm">{formik.errors.confirmPassword}</p>
                )}

                {/* General server error */}
                {serverError && <p className="text-red-500 text-sm">{serverError}</p>}

                {/* OTP */}
                <OTPInput
                    value={formik.values.Otp}
                    onChange={(val) => formik.handleChange({ target: { name: "Otp", value: val } })}
                />
                {formik.errors.Otp && formik.touched.Otp && (
                    <p className="text-red-500 text-sm">{formik.errors.Otp}</p>
                )}

                {/* Submit */}
                <button
                    type="submit"
                    disabled={formik.isSubmitting}
                    className="w-full bg-blue-600 text-white py-2 px-4 rounded-lg hover:bg-blue-700 disabled:opacity-50"
                >
                    {formik.isSubmitting ? "Resetting..." : "Reset Password"}
                </button>
            </form>
        </div>
    );
}

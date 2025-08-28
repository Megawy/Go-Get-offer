"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import useForm from "@/Hooks/useForm";
import { otpVerificationSchema } from "@/Utils/Validation/ValidationSchemas";
import OTPInput from "@/Components/chadcn-ui/otp/otpInput";
import { useFetch } from "@/Hooks/useFetch";
import { useDispatch } from "react-redux";
import { setIsEmailConfirmed } from "@/Redux/Slices/authSlice";
import { normalizeErrors } from "@/Services/errorNormalizer";
import { getErrorMessage } from "@/Services/errorHandler";

export default function OtpConfirmPage() {
    const router = useRouter();
    const [serverError, setServerError] = useState("");
    const dispatch = useDispatch();

    const initialValues = { Otp: "" };

    const onSubmit = async (values) => {
        try {
            setServerError("");

            const response = await useFetch("/api/EmailVerification/confirm", {
                method: "POST",
                data: { Otp: values.Otp },
            });

            if (response.status) {
                console.log("✅ OTP Verified:", response);
                dispatch(setIsEmailConfirmed(true));
                router.replace("/role-select");
            } else {
                setServerError(response.message || "Invalid OTP");
            }
        } catch (err) {
            const normalized = normalizeErrors(err);
            if (normalized.Otp) {
                formik.setErrors({ Otp: normalized.Otp });
            } else if (normalized.general) {
                setServerError(normalized.general);
            } else {
                setServerError(getErrorMessage(err, "Something went wrong"));
            }
        }
    };


    const formik = useForm(initialValues, otpVerificationSchema, onSubmit);

    return (
        <div className="flex flex-col items-center justify-center min-h-screen px-4">
            <form
                className="bg-white shadow-lg rounded-2xl p-6 w-full max-w-md flex flex-col gap-6"
                onSubmit={formik.handleSubmit}
                noValidate
            >
                <h1 className="text-2xl font-bold text-center">Enter OTP</h1>

                <OTPInput
                    value={formik.values.Otp}
                    onChange={(val) =>
                        formik.handleChange({ target: { name: "Otp", value: val } })
                    }
                />

                {/* Validation + Server errors */}
                {formik.errors.Otp && formik.touched.Otp && (
                    <p className="text-red-500 text-sm text-center">{formik.errors.Otp}</p>
                )}
                {serverError && (
                    <p className="text-red-500 text-sm text-center">{serverError}</p>
                )}

                <button
                    type="submit"
                    disabled={formik.isSubmitting}
                    className="w-full bg-blue-600 text-white py-2 px-4 rounded-lg hover:bg-blue-700 disabled:opacity-50"
                >
                    {formik.isSubmitting ? "Verifying..." : "Confirm OTP"}
                </button>
            </form>
        </div>
    );
}

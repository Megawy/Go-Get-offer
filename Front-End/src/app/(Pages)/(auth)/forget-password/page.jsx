"use client";

import { useState } from "react";
import useForm from "@/Hooks/useForm";
import { useFetch } from "@/Hooks/useFetch";
import { getErrorMessage } from "@/Services/errorHandler";
import { normalizeErrors } from "@/Services/errorNormalizer";
import { forgetPasswordSchema } from "@/Utils/Validation/ValidationSchemas";
import { useModal, ModalActions } from "@/Hooks/useModal";
import { setResetEmail } from "@/Redux/Slices/authSlice";
import { useDispatch } from "react-redux";

export default function ForgetPasswordPage() {
    const { openModal } = useModal();
    const [serverError, setServerError] = useState("");
    const dispatch = useDispatch();
    const initialValues = { email: "" };

    const onSubmit = async (values) => {
        try {
            setServerError("");

            const response = await useFetch("/api/profile/password/forget", {
                method: "POST",
                data: { email: values.email },
            });

            if (response?.status) {
                dispatch(setResetEmail(values.email));
                console.log("✅ Forget password success:", response);
                openModal({
                    type: "success",
                    title: "Success",
                    message: "Check your email for the reset code.",
                    actionType: ModalActions.RESET_PASSWORD,
                });
            } else {
                openModal({
                    type: "failure",
                    title: "Error",
                    message: response?.message || "Something went wrong",
                });
            }
        } catch (err) {
            const normalized = normalizeErrors(err);

            if (normalized.email) {
                formik.setErrors({ email: normalized.email }); // ✅ استخدم formik مباشرة
            } else if (normalized.general) {
                setServerError(normalized.general);
            } else {
                setServerError(getErrorMessage(err, "Something went wrong"));
            }
        }
    };



    const formik = useForm(initialValues, forgetPasswordSchema, onSubmit);

    return (
        <div className="flex flex-col items-center justify-center min-h-screen px-4">
            <form
                className="bg-white shadow-lg rounded-2xl p-6 w-full max-w-md flex flex-col gap-6"
                onSubmit={formik.handleSubmit}
                noValidate
            >
                <h1 className="text-2xl font-bold text-center">Forgot Password</h1>

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
                {serverError && <p className="text-red-500 text-sm">{serverError}</p>}

                <button
                    type="submit"
                    disabled={formik.isSubmitting}
                    className="w-full bg-blue-600 text-white py-2 px-4 rounded-lg hover:bg-blue-700 disabled:opacity-50"
                >
                    {formik.isSubmitting ? "Sending..." : "Send Reset Code"}
                </button>
            </form>
        </div>
    );
}

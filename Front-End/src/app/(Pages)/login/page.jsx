"use client";

import { useState } from "react";
import useForm from "@/Hooks/useForm.js";
import { loginSchema } from "@/Utils/Validation/ValidationSchemas.js";

export default function LoginPage() {
    const initialValues = {
        email: "",
        password: "",
    };

    const [submitted, setSubmitted] = useState(false);

    const onSubmit = (values) => {
        console.log("Submitted values:", values);
        setSubmitted(true);
    };

    const formik = useForm(initialValues, loginSchema, onSubmit);

    return (
        <div className="flex flex-col items-center justify-center min-h-screen">
            <form
                className="flex flex-col gap-2 w-80"
                onSubmit={formik.handleSubmit}
                noValidate
            >
                {/* Email */}
                <input
                    type="email"
                    name="email"
                    placeholder="Email"
                    value={formik.values.email}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    className="border p-2 rounded"
                />
                {formik.errors.email && formik.touched.email && (
                    <p className="text-red-500 text-sm">{formik.errors.email}</p>
                )}

                {/* Password */}
                <input
                    type="password"
                    name="password"
                    placeholder="Password"
                    value={formik.values.password}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    className="border p-2 rounded"
                />
                {formik.errors.password && formik.touched.password && (
                    <p className="text-red-500 text-sm">{formik.errors.password}</p>
                )}

                {/* Submit */}
                <button
                    type="submit"
                    className="bg-blue-500 text-white p-2 rounded"
                >
                    Submit
                </button>
            </form>

            {/* Show values only if submitted */}
            {submitted && !Object.keys(formik.errors).length && (
                <div className="mt-4 p-2 border rounded">
                    <p>Logged in as: {formik.values.email}</p>
                    <p>
                        Token: #
                        {`5454123##5453adsa#da${formik.values.password}#65445d4fs5d`}
                    </p>
                </div>
            )}
        </div>
    );
}

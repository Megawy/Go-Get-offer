import { useState } from "react";

export default function useForm(initialValues, validationSchema, onSubmit) {
    const [values, setValues] = useState(initialValues);
    const [errors, setErrors] = useState({});
    const [touched, setTouched] = useState({});

    const handleChange = async (e) => {
        const { name, value } = e.target;
        setValues((prev) => ({ ...prev, [name]: value }));

        if (touched[name]) {
            try {
                await validationSchema.validateAt(name, { ...values, [name]: value });
                setErrors((prev) => ({ ...prev, [name]: "" }));
            } catch (err) {
                setErrors((prev) => ({ ...prev, [name]: err.message }));
            }
        }
    };

    const handleBlur = async (e) => {
        const { name } = e.target;
        setTouched((prev) => ({ ...prev, [name]: true }));

        try {
            await validationSchema.validateAt(name, values);
            setErrors((prev) => ({ ...prev, [name]: "" }));
        } catch (err) {
            setErrors((prev) => ({ ...prev, [name]: err.message }));
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        console.log("🔥 handleSubmit triggered");
        try {
            await validationSchema.validate(values, { abortEarly: false });
            setErrors({});
            console.log("✅ Validation Passed:", values);
            onSubmit(values);
        } catch (err) {
            console.error("❌ Validation Error (Full):", err);
            const newErrors = {};
            err.inner.forEach((error) => {
                newErrors[error.path] = error.message;
            });
            setErrors(newErrors);

            const allTouched = Object.keys(initialValues).reduce(
                (acc, key) => ({ ...acc, [key]: true }),
                {}
            );
            setTouched(allTouched);
        }
    };

    const resetForm = () => {
        setValues(initialValues);
        setErrors({});
        setTouched({});
    };

    return {
        values,
        errors,
        touched,
        handleChange,
        handleBlur,
        handleSubmit,
        resetForm,
        setErrors
    };
}

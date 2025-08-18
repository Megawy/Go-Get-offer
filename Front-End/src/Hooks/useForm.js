import { useState } from "react";
import * as yup from "yup";

export default function useForm(initialValues, validationSchema, onSubmit) {
    const [values, setValues] = useState(initialValues);
    const [errors, setErrors] = useState({});
    const [touched, setTouched] = useState({});

    // عشان تمسك القيم
    const handleChange = (e) => {
        const { name, value } = e.target;
        setValues((prev) => ({ ...prev, [name]: value }));
    };

    // عشان تعرف إمتى تعمل validate
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

    // لما تضغط submit
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await validationSchema.validate(values, { abortEarly: false });
            setErrors({});
            onSubmit(values);
        } catch (err) {
            const newErrors = {};
            err.inner.forEach((error) => {
                newErrors[error.path] = error.message;
            });
            setErrors(newErrors);
        }
    };

    return {
        values,
        errors,
        touched,
        handleChange,
        handleBlur,
        handleSubmit,
    };
}

import * as Yup from "yup";

const emailRule = Yup.string()
    .matches(
        /^[a-zA-Z0-9._%+-]+(\.[a-zA-Z0-9._%+-]+){0,1}@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}$/,
        "Invalid email format (max 2 dots allowed before @ and single domain required)"
    )
    .required("Email is required");

const passwordRule = Yup.string()
    .min(8, "Password must be at least 8 characters")
    .matches(/[A-Z]/, "Password must contain at least one uppercase letter")
    .matches(/[a-z]/, "Password must contain at least one lowercase letter")
    .matches(/\d/, "Password must contain at least one number")
    .matches(/[@$!%*?&]/, "Password must contain at least one special character")
    .required("Password is required");

const usernameRule = Yup.string()
    .min(3, "Username must be at least 3 characters")
    .max(20, "Username must be less than 20 characters")
    .required("Username is required");

export const loginSchema = Yup.object({
    email: emailRule,
    password: passwordRule,
});

export const registerSchema = Yup.object({
    username: usernameRule,
    email: emailRule,
    password: passwordRule,
    confirmPassword: Yup.string()
        .oneOf([Yup.ref("password"), null], "Passwords must match")
        .required("Confirm Password is required"),
});

export const rules = {
    emailRule,
    passwordRule,
    usernameRule,
};

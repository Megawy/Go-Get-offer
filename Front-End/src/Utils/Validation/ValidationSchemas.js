import * as Yup from "yup";

// ✅ Email Rule (gmail / yahoo / outlook فقط + domains: .com / .net / .edu / .org)
const emailRule = Yup.string()
    .matches(
        /^(?:[a-zA-Z0-9._%+-]+)@(gmail|yahoo|outlook)\.(com|net|edu|org)$/,
        "Email must be Gmail, Yahoo, or Outlook with valid domain (.com, .net, .edu, .org)"
    )
    .required("Email is required");

// ✅ Password Rule (Strong Password)
const passwordRule = Yup.string()
    .min(8, "Password must be at least 8 characters")
    .matches(/[A-Z]/, "Password must contain at least one uppercase letter")
    .matches(/[a-z]/, "Password must contain at least one lowercase letter")
    .matches(/\d/, "Password must contain at least one number")
    .matches(/[@$!%*?&]/, "Password must contain at least one special character")
    .trim()
    .required("Password is required");

// ✅ Confirm Password Rule
export const confirmPasswordRule = (refFieldName) =>
    Yup.string()
        .trim()
        .oneOf([Yup.ref(refFieldName), null], "Passwords must match")
        .required("Confirm Password is required");

// ✅ Company Name Rule
const companyNameRule = Yup.string()
    .min(2, "Company name must be at least 2 characters")
    .max(50, "Company name must be less than 50 characters")
    .required("Company name is required");

// ✅ Egyptian Phone Number Rule (010 / 011 / 012 / 015 ويكون 11 رقم)
const phoneNumberRule = Yup.string()
    .matches(/^(010|011|012|015)[0-9]{8}$/, "Invalid Egyptian phone number")
    .required("Phone number is required");

// ✅ Otp formula
const otpRule = Yup.string()
    .length(6, "OTP must be exactly 6 digits")
    .matches(/^[0-9]+$/, "OTP must be a number")
    .required("OTP is required");

// ================================
// ✅ Schemas
// ================================

// Login
export const loginSchema = Yup.object({
    email: emailRule,
    passwordHash: passwordRule,
});

// Signup
export const signupSchema = Yup.object({
    email: emailRule,
    phoneNumber: phoneNumberRule,
    companyName: companyNameRule,
    passwordHash: passwordRule,
    confirmPassword: confirmPasswordRule("passwordHash"),
});

// Reset Password
export const resetPasswordSchema = Yup.object({
    email: emailRule,
    NewPassword: passwordRule,
    confirmPassword: confirmPasswordRule("NewPassword"),
    Otp: otpRule,
});

// Forgot Password
export const forgetPasswordSchema = Yup.object({
    email: emailRule,
});

// OTP Verification
export const otpVerificationSchema = Yup.object({
    Otp: otpRule,
});

// Export rules separately if needed
export const rules = {
    emailRule,
    passwordRule,
    companyNameRule,
    phoneNumberRule,
    otpRule,
};

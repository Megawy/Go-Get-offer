import { createSlice } from "@reduxjs/toolkit";

// initialState
const initialState = {
    user: null,
    token: null,
    isAuthenticated: false,
    companyName: null,
    emailAddress: null,
    phoneNumber: null,
    role: null,
};

const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        setCredentials: (state, action) => {
            const { token, user, companyName, emailAddress, phoneNumber, role } = action.payload;

            state.token = token ?? state.token;
            state.user = user ?? state.user;
            state.isAuthenticated = !!(token || state.token);
            state.companyName = companyName ?? state.companyName;
            state.emailAddress = emailAddress ?? state.emailAddress;
            state.phoneNumber = phoneNumber ?? state.phoneNumber;
            state.role = role ?? state.role;
        },
        logout: (state) => {
            state.user = null;
            state.token = null;
            state.isAuthenticated = false;
            state.companyName = null;
            state.emailAddress = null;
            state.phoneNumber = null;
            state.role = null;
        },
    },
});

export const { setCredentials, logout } = authSlice.actions;
export const authReducer = authSlice.reducer;

// Selectors
export const selectCurrentUser = (state) => state.auth.user;
export const selectCurrentToken = (state) => state.auth.token;
export const selectIsAuthenticated = (state) => state.auth.isAuthenticated;
export const selectCurrentCompanyName = (state) => state.auth.companyName;
export const selectCurrentEmailAddress = (state) => state.auth.emailAddress;
export const selectCurrentPhoneNumber = (state) => state.auth.phoneNumber;
export const selectCurrentRole = (state) => state.auth.role;

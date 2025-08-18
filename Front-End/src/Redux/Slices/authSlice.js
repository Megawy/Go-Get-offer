import { createSlice } from "@reduxjs/toolkit";

// initialState
const initialState = {
    user: null,            
    token: null,           // Access Token
    isAuthenticated: false // Simple flag
};

const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        // Called after login or refresh
        setCredentials: (state, action) => {
            const { user, token } = action.payload;
            state.user = user || state.user;   // If a new user is provided, store it; otherwise keep the old one
            state.token = token;
            state.isAuthenticated = Boolean(token);   // true if token exists
        },

        // Called after logout or refresh failure
        logout: (state) => {
            state.user = null;
            state.token = null;
            state.isAuthenticated = false;
        },
    },
});

//  Actions
export const { setCredentials, logout } = authSlice.actions;

//  Reducer
export  let authReducer = authSlice.reducer;

//  Selectors 
export const selectCurrentUser = (state) => state.auth.user;
export const selectCurrentToken = (state) => state.auth.token;
export const selectIsAuthenticated = (state) => state.auth.isAuthenticated;

import { configureStore } from "@reduxjs/toolkit";
import { counterReducer } from "./Slices/counterSlice.js";
import { authReducer } from "./Slices/authSlice.js";
import { modalReducer } from "./Slices/modalSlice.js"; // ✅ لازم braces

const store = configureStore({
    reducer: {
        counter: counterReducer,
        auth: authReducer,
        modal: modalReducer,
    }
})

export default store;

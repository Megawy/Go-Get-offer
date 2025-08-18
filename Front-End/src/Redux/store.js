import { configureStore } from "@reduxjs/toolkit";
import { counterReducer } from "./Slices/counterSlice.js";
import {authReducer} from "./Slices/authSlice.js"; // Import the auth reducer
const store = configureStore({
    reducer: {
        counter: counterReducer,
        auth: authReducer
    }

})

export default store;
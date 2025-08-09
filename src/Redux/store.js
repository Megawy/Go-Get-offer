import { configureStore } from "@reduxjs/toolkit";
import { counterReducer } from "./Slices/counterSlice.js";

const store = configureStore({
    reducer: {
        counter: counterReducer
    }

})

export default store;
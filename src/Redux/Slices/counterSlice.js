import { createSlice } from "@reduxjs/toolkit";

const initialState = { counter: 0 }

let counterSlice = createSlice({
    name: "counterSlice",
    initialState,
    reducers: {
        increment: (state, action) => {
            state.counter += 1;
        },
        decrement: (state, action) => {
            state.counter -= 1;
        }
    }
})

export let counterReducer = counterSlice.reducer;
export let { increment, decrement } = counterSlice.actions;
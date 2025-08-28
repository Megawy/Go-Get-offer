import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    isOpen: false,
    type: "",
    title: "",
    message: "",
    image: null,
    actionType: null,
    confirmAction: null,
    cancelAction: null,
};

const modalSlice = createSlice({
    name: "modal",
    initialState,
    reducers: {
        openModal: (state, action) => {
            state.isOpen = true;
            state.type = action.payload.type || "";
            state.title = action.payload.title || "";
            state.message = action.payload.message || "";
            state.image = action.payload.image || null;
            state.actionType = action.payload.actionType || null;
            state.confirmAction = action.payload.confirmAction || null;
            state.cancelAction = action.payload.cancelAction || null;
        },
        closeModal: () => initialState, // reset everything
    },
});

export const { openModal, closeModal } = modalSlice.actions;
export const modalReducer = modalSlice.reducer;

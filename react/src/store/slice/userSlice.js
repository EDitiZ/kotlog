import { createSlice } from "@reduxjs/toolkit";

const userSlice = createSlice({
    name: "user",
    initialState: {
        user: null,
        isLoggedIn: false,
    },
    reducers: {
        login(state, action){
            state.isLoggedIn = true;
            state.user = action.payload;
        },
        logout(state, action){
            state.isLoggedIn = false;
            state.user = null;
        },
    },
});

export const {login, logout} = userSlice.actions;
export const userReducer = userSlice.reducer;
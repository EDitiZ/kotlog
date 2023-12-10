import { configureStore } from "@reduxjs/toolkit";
import { login, logout, userReducer } from "./slice/userSlice";

const store = configureStore({
    reducer: {
        user: userReducer,
    },
});

export {store, login, logout};
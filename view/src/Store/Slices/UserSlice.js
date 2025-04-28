import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axios from 'axios';
import { use } from 'react';

const initialState = {
    user: null,
    token: null,
    error: null,
    loading: false,
    isLoggedIn: false
};

const API_URL = process.env.REACT_APP_API_URL;

export const fetchUser = createAsyncThunk("user/fetchUser", async (token) => {
    const response = await axios.get(`${API_URL}/users/`);
    const userData = {...response.data};
    userData.token = token;
    return response.data;
});

const userSlice = createSlice({
    name: 'userSlice',
    initialState,
    reducers: {
        setUser(state, action) {
            state.user = action.payload.user;
            state.token = action.payload.token;
            state.error = null;
            state.loading = false;
            state.isLoggedIn = true;
        },
        clearUser(state) {
            state.user = null;
            state.token = null;
            state.error = null;
            state.loading = false;
            state.isLoggedIn = false;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchUser.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchUser.fulfilled, (state, action) => {
                state.user = action.payload.user;
                state.token = action.payload.token;
                state.loading = false;
                state.error = null;
                state.isLoggedIn = true;
            })
            .addCase(fetchUser.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message;
            });
    },
});

export const { setUser, clearUser } = userSlice.actions;

export default userSlice.reducer;
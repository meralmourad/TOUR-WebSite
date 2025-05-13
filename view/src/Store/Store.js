import { configureStore } from '@reduxjs/toolkit';
import UserSlice from './Slices/UserSlice';
import ChatSlice from './Slices/ChatSlice';

const store = configureStore({
    reducer: {
        info: UserSlice,
        chat: ChatSlice
    },
});

export default store;
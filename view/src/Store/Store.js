import { configureStore } from '@reduxjs/toolkit';
import UserSlice from './Slices/UserSlice';

const store = configureStore({
    reducer: {
        info: UserSlice
    },
});

export default store;
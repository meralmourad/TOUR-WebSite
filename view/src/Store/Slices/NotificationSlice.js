import { createSlice } from '@reduxjs/toolkit';

const initialState = {
    showChat: false,
    senderId: null,
    receiverId: null
};

const chatSlice = createSlice({
    name: 'chatSlice',
    initialState,
    reducers: {
        setChat(state, action) {
            state.senderId = action.payload.senderId;
            state.receiverId = action.payload.receiverId;
            state.showChat = true;
        },
        clearChat(state) {
            state.senderId = null;
            state.receiverId = null;
            state.showChat = false;
        }
    },
});

export const { setChat, clearChat } = chatSlice.actions;

export default chatSlice.reducer;
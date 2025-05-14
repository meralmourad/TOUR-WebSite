import axios from 'axios';

const API_URL = process.env.REACT_APP_API_URL;
const token = JSON.parse(localStorage.getItem("Token"))?.token;

export const getMessages = async (senderId, receiverId) => {
    try {
        const response = await axios.get(`${API_URL}/Message/conversation?senderId=${senderId}&receiverId=${receiverId}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data.$values;
    } catch (error) {
        throw error;
    }
}

export const sendMessage = async (senderId, receiverId, content) => {
    try {
        const response = await axios.post(`${API_URL}/Message`, {
            id: 0,
            senderId,
            receiverId,
            content
        }, {
            headers: {
                Authorization: `Bearer ${token}`
            },
        });
        return response.data;
    } catch (error) {
        throw error;
    }
}

import axios from 'axios';

const API_URL = process.env.REACT_APP_API_URL;
const token = JSON.parse(localStorage.getItem("Token"))?.token;

export const getUsers = async () => {
    try {
        const response = await axios.get(`${API_URL}/User`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const getUserById = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/User/${id}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const deleteUser = async (id) => {
    try {
        const response = await axios.delete(`${API_URL}/User/${id}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const updateUser = async (id, user) => {
    try {
        const response = await axios.put(`${API_URL}/User/${id}`, user, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        
        return response.data;
    } catch (error) {
        throw error;
    }
};

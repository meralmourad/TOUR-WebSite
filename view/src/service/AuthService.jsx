import axios from 'axios';

const API_URL = process.env.REACT_APP_API_URL;

export const login = async (Data) => {
    try {
        const response = await axios.post(`${API_URL}/Auth/login`, {
            email: Data.email,
            password: Data.password
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const register = async (Data) => {
    try {
        const response = await axios.post(`${API_URL}/Auth/signup`, {
            fullName: Data.name,
            email: Data.email,
            password: Data.password,
            confirmPassword: Data.confirmPassword,
            phoneNumber: Data.phone,
            role: Data.role,
            address: Data.country,
        });
        return response.data;
    }
    catch (error) {
        throw error;
    }
};

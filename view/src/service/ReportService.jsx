import axios from 'axios';
import { getUserById } from './UserService';

const API_URL = process.env.REACT_APP_API_URL;
const token = JSON.parse(localStorage.getItem("Token"))?.token;

export const getReportsByTripId = async (tripId) => {
    try {
        const response = await axios.get(`${API_URL}/Report/trip/${tripId}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            }
        });
        const data = response.data.$values;
        for (let i = 0; i < data.length; i++) {
            data[i].sender = await getUserById(data[i].senderId);
        }
        return data;
    } catch (error) {
        throw error;
    }
}

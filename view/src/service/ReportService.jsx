import axios from 'axios';

const API_URL = process.env.REACT_APP_API_URL;
const token = JSON.parse(localStorage.getItem("Token"))?.token;

export const getReportsByTripId = async (tripId) => {
    try {
        const response = await axios.get(`${API_URL}/Report/Trip/${tripId}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            }
        });
        return response.data.$values;
    } catch (error) {
        throw error;
    }
}

import axios from 'axios';

const API_URL = process.env.REACT_APP_API_URL;
const token = JSON.parse(localStorage.getItem("Token"))?.token;

export const getBookings = async () => {
    try {
        const response = await axios.get(`${API_URL}/Booking`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        const bookings = response.data.$values;
        return bookings;
    } catch (error) {
        throw error;
    }
};

export const addBooking = async (booking) => {
    try {
        const response = await axios.post(`${API_URL}/Booking`, booking, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return response.data;
    }
    catch (error) {
        throw error;
    }
};

export const getBookingById = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/Booking/${id}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        const booking = response.data;
        return booking;
    }
    catch (error) {
        throw error;
    }
};

export const deleteBooking = async (id) => {
    try {
        const response = await axios.delete(`${API_URL}/Booking/${id}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return response.data;
    }
    catch (error) {
        throw error;
    }
};

export const updateBooking = async (id, booking) => {
    try {
        const response = await axios.put(`${API_URL}/Booking/${id}`, booking, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return response.data;
    }
    catch (error) {
        throw error;
    }
};

export const getBookingByTripId = async (tripId) => {
    try {
        const response = await axios.get(`${API_URL}/Booking/trip/${tripId}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        const booking = response.data;
        return booking;
    }
    catch (error) {
        throw error;
    }
};

export const approveBooking = async (id, state = 1) => {
    /*
        state: -1 = rejected
        state: 0 = pending
        state: 1 = approved
        
    */
    try {
        const response = await axios.put(`${API_URL}/Booking/approve/${id}`, state, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return response.data;
    }
    catch (error) {
        throw error;
    }
};

export const rateTrip = async (bookingId, rating) => {
    try {
        const response = await axios.put(`${API_URL}/Booking/rate/${bookingId}`, rating, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return response.data;
    }
    catch (error) {
        throw error;
    }
};
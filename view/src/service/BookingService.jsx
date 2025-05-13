import axios from 'axios';
import { getTripById } from './TripsService';
import { getUserById } from './UserService';

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
        for(let i = 0; i < bookings.length; i++) {
            bookings[i].trip = await getTripById(bookings[i].tripId);
            bookings[i].tourist = await getUserById(bookings[i].touristId);
        }
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

// export const getBookingsByTripId = async (tripId) => {
//     try {
//         const response = await axios.get(`${API_URL}/Booking/trip/${tripId}`, {
//             headers: {
//                 'Authorization': `Bearer ${token}`
//             }
//         });
//         const bookings = response.data.$values;
//         for(let i = 0; i < bookings.length; i++) {
//             bookings[i].trip = await getTripById(bookings[i].tripId);
//             bookings[i].tourist = await getUserById(bookings[i].touristId);
//         }
//         return bookings;
//     }
//     catch (error) {
//         throw error;
//     }
// };

export const approveBooking = async (id, state = 1) => {
    /*
        state: -1 = rejected
        state: 0 = pending
        state: 1 = approved
    */
    try {
        const response = await fetch(`${API_URL}/Booking/approve/${id}`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(state),
        });
        return await response.json();
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

export const searchBookings = async (start, len, tripId, USERID, IsApproved = 2) => { // 1 = approved, 0 = rejected, 2 = all
    start = start ?? 0;
    len = len ?? 3;
    tripId = tripId ?? 0;
    USERID = USERID ?? 0;
    try {
        let bookings = [];
        let totalCount = 0;

        if(IsApproved !== 0) {
            const response = await axios.get(`${API_URL}/Search/bookings?start=${start}&len=${len}&IsApproved=${true}&USERID=${USERID}`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            // console.log(`${API_URL}/Search/bookings?start=${start}&len=${len}&IsApproved=${true}&USERID=${USERID}`);
            bookings = response.data.bookings.$values;
            totalCount = response.data.totalCount;
        }
        if(IsApproved !== 1) {
            const response = await axios.get(`${API_URL}/Search/bookings?start=${start}&len=${len}&IsApproved=${false}&USERID=${USERID}`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            bookings = [...bookings, ...response.data.bookings.$values];
            totalCount += response.data.totalCount;
        }
        // console.log(IsApproved);
        // console.log(bookings);
        for(let i = 0; i < bookings.length; i++) {
            bookings[i].trip = await getTripById(bookings[i].tripId);
            bookings[i].tourist = await getUserById(bookings[i].touristId);
        }
        return { bookings, totalCount };
    }
    catch (error) {
        throw error;
    }
};
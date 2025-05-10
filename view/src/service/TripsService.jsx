import axios from 'axios';
import { getUserById } from './UserService';

const API_URL = process.env.REACT_APP_API_URL;
const token = JSON.parse(localStorage.getItem("Token"))?.token;

export const addTrip = async (trip) => {
    try {
        const response = await axios.post(`${API_URL}/Trip`, trip, {
        headers: {
            'Authorization': `Bearer ${token}`
        }
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const getTrips = async () => {
    try {
        const response = await axios.get(`${API_URL}/Trip`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        const trips = response.data.$values;

        for (let i = 0; i < trips.length; i++) {
            const agence = await getUserById(trips[i].agenceId);
            trips[i].agence = agence;
        }

        return trips;
    } catch (error) {
        throw error;
    }
};

export const getTripById = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/Trip/${id}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        const trip = response.data;
        const agence = await getUserById(trip.agenceId);
        trip.agence = agence;
        return trip;
    }
    catch (error) {
        throw error;
    }
};

export const updateTrip = async (trip) => {
    try {
        const response = await axios.put(`${API_URL}/Trip/${trip.id}`, trip, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const deleteTrip = async (id) => {
    try {
        const response = await axios.delete(`${API_URL}/Trip/${id}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const getTripsByAgenceId = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/Trip/agence/${id}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        const trips = response.data.$values;

        const agence = await getUserById(id);
        for (let i = 0; i < trips.length; i++) {
            trips[i].agence = agence;
        }

        return trips;
    } catch (error) {
        throw error;
    }
};

export const ApproveTrip = async (id) => {
    try {
        const response = await axios.put(`${API_URL}/Trip/approve/${id}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        return response.data;
    } catch (error) {
        throw error;
    }
}

export const SearchTrips = async (start, len, destination, startDate, endDate, price, isApproved, searchTerm, agencyId = 0, sortBy = true) => {
    start = start ?? 0;
    len = len ?? 2147483647;
    destination = destination ?? "";
    startDate = startDate ?? "";
    endDate = endDate ?? "";
    price = price ?? 2147483647;
    isApproved = isApproved ?? true;
    searchTerm = searchTerm ?? "";
    agencyId = agencyId ?? 0;
    try {
        const response = await axios.get(`${API_URL}/Search/trips?start=${start}&len=${len}&destination=${destination}&startDate=${startDate}&endDate=${endDate}&price=${price}&isApproved=${isApproved}&q=${searchTerm}&agencyId=${agencyId}&sortByRating${sortBy}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        const trips = response.data.trips.$values;
        const totalCount = response.data.totalCount;
        for (let i = 0; i < trips.length; i++) {
            const agence = await getUserById(trips[i].agenceId);
            trips[i].agence = agence;
        }
        return trips;
    } catch (error) {
        throw error;
    }
}
import axios from 'axios';

const API_URL = process.env.REACT_APP_API_URL;
const token = JSON.parse(localStorage.getItem("Token"))?.token;

export const getNotifications = async (id, start, len) => {
  try {
    const response = await axios.get(`${API_URL}/Notifications/receiver/${id}?start=${start}&len=${len}`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });

    return {allNotifications: response.data.$values };
  } catch (error) {
    throw error;
  }
}


/*

{
  "id": 0,
  "senderId": 0,
  "receiverId": 0,
  "context": "string"
}

*/
export const addNotification = async (notification) => {
  try {
    const response = await axios.post(`${API_URL}/Notifications?receiverIds=${notification.receiverIds}`, notification, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });

    return response.data;
  } catch (error) {
    throw error;
  }
}

import "./BookingPending.scss";
import React, { useState , use } from "react";
const BookingPending = () => {
    const [bookings, setBookings] = useState([
        { id: 1, name: "Booking 1" },
        { id: 2, name: "Booking 2" },
        { id: 3, name: "Booking 3" },
    ]);

    return (
        <div className="booking-pending-container">
            <div className="search-bar">
                <input type="text" placeholder="Search..." />
                <i className="fa fa-search"></i>
            </div>
            <h2>Booking Pending</h2>
            <div className="booking-list">
                {bookings.map((booking) => (
                    <div key={booking.id} className="booking-item">
                        <span className="booking-name">{booking.name}</span>
                        <button className="reject-btn">reject</button>
                        <button className="accept-btn">accept</button>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default BookingPending;
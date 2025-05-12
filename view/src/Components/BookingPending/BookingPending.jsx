import "./BookingPending.scss";
import React, { useState } from "react";

const BookingPending = () => {
    const [searchTerm, setSearchTerm] = useState("");
    const [bookings, setBookings] = useState([]);
    
    const handleSearchChange = (event) => {
        setSearchTerm(event.target.value);
    }

    const fetchBookings = async () => {
        try {
            const response = await fetch(`http://localhost:5000/api/bookings?search=${searchTerm}`);
            const data = await response.json();
            setBookings(data);
        } catch (error) {
            console.error("Error fetching bookings:", error);
        }
    }

    return (
        <div className="booking-pending-container travel-cards-container">
        <h2>Agencies Pending</h2>
        <div className="search-filter">
            <span role="img" aria-label="search" style={{ marginRight: "10px" }}>
                <img
                src="/Icons/search.jpg"
                alt=""
                style={{
                    width: "20px",
                    height: "20px",
                }}
                ></img>
            </span>
            <input
                style={{ backgroundColor: "white" }}
                type="text"
                value={searchTerm}
                onChange={handleSearchChange}
            />
        </div>
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
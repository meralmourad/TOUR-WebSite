
import "./BookingPending.scss";

const BookingPending = () => {
    const bookings = [
        { id: 1, name: "omar" },
        { id: 2, name: "omar" },
        { id: 3, name: "omar" },
        { id: 4, name: "omar" },
        { id: 5, name: "omar" },
        { id: 6, name: "omar" },
    ];

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
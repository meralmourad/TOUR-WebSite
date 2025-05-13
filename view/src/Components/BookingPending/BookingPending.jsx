import { useSelector } from "react-redux";
import { approveBooking, searchBookings } from "../../service/BookingService";
import "./BookingPending.scss";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import Swal from "sweetalert2";
import { getTripById } from "../../service/TripsService";

const BookingPending = () => {
    const { user } = useSelector((store) => store.info);
    const navigate = useNavigate();
    const id = useParams()?.id;
    const [searchTerm, setSearchTerm] = useState("");
    const [bookings, setBookings] = useState([]);
    const [render, setRender] = useState(false);

    const handleSearchChange = (event) => {
        setSearchTerm(event.target.value);
    }

    useEffect(() => {
        const fetch = async () => {
            try {
                const trip = await getTripById(id);
                // console.log(user);

                if(!user || !trip) return;

                if(user.role !== 'Admin' && trip.agenceId !== user.id) {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: "You are not allowed to access this page!"
                    });
                    navigate("/home");
                    return;
                }
                // console.log(trip);

                const { bookings } = await searchBookings(null, 1000, id, user.id, 0);
                const finalBookings = bookings.filter((booking) => {
                    return booking.isApproved === 0 && booking.tourist.name.toLowerCase().includes(searchTerm.toLowerCase());
                });
                // console.log(finalBookings);
                setBookings(finalBookings);
            }
            catch (error) {
                console.error("Error fetching bookings:", error);
            }
        }
        fetch();
    }, [user, searchTerm, id, render]);


    const accept = async (bookingId) => {
        Swal.fire({
            title: "Are you sure?",
            text: "You are about to accept this booking.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, accept it!"
        }).then(async (result) => {
            if (result.isConfirmed) {
            try {
                await approveBooking(bookingId, 1);
                setRender(!render);
                Swal.fire("Accepted!", "The booking has been accepted.", "success");
            } catch (error) {
                console.error("Error accepting booking:", error);
                Swal.fire("Error!", "There was an error accepting the booking.", "error");
            }
            }
        });
    }

    const reject = async (bookingId) => {
        Swal.fire({
            title: "Are you sure?",
            text: "You are about to reject this booking.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#d33",
            cancelButtonColor: "#3085d6",
            confirmButtonText: "Yes, reject it!"
        }).then(async (result) => {
            if (result.isConfirmed) {
            try {
                await approveBooking(bookingId, -1);
                setRender(!render);
                Swal.fire("Rejected!", "The booking has been rejected.", "success");
            } catch (error) {
                console.error("Error rejecting booking:", error);
                Swal.fire("Error!", "There was an error rejecting the booking.", "error");
            }
            }
        });
    }

    return (
        <div className="booking-pending-container travel-cards-container">
        <h2>Booking Pending</h2>
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
                placeholder="Search by name"
            />
        </div>
            <div className="booking-list">
                {bookings.length === 0 && <div className="no-booking">No Booking Found</div>}
                {bookings.map((booking) => (
                    <div key={booking.id} className="booking-item" >
                        <div onClick={() => navigate(`/profile/${booking.tourist.id}`)}>
                            <span className="booking-name">{booking.tourist.name}   |   </span> 
                            <span className="booking-small">{booking.seatsNumber}</span>
                        </div>
                        <button className="reject-btn" onClick={() => reject(booking.id)}>reject</button>
                        <button className="accept-btn" onClick={() => accept(booking.id)}>accept</button>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default BookingPending;
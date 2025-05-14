import { useState, useEffect } from "react";
import "./Trip.scss";
import { useNavigate, useParams } from "react-router-dom";
import { getTripById } from "../../../service/TripsService";
import { useSelector } from "react-redux";
import Swal2 from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { addBooking } from "../../../service/BookingService";

const MySwal = withReactContent(Swal2);

const Trip = () => {
  const { user } = useSelector((store) => store.info);
  const id = useParams()?.id;
  const navigate = useNavigate();
  const [tripData, setTripData] = useState(null);
  const [imageIndex, setImageIndex] = useState(0); // ⬅️ لحفظ رقم الصورة الحالية

  useEffect(() => {
    const fetchTripData = async () => {
      try {
        const trip = await getTripById(id);
        setTripData(trip);
      } catch (error) {
        console.error("Error fetching trip data:", error);
      }
    };

    fetchTripData();
  }, [user, id]);

  const handleBooking = async () => {
    const result = await MySwal.fire({
      title: "Booking Details",
      html: `
          <div class="custom-form">
            <div class="row">
              <span>Name :</span><span>${user.name}</span>
              <span>Email :</span><span>${user.email}</span>
            </div>
            <div class="row">
              <span>Phone :</span><input id="swal-input-phone" class="swal2-input" value="${user.phoneNumber}" />
            </div>
            <div class="row">
              <span>Payment :</span><span>Cash Only!</span>
              <span>Seats :</span><input id="swal-input-seats" min=1 class="swal2-input" type="number" value="1"/>
            </div>
          </div>
        `,
      showCancelButton: true,
      confirmButtonText: "Confirm",
      cancelButtonText: "Discard",
      focusConfirm: false,
      preConfirm: () => {
        const phone = document.getElementById("swal-input-phone").value;
        const seats = document.getElementById("swal-input-seats").value;

        if (!phone || !seats) {
          Swal2.showValidationMessage("Please fill out all fields.");
          return;
        }

        return { phone, seats };
      },
    });

    if (result.isConfirmed) {
      const { phone, seats } = result.value;

      try {
        await addBooking({
          touristId: user.id,
          tripId: tripData.id,
          seatsNumber: seats,
          phoneNumber: phone,
        });
        MySwal.fire({
          title: "Booking Confirmed!",
          text: `Your booking for ${seats} seat(s) has been confirmed.`,
          icon: "success",
        });
      } catch (error) {
        console.error("Error adding booking:", error);
        MySwal.fire({
          title: "Booking Failed!",
          text: error.response.data.message,
          icon: "error",
        });
        return;
      }
    } else {
      MySwal.fire({
        title: "Booking Discarded!",
        text: "Booking Discarded!",
        icon: "error",
      });
    }
  };

  const handlePrev = () => {
    if (!tripData) return;
    setImageIndex((prev) =>
      prev === 0 ? tripData.images.$values.length - 1 : prev - 1
    );
  };

  const handleNext = () => {
    if (!tripData) return;
    setImageIndex((prev) =>
      prev === tripData.images.$values.length - 1 ? 0 : prev + 1
    );
  };

  return (
    <>
      {tripData && (
        <div className="trip-container">
          <div className="trip-header">
            <img
              src={
                tripData.images.$values[imageIndex] ||
                "https://media-public.canva.com/MADQtrAClGY/2/screen.jpg"
              }
              alt="Trip"
              className="trip-image"
            />
            {tripData.images.$values.length > 1 && (
              <>
                <button className="arrow-button left" onClick={handlePrev}>
                  &#8592;
                </button>
                <button className="arrow-button right" onClick={handleNext}>
                  &#8594;
                </button>
              </>
            )}
          </div>

          <div className="trip-details">
            <div className="trip-section">
              <h3>Available Sets:</h3>
              <p className="highlight">{tripData.availableSets}</p>
              <div onClick={() => navigate("/profile/" + tripData.agence.id)}>
                <h3>Agency Name:</h3>
                <p>{tripData.agence.name}</p>
              </div>
            </div>

            <div className="trip-section">
              <h3>Destinations</h3>
              <ul>
                {tripData.locations.$values.map((loc) => (
                  <li key={loc}>{loc}</li>
                ))}
              </ul>
              <p>
                <strong>From:</strong> {tripData.startDate}
              </p>
              <p>
                <strong>To:</strong> {tripData.endDate}
              </p>
            </div>

            <div className="trip-section">
              <h3>Price:</h3>
              <p className="highlight">{tripData.price} Per Person</p>
              <h3>Description</h3>
              <p>{tripData.description}</p>
            </div>
          </div>

          {tripData.status === 1 &&
            (user.role === "Admin" || user.id === tripData.agenceId) && (
              <button
                className="pending-bookings-button"
                onClick={() => navigate(`/BookingPending/${tripData.id}`)}
              >
                pending bookings
                <span className="arrow">→</span>
              </button>
            )}
          {user.role === "Tourist" && (
            <button className="book-now-button" onClick={handleBooking}>
              BOOK NOW!
              <span className="arrow">→</span>
            </button>
          )}
        </div>
      )}
    </>
  );
};

export default Trip;

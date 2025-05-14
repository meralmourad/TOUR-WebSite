import { useState, useEffect } from "react";
import "./Trip.scss";
import { useNavigate, useParams } from "react-router-dom";
import { getTripById } from "../../../service/TripsService";
import { useSelector } from "react-redux";
import Swal2 from "sweetalert2";
// import Swal from "sweetalert";
import withReactContent from "sweetalert2-react-content";
import { addBooking } from "../../../service/BookingService";

// const images = [
//   "https://ultimahoracol.com/sites/default/files/2024-12/PORTADAS%20ESCRITORIO%20-%202024-12-19T122111.264.jpg",
//   "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTCVuJDmSjVmO1RzJaVuLlix7evJoVWOhL4ghYK0mlJad4o_w2nu8H3UOUF&s=10",
//   "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ45rDg-QQbP8l4fp0IT1B1zDLU8BdxV_LIFToRuNG9KEPsc52B4B9rlcX4&s=10",
// ];

const MySwal = withReactContent(Swal2);

const Trip = () => {
  const { user } = useSelector((store) => store.info);
  const id = useParams()?.id;
  const navigate = useNavigate();
  const [tripData, setTripData] = useState(null);

  useEffect(() => {
    const fetchTripData = async () => {
      try {
        const trip = await getTripById(id);
        // console.log(trip);
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

  return (
    <>
      {tripData && (
        <div className="trip-container">
          <div className="trip-header">
            <img
              src={
                tripData.images.$values[0]
                  ? tripData.images.$values[0]
                  : "https://media-public.canva.com/MADQtrAClGY/2/screen.jpg"
              }
              alt="Trip"
              className="trip-image"
            />
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
          {/* {console.log(tripData)} */}
          {tripData.status === 1 &&
            (user.role === "Admin" || user.id === tripData.agenceId) && (
              <div className="trip-buttons">
                <button
                  className="edit_butt"
                    onClick={() => navigate(`/EditTrip/${tripData.id}`)}
                >
                    Edit Trip
                    <span className="arrow">→</span>
                  </button>
                <button
                  className="pending-bookings-button"
                  onClick={() => navigate(`/BookingPending/${tripData.id}`)}
                >
                  pending bookings
                  <span className="arrow">→</span>
                </button>
              </div>
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

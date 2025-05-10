import { useState , useEffect } from "react";
import "./Trip.scss";
import swal from "sweetalert";
import { useParams } from "react-router-dom";
import { getTripById } from "../../../service/TripsService";
import { useSelector } from "react-redux";

// const images = [
//   "https://ultimahoracol.com/sites/default/files/2024-12/PORTADAS%20ESCRITORIO%20-%202024-12-19T122111.264.jpg",
//   "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTCVuJDmSjVmO1RzJaVuLlix7evJoVWOhL4ghYK0mlJad4o_w2nu8H3UOUF&s=10",
//   "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ45rDg-QQbP8l4fp0IT1B1zDLU8BdxV_LIFToRuNG9KEPsc52B4B9rlcX4&s=10",
// ];

const Trip = () => {
  const { user } = useSelector((store) => store.info);
  const id = useParams()?.id;
  const [tripData, setTripData] = useState(null);

  useEffect(() => {
    const fetchTripData = async () => {
      try {
        const trip = await getTripById(id);
        console.log(trip);
        setTripData(trip);
      } catch (error) {
        console.error("Error fetching trip data:", error);
      }
    };

    fetchTripData();
  },[]);

  const handleBooking = () => {
    const wrapper = document.createElement("div");
    wrapper.innerHTML = `
      <div class="custom-form">
        <div class="row">
          <span>Name :</span><span>${ user.name }</span>
          <span>Email :</span><span>${ user.email }</span>
        </div>
        <div class="row">
          <span>Phone :</span><span>${ user.phoneNumber }</span>
        </div>
        <div class="row">
          <span>Payment :</span><span>Cash Only!</span>
          <span>Sets :</span><span>1 <span style="color: green;"></span></span>
        </div>
        <div class="button-row">
          <button class="discard-button">Discard</button>
          <button class="confirm-button">Confirm</button>
        </div>
      </div>
    `;

    swal({
      content: wrapper,
      buttons: false,
    });

    setTimeout(() => {
      document.querySelector(".discard-button")?.addEventListener("click", () => {
        swal("Cancelled", "Booking discarded.", "error");
      });
      document.querySelector(".confirm-button")?.addEventListener("click", () => {
        swal("Confirmed", "Your booking has been confirmed!", "success");
      });
    }, 100);
  };

  return (
    <>
      {tripData && 
        <div className="trip-container">
          <div className="trip-header">
            <img src={tripData.images.$values[0]? tripData.images.$values[0]: 'https://media-public.canva.com/MADQtrAClGY/2/screen.jpg'} alt="Trip" className="trip-image" />
          </div>

          <div className="trip-details">
            <div className="trip-section">
              <h3>Available Sets:</h3>
              <p className="highlight">{tripData.availableSets}</p>
              <h3>Agency Name:</h3>
              <p>{tripData.agence.name}</p>
            </div>

            <div className="trip-section">
              <h3>Destinations</h3>
              <ul>
                  {tripData.locations.$values.map((loc) => (
                    <li key={loc}>{loc}</li>
                  ))}
              </ul>
              <p><strong>From:</strong> { tripData.startDate }</p>
              <p><strong>To:</strong> { tripData.endDate }</p>
            </div>

            <div className="trip-section">
              <h3>Price:</h3>
              <p className="highlight">{tripData.price} Per Person</p>
              <h3>Description</h3>
              <p>{tripData.description}</p>
            </div>
          </div>

          <button className="book-now-button" onClick={handleBooking}>
            BOOK NOW!
            <span className="arrow">→</span>
          </button>
        </div>
    }
    </>
  );
};

export default Trip;

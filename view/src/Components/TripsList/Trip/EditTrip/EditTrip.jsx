import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import Swal2 from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import "./EditTrip.scss";
import {updateTrip,getTripById} from "../../../../service/TripsService.jsx";
import Select from "react-select";

const EditTrip = () => {
  const { id } = useParams();
  const [TripData, setTripData] = useState(null);
  const [confirm, setConfirm] = useState(false);
  const navigate = useNavigate();
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
  }, [id]);

  useEffect(() => {
    if (confirm) {
      const sendUpdatings = async () => {
        try {
          const update = await updateTrip({
            id ,
            price:TripData.price ,
            startDate : TripData.startDate,
            endDate : TripData.endDate ,
            description: TripData.description,
            availableSets : TripData.availableSets
          });
        } 
        catch (error) {
          console.error("Error updating trip data:", error);
        }
      };

      sendUpdatings();
      Swal2.fire({
        title: "Done!",
        text: "Trip updated successfully.",
        icon: "success",
        confirmButtonText: "Ok",
      });
      navigate(`/Trip/${id}`);
    }
  }, [confirm]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setTripData ({ ...TripData , [name]: value });
  };

  const handleDiscard = () => {
    Swal2.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, Discard it!",
    }).then((result) => {
      if (result.isConfirmed) {
        Swal2.fire("Discarded!", "Your file has been discarded.", "success");
      }
    });
  };

  return TripData ? (
    <>
      <div className="header">
        <div className="header-images">
          <img
            src={
              TripData.images.$values[0]
                ? TripData.images.$values[0]
                : "https://media-public.canva.com/MADQtrAClGY/2/screen.jpg"
            }
            alt="Trip"
            className="trip-image"
          />
          <div className="overlay-title">
            <h1>{TripData.title.toUpperCase()}</h1>
          </div>
        </div>
      </div>

      <div className="add-trip-container">
        <div className="form-group">
          <label>Title:</label>
          <input
            type="text"
            name="title"
            defaultValue={TripData.title}
            disabled
          />
        </div>

        <div className="form-group">
          <label>Price:</label>
          <input
            type="number"
            name="price"
            value = {TripData.price}
            onChange={handleInputChange}
          />
        </div>
        <div className="form-group">
          <label>Categories:</label>
          <input
            type="text"
            name="categories"
            disabled
            defaultValue={TripData.categories.$values
              .map((option) => option)
              .join(", ")}
          />
        </div>
        <div className="form-group">
          <label>Sets:</label>
          <input
            type="number"
            name="availableSets"
            value={TripData.availableSets}
            onChange={handleInputChange}
          />
        </div>

        <div className="form-group">
          <label>Destinations:</label>
          <input
            type="text"
            disabled
            defaultValue={TripData.locations.$values
              .map((option) => option)
              .join(", ")}
          />
        </div>

        <div className="form-group">
          <label>Start Date:</label>
          <input
            type="date"
            name="startDate"
            value={TripData.startDate}
            onChange={handleInputChange}
          />
        </div>
        <div className="form-group">
          <label>Description:</label>
          <textarea
            name="description"
            onChange={handleInputChange}
            value={TripData.description}
          />
        </div>

        <div className="form-group">
          <label>End Date:</label>
          <input
            type="date"
            name="endDate"
            value={TripData.endDate}
            onChange={handleInputChange}
          />
        </div>
        <div className="button-group">
          <button className="discard-button" onClick={handleDiscard}>
            Discard
          </button>
          <button className="confirm-button" onClick={() => setConfirm(true)}>
            Confirm
          </button>
        </div>
      </div>
    </>
  ) : null;
};

export default EditTrip;

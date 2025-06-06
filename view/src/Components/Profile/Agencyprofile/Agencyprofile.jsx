import { useEffect, useState } from "react";
import "./Agencyprofile.scss";
import EditAgency from "./EditAgency";
import { SearchTrips } from "../../../service/TripsService";
import { useNavigate } from "react-router-dom";
import Rate from "../../Rate/Rate";
import AddTrip from "./AddTrip/AddTrip";
import axios from 'axios';
const API_URL = process.env.REACT_APP_API_URL;
const token = JSON.parse(localStorage.getItem("Token"))?.token;

const AgencyProfile = ({ userprofile, myProfile }) => {
  const [edit, setEdit] = useState(false);
  const navigate = useNavigate();
  const [highestTrip, setHighestTrip] = useState([]);
  const[showAddTrip, setShowAddTrip] = useState(false);
  const [button, setbutton] = useState(false);
  
  // console.log(highestTrip);
  
  useEffect(() => {
    const fetchHighestRatedTrips = async () => {
        // console.log(userprofile.id);
        const { trips } = await SearchTrips(0, 3, null, null, null, null, true, null, userprofile.id);
        setHighestTrip(trips);
    };

    fetchHighestRatedTrips();
  }, [userprofile]);
  useEffect(() => {
    const downloadFile = async () => {
      try {
        if (button) {
          const response = await axios.get(`${API_URL}/vendorReports/trip-report/${userprofile.id}`, {
            headers: {
              'Authorization': `Bearer ${token}`
            },
            responseType: 'blob' // Ensure the response is treated as a file
          });

          // Create a URL for the file and trigger the download
          const url = window.URL.createObjectURL(new Blob([response.data]));
          const link = document.createElement('a');
          link.href = url;
          link.setAttribute('download', 'trip-report.pdf'); // Set the file name
          document.body.appendChild(link);
          link.click();
          link.remove();
        }
      } catch (error) {
        console.log(error);
      }
    };

    downloadFile();
  }, [button]);

  return ( 
  <>
    <div className="agency-profile">
    {showAddTrip ? <AddTrip setShowAddTrip ={setShowAddTrip} /> : 
    <>
    {myProfile &&
      <button
        onClick={() => setShowAddTrip(!showAddTrip)}
        className="add-trip-button"
      >
        +
      </button>
    }
      <div className="header">
        <div className="header-images">
          <img
            src="https://beta.sis.gov.eg/media/195319/pyramids-2371501-1920.jpg"
            alt=""
          />
          <img
            src="https://kenzly.com/wp-content/uploads/2025/04/Cairo-to-Alexandria-Full-Day-Tour-3.webp"
            alt=""
          />
          <div className="overlay-title">
            <h1>
              {userprofile.name.toUpperCase()}
            </h1>
          </div>
        </div>
        <button onClick={()=>setbutton(true) } className="bouns" >HERE BOUNS </button>
      </div>
      <div className="content">

        <div className="highest-rated">
          <h2>HIGHEST RATED</h2>
          {
            highestTrip.map((trip) => (
                <div key={trip.id} className="rated-item" onClick={() => navigate(`/Trip/${trip.id}`)}>
                  <img
                    src={trip.images.$values[0]? trip.images.$values[0]: 'https://media-public.canva.com/MADQtrAClGY/2/screen.jpg'}
                    alt=""
                    className="rated-img"
                  />
                  <div className="rated-info">
                    <h3>{trip.city}</h3>
                    <p>{trip.description}</p>
                    <Rate children={Math.round(trip.rating)} />
                  </div>
                </div>
            ))
          }
          <button className="see-more" onClick={() => navigate(`/home/${userprofile.id}`)}>SEE MORE →</button>
        </div>

        {!edit && <>
            <div className="agency-info">
            <div className="info-item">
                <label>Name:</label>
                <span>{userprofile.name}</span>
            </div>
            <div className="info-item">
                <label>E-mail:</label>
                <span>{userprofile.email}</span>
            </div>
            <div className="info-item">
                <label>Phone:</label>
                <span>{userprofile.phoneNumber}</span>
            </div>
            <div className="info-item">
                <label>Country:</label>
                <span>{userprofile.address}</span>
            </div>
            {myProfile && <button className="edit-button" onClick={() => setEdit(true)}>
                Edit
            </button> }
            </div>
        </> }
        {edit &&
            <EditAgency user={userprofile} setEdit={setEdit} />
        }
      </div>
      </>}
    </div>
    </>
  );
};

export default AgencyProfile;
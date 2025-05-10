import "./Home.scss";
import Filter from "../Trip/Filter/Filter";
import Rate from "../Rate/Rate";
import axios from "axios";
import swal from 'sweetalert';
import { useSelector }  from "react-redux";
import { useNavigate} from 'react-router-dom'; 
import React, { useState , useEffect } from "react";

const API_URL = process.env.REACT_APP_API_URL;

const TravelCards = () => {

  const navigate = useNavigate();
  const [start , setStart] = useState(0);
  const [tripsData, setTripsData] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [Showfilter, setShowFilter] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const { user, loading , isLoggedIn } = useSelector((store) => store.info);

  const itemsPerPage = 10 ;

  useEffect(() => {
    const fetchTrips = async () => {
      try {
        const response = await axios.get(`${API_URL}/Search/trips?start=${start}&len=${itemsPerPage}`); 
        setTripsData(response.data.$values);
        if(response.data.$values.length === 0 ) {
          setCurrentPage(1) ;
          setStart(0);
          swal("Opps!", "No trips found for this destination", "error");
        }
        else {
          setCurrentPage(currentPage);
        }
        console.log(response.data.$values);
        
      } 
      catch (error) {
        console.error("Error fetching trips:", error);
      }
    };

    fetchTrips();
  }, [start, currentPage]);

  const currentTrips = tripsData;

  const handleSearchChange = (e) => {
    setSearchTerm(e.target.value);
    setCurrentPage(1);
  };

  

  return (
    <>
        <div className="travel-cards-container">
        {!loading && isLoggedIn ? (
          <>
            <h2>
              Hello , {user.name.toUpperCase()} <br /> DISCOVER THE WORLD NOW!
            </h2>
          </>
        ) : (
          <h2>
            Hello<br /> DISCOVER THE WORLD NOW!
          </h2>
        )}

        <div className="search-filter">
          <span role="img" aria-label="search" style={{ marginRight: "10px" }}>
            <img
              src="Icons/search.jpg"
              alt=""
              style={{
                width: "20px",
                height: "20px",
              }}
            ></img>
          </span>
          <input
            type="text"
            placeholder="Search Destination..."
            value={searchTerm}
            onChange={handleSearchChange}
          />
          <span
            role="img"
            aria-label="filter"
            style={{ marginRight: "10px", cursor: "pointer" }}
            onClick={() => setShowFilter(!Showfilter) }
          >
            <img
              src="Icons/Filter.jpg"
              alt=""
              style={{
                width: "20px",
                height: "20px",
              }}
            ></img>
          </span>
        </div>
        {Showfilter && <Filter ShowFilter={Showfilter} />}
        <div className="cards-grid">
          {currentTrips.map((trip) => (
            <div className="trip-card" key={trip.id}>
              <img src="" alt="" onClick={() => navigate(`/Trip/${trip.id}`)} />
              <div
                className="trip-info"
                onClick={() => navigate(`/Trip/${trip.id}`)}
              >
                <h4>{trip.city}</h4>
                <p>{trip.description}</p>
                <div>
                  <Rate children={trip.rating} />
                </div>
              </div>
            </div>
          ))}
        </div>
        <div className="pagination">
          <button
            disabled={currentPage === 1}
            onClick={() => {
              setCurrentPage(currentPage - 1);
              setStart(start - itemsPerPage);
            }}
          >
            ◀
          </button>

          {Array.from({ length: 4 }).map((_, index) => (
            <button
              key={index}
              className={currentPage === index + 1 ? "active" : ""}
              onClick={() => {
                setCurrentPage(index + 1);
                setStart(index * itemsPerPage);
              }}
            >
              {index + 1}
            </button>
          ))}
          <button
            disabled={currentPage === 4}
            onClick={() => {
              setCurrentPage(currentPage + 1);
              setStart(start + itemsPerPage);
            }}
          >
            ▶
          </button>
        </div>
      </div>
    </>
  );
}

export default TravelCards;

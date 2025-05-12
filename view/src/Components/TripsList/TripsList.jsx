import "./TripsList.scss";
import Filter from "./Trip/Filter/Filter";
import Rate from "../Rate/Rate";
import swal from "sweetalert";
import { useSelector } from "react-redux";
import { useNavigate, useParams } from "react-router-dom";
import React, { useState, useEffect } from "react";
import { SearchTrips } from "../../service/TripsService";

const itemsPerPage = 10;

const TravelCards = () => {
  const id = useParams()?.id;
  const navigate = useNavigate();
  const [start, setStart] = useState(0);
  const [tripsData, setTripsData] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [Showfilter, setShowFilter] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [numberOfItems, setNumberOfItems] = useState(0);
  const [startDate, setStartDate] = useState(null);
  const [endDate, setEndDate] = useState(null);
  const [price, setPrice] = useState(null);
  const { user, loading, isLoggedIn } = useSelector((store) => store.info);
  const [isApproved, setIsApproved] = useState(true);
  
  const numberOfPages = Math.ceil(numberOfItems / itemsPerPage);
  
  useEffect(() => {
    const convertTime = (time) => {
      if(!time) return null;
      const date = new Date(time);
      return date.toISOString().split("T")[0];
    }
    const fetchTrips = async () => {
      try {
        const { trips, totalCount } = await SearchTrips(
          start,
          itemsPerPage,
          null /* destination */,
          convertTime(startDate) /* startDate */,
          convertTime(endDate) /* endDate */,
          price /* price */,
          isApproved /* isApproved */,
          searchTerm,
          id,
        );

        setNumberOfItems(totalCount);
        setTripsData(trips);

        if (trips.length === 0) {
          setCurrentPage(1);
          setStart(0);
          swal("Opps!", "No trips found for this destination", "error");
        } else {
          setCurrentPage(currentPage);
        }
        // console.log(trips);
      } catch (error) {
        console.error("Error fetching trips:", error);
      }
    };

    fetchTrips();
//     setTripsData([{id: 1, city: "Paris", description: "Beautiful city", rating: 4}
// , {id: 2, city: "London", description: "Historic city", rating: 5}]);
  }, [start, currentPage, searchTerm, startDate, endDate, price, id , user, isApproved]);

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
            Hello
            <br /> DISCOVER THE WORLD NOW!
          </h2>
        )}

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
            type="text"
            placeholder="Search Destination..."
            value={searchTerm}
            onChange={handleSearchChange}
          />
          <span
            role="img"
            aria-label="filter"
            style={{ marginRight: "10px", cursor: "pointer" }}
            onClick={() => setShowFilter(!Showfilter)}
          >
            <img
              src="/Icons/Filter.jpg"
              alt=""
              style={{
                width: "20px",
                height: "20px",
              }}
            ></img>
          </span>
        </div>

        {Showfilter &&   
          <Filter
            id={id}
            setShowFilter={setShowFilter}
            startDate={startDate}
            endDate={endDate}
            price={price}
            setStartDate={setStartDate}
            setEndDate={setEndDate}
            setPrice={setPrice}
            setIsApproved={setIsApproved}
            isApproved={isApproved}
          />
        }
        <div className="cards-grid">
          {currentTrips.map((trip) => (
            <div className="trip-card" key={trip.id}>
              {/* {console.log(trip)} */}
              <img src={trip.images.$values[0]? trip.images.$values[0]: 'https://media-public.canva.com/MADQtrAClGY/2/screen.jpg'} alt="" onClick={() => navigate(`/Trip/${trip.id}`)} />
              <div
                className="trip-info"
                onClick={() => navigate(`/Trip/${trip.id}`)}
              >
                <h4>{trip.city}</h4>
                <p>{trip.description}</p>
                <div>
                  <Rate children={Math.round(trip.rating)} />
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

          {Array.from({ length: numberOfPages }).map((_, index) => (
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
            disabled={currentPage === numberOfPages}
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
};

export default TravelCards;

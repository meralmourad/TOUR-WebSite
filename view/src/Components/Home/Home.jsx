import React, { useState } from "react";
import Rate from "../Rate/Rate";
import { useSelector }  from "react-redux";
import "./Home.scss";

const tripsData = [
  { id: 1, city: "London", description: "An exciting trip to London in 5 days", rating: 4, image: "london.jpg" },
  { id: 2, city: "Maldives", description: "Relaxing trip to Maldives in 5 days", rating: 5, image: "maldives.jpg" },
  { id: 3, city: "Greece", description: "An exciting trip to Greece in 5 days", rating: 5, image: "greece.jpg" },
  { id: 4, city: "London", description: "Another London trip in 5 days", rating: 4, image: "london.jpg" },
  { id: 5, city: "Maldives", description: "Another Maldives trip", rating: 5, image: "maldives.jpg" },
  { id: 6, city: "Greece", description: "Another Greece trip", rating: 5, image: "greece.jpg" },
  { id: 7, city: "London", description: "An exciting trip to London in 5 days", rating: 4, image: "london.jpg" },
  { id: 8, city: "Maldives", description: "Relaxing trip to Maldives in 5 days", rating: 5, image: "maldives.jpg" },
  { id: 9, city: "Greece", description: "An exciting trip to Greece in 5 days", rating: 5, image: "greece.jpg" },
  { id: 10, city: "London", description: "Another London trip in 5 days", rating: 4, image: "london.jpg" },
  { id: 11, city: "Maldives", description: "Another Maldives trip", rating: 5, image: "maldives.jpg" },
  { id: 12, city: "Greece", description: "Another Greece trip", rating: 5, image: "greece.jpg" },
  { id: 13, city: "London", description: "An exciting trip to London in 5 days", rating: 4, image: "london.jpg" },
  { id: 24, city: "Maldives", description: "Relaxing trip to Maldives in 5 days", rating: 5, image: "maldives.jpg" },
  { id: 34, city: "Greece", description: "An exciting trip to Greece in 5 days", rating: 5, image: "greece.jpg" },
  { id: 44, city: "London", description: "Another London trip in 5 days", rating: 4, image: "london.jpg" },
  { id: 54, city: "Maldives", description: "Another Maldives trip", rating: 5, image: "maldives.jpg" },
  { id: 64, city: "Greece", description: "Another Greece trip", rating: 5, image: "greece.jpg" },
  { id: 15, city: "London", description: "An exciting trip to London in 5 days", rating: 4, image: "london.jpg" },
  { id: 16, city: "Maldives", description: "Relaxing trip to Maldives in 5 days", rating: 5, image: "maldives.jpg" },
  { id: 222, city: "Greece", description: "An exciting trip to Greece in 5 days", rating: 5, image: "greece.jpg" },
  { id: 4, city: "London", description: "Another London trip in 5 days", rating: 4, image: "london.jpg" },
  { id: 5, city: "Maldives", description: "Another Maldives trip", rating: 5, image: "maldives.jpg" },
  { id: 6, city: "Greece", description: "Another Greece trip", rating: 5, image: "greece.jpg" }
];

const TravelCards = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const [searchTerm, setSearchTerm] = useState("");

  const itemsPerPage = 6;
  const { user, loading , isLoggedIn } = useSelector((store) => store.info);

  const filteredTrips = tripsData.filter((trip) =>
    trip.city.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const totalPages = Math.ceil(filteredTrips.length / itemsPerPage);
  const startIndex = (currentPage - 1) * itemsPerPage;
  const currentTrips = filteredTrips.slice(startIndex, startIndex + itemsPerPage);

  const handleSearchChange = (e) => {
    setSearchTerm(e.target.value);
    setCurrentPage(1);
  };

  

  return (
    <div className="travel-cards-container">
      { !loading && isLoggedIn ? <><h2>Hello , {user.name.toUpperCase()} <br /> DISCOVER THE WORLD NOW!</h2> </> : 
      <h2>Hello<br /> DISCOVER THE WORLD NOW!</h2> }

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
          placeholder="Search destination..."
          value={searchTerm}
          onChange={handleSearchChange}
        />
      </div>

      <div className="cards-grid">
        {currentTrips.map((trip) => (
          <div className="trip-card" key={trip.id}>
            <img src={trip.image} alt="" />
            <div className="trip-info">
              <h4>{trip.city}</h4>
              <p>{trip.description}</p>
              <div>
               <Rate/>
              </div>
            </div>
          </div>
        ))}
      </div>

      <div className="pagination">
        <button
          disabled={currentPage === 1}
          onClick={() => setCurrentPage((prev) => prev - 1)}
        >
          ◀
        </button>
        {Array.from({ length: totalPages }).map((_, idx) => (
          <button
            key={idx}
            className={currentPage === idx + 1 ? "active" : ""}
            onClick={() => setCurrentPage(idx + 1)}
          >
            {idx + 1}
          </button>
        ))}
        <button
          disabled={currentPage === totalPages}
          onClick={() => setCurrentPage((prev) => prev + 1)}
        >
          ▶
        </button>
      </div>
    </div>
  );
};

export default TravelCards;

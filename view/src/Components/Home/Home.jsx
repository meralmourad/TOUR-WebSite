import React, { useState } from "react";
import Rate from "../Rate/Rate";
import { useSelector } from "react-redux";

const tripsData = [
  { id: 1, city: "London", description: "An exciting trip to London in 5 days", rating: 4, image: "london.jpg" },
  { id: 2, city: "Maldives", description: "Relaxing trip to Maldives in 5 days", rating: 5, image: "maldives.jpg" },
  { id: 3, city: "Greece", description: "An exciting trip to Greece in 5 days", rating: 5, image: "greece.jpg" },
  { id: 4, city: "London", description: "Another London trip in 5 days", rating: 4, image: "london.jpg" },
  { id: 5, city: "Maldives", description: "Another Maldives trip", rating: 5, image: "maldives.jpg" },
  { id: 6, city: "Greece", description: "Another Greece trip", rating: 5, image: "greece.jpg" },
];

const TravelCards = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const [searchTerm, setSearchTerm] = useState("");

  const itemsPerPage = 3;
  const { user, isLoggedIn } = useSelector((store) => store.info);

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
      {console.log(user)} ;
      <h2>Hello , {user} <br /> DISCOVER THE WORLD NOW!</h2>

      <div className="search-filter">
        <input
          type="text"
          placeholder="Search destination..."
          value={searchTerm}
          onChange={handleSearchChange}
        />
        <button className="filter-button">
          🔍
        </button>
      </div>

      <div className="cards-grid">
        {currentTrips.map((trip) => (
          <div className="trip-card" key={trip.id}>
            <img src={trip.image} alt={trip.city} />
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

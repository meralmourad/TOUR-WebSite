import React, { useState } from "react";
import Filter from "../FilterTrip/Filter";

const TravelCards = () => {
  const [showFilter, setShowFilter] = useState(true);

  const toggleFilter = () => {
    setShowFilter(!showFilter); 
  };

  return (
    <>
        <div className="travel-cards-container">
            showFilter && (
                <div className="overlay">
                    <div className="sidebar">
                        <span
                            role="img"
                            aria-label="filter"
                            style={{ marginRight: "10px", cursor: "pointer" }}
                            onClick={toggleFilter} 
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
                    </div>)

                </div>
        </div>
    </>
    );
};

export default TravelCards;
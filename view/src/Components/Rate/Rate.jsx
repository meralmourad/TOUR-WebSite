import React, { useState } from "react";
import "./Rate.scss";

const StarRating = ({ outOf = 5, onChange }) => {
  const [rating, setRating] = useState(0);
  const [hover, setHover] = useState(0);

  const handleClick = (index) => {
    setRating(index);
    if (onChange) onChange(index); // send to parent if needed
  };

  return (
    <div className="star-rating">
      {Array.from({ length: outOf }).map((_, index) => {
        const starValue = index + 1;
        return (
          <span
            key={index}
            className={starValue <= (hover || rating) ? "filled" : ""}
            onClick={() => handleClick(starValue)}
            onMouseEnter={() => setHover(starValue)}
            onMouseLeave={() => setHover(0)}
          >
            ★
          </span>
        );
      })}
    </div>
  );
};

export default StarRating;

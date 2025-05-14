import React, { useState, useEffect } from "react";
import "./Rate.scss";

const StarRating = ({onChange, children , SendRate }) => {
  const outOf = 5 ;
  const [rating, setRating] = useState(0);
  const [hover, setHover] = useState(0);
  const [flag, setFlag] = useState(false);

  const handleClick = (index) => {
    setRating(index);
    if (onChange) onChange(index); 
  };

  useEffect(() => {
    if (children != null ) {
      setFlag(true); 
      handleClick(children) 
    }
  }, [children]);


  useEffect(() => {
    if (SendRate) {
      SendRate(rating);
    }
  }, [rating]);

  return (
    <div className="star-rating">
      {Array.from({ length: outOf }).map((_, index) => {
        const starValue = index + 1;
        return (
      <span
        key={index}
        className={starValue <= (hover || rating) ? "filled" : ""}
        onClick={!flag ? () => handleClick(starValue) : undefined}
        onMouseEnter={!flag ? () => setHover(starValue) : undefined}
        onMouseLeave={!flag ? () => setHover(0) : undefined}
      >
        ★
      </span>);
      })}
    </div>
  );
};

export default StarRating;
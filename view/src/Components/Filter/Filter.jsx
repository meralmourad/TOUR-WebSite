import React, { useState } from "react";
import "./Filter.scss";

const Filter = () => {
  const [price, setPrice] = useState(350);
  const [seats, setSeats] = useState(10);
  const [rate, setRate] = useState(4.3);
  const [fromDate, setFromDate] = useState("");
  const [toDate, setToDate] = useState("");
  const [additionalInput, setAdditionalInput] = useState(""); 

  const getGradient = (value, max) => {
    const percentage = (value / max) * 100;
    return `linear-gradient(to right, #d77a7a 0%, #d77a7a ${percentage}%, #ccc ${percentage}%, #ccc 100%)`; // نبيتي فاتح
  };

 
  const handleDateChange = (e, setter) => {
    let inputDate = e.target.value;
    const year = inputDate.split('-')[0]; 

    
    if (year.length <= 4) {
      setter(inputDate); 
    } else {
      alert("السنة يجب أن تحتوي على 4 أرقام فقط.");
    }
  };

  return (
    <div className="filter">
      <div className="filter-group">
        <label>Price</label>
        <input
          type="range"
          min="0"
          max="1000"
          value={price}
          onChange={(e) => setPrice(e.target.value)}
          style={{ background: getGradient(price, 1000) }}
        />
        <span>{price}</span>
      </div>

      <div className="filter-group">
        <label>Seats</label>
        <input
          type="range"
          min="1"
          max="100"
          value={seats}
          onChange={(e) => setSeats(e.target.value)}
          style={{ background: getGradient(seats, 100) }}
        />
        <span>{seats}</span>
      </div>

      <div className="filter-group">
        <label>Rate</label>
        <input
          type="range"
          min="0"
          max="5" 
          step="0.1"
          value={rate}
          onChange={(e) => setRate(e.target.value)}
          style={{ background: getGradient(rate, 5) }}
        />
        <span>{rate}</span>
      </div>

      
      <div className="filter-group">
        <label>Input</label>
        <input
          type="text"
          value={additionalInput}
          onChange={(e) => setAdditionalInput(e.target.value)}
        />
      </div>

      <div className="filter-group dates">
        <label>From Date</label>
        <input
          type="date"
          value={fromDate}
          onChange={(e) => handleDateChange(e, setFromDate)}
          min="1900-01-01"
          max="2099-12-31"
        />

        <label>To Date</label>
        <input
          type="date"
          value={toDate}
          onChange={(e) => handleDateChange(e, setToDate)}
          min="1900-01-01"
          max="2099-12-31"
        />
      </div>

      <div className="buttons">
        <button className="cancel">✖</button>
        <button className="apply">✔</button>
      </div>
    </div>
  );
};

export default Filter;

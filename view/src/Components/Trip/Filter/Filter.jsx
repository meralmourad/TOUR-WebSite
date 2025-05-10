import React, { useState } from "react";
import "./Filter.scss";
import swal from "sweetalert";
import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;

const Filter = ({ ShowFilter }) => {
  const [price, setPrice] = useState(350);
  const [seats, setSeats] = useState(10);
  const [rate, setRate] = useState(4);
  const [fromDate, setFromDate] = useState("");
  const [toDate, setToDate] = useState("");
  const [destination, setDestination] = useState("");

  const getGradient = (value, max) => {
    const percentage = (value / max) * 100;
    return `linear-gradient(to right, #d77a7a 0%, #d77a7a ${percentage}%, #ccc ${percentage}%, #ccc 100%)`;
  };

  const handleDateChange = (e, setter) => {
    const inputDate = e.target.value;
    setter(inputDate);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const fromDateObj = new Date(fromDate);
    const toDateObj = new Date(toDate);

    if (fromDateObj > toDateObj) {
      swal("Error", "From date cannot be later than To date", "error");
      return;
    }

    try {
      const response = await axios.get(`${API_URL}/Search/trips`, {
        params: {
          endPrice: price,
          startDate: fromDate,
          len: (toDateObj - fromDateObj) / (1000 * 60 * 60 * 24),
          destination: destination,
          // seats,
          // rate,
        },
      });
      console.log("Filtered Results:", response.data);
    } catch (error) {
      console.error("Error:", error);
    }
  };

  return (
    <>
      {ShowFilter && (
        <div className="filter">
          <div className="filter-group">
            <label>Price</label>
            <input
              type="range"
              min="0"
              max="1000"
              value={price}
              onChange={(e) => setPrice(Number(e.target.value))}
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
              onChange={(e) => setSeats(Number(e.target.value))}
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
              step="1"
              value={rate}
              onChange={(e) => setRate(Number(e.target.value))}
              style={{ background: getGradient(rate, 5) }}
            />
            <span>{rate}</span>
          </div>

          <div className="filter-group">
            <label>Destination</label>
            <input
              type="text"
              value={destination}
              onChange={(e) => setDestination(e.target.value)}
            />
          </div>

          <div className="filter-group dates">
            <label>From Date</label>
            <input
              type="date"
              value={fromDate}
              onChange={(e) => handleDateChange(e, setFromDate)}
              min="2010-01-01"
              max="2026-01-01"
            />

            <label>To Date</label>
            <input
              type="date"
              value={toDate}
              onChange={(e) => handleDateChange(e, setToDate)}
              min="2010-01-01"
              max="2026-01-01"
            />
          </div>

          <div className="buttons">
            <button
              className="cancel"
              onClick={() =>
                swal("Cancelled", "Filters were not applied", "info")
              }
            >
              ✖
            </button>
            <button className="apply" onClick={handleSubmit}>
              ✔
            </button>
          </div>
        </div>
      )}
    </>
  );
};

export default Filter;
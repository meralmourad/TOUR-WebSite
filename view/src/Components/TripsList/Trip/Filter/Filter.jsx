import React from "react";
import "./Filter.scss";
import swal from "sweetalert";
import { useSelector } from "react-redux";

const Filter = ({ id, setShowFilter, price, startDate, endDate, setPrice, setStartDate, setEndDate, isApproved, setIsApproved }) => {
  const { user } = useSelector((store) => store.info);
  const [pricce, setPricce] = React.useState(price || 1500);
  const [fromDate, setFromDate] = React.useState(startDate || "");
  const [toDate, setToDate] = React.useState(endDate || "");
  const [isApprovedFilter, setIsApprovedFilter] = React.useState(isApproved);

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
    setPrice(pricce);
    setStartDate(fromDate);
    setEndDate(toDate);
    setShowFilter(false);
    setIsApproved(isApprovedFilter);
  };

  return (
    <>
        <div className="filter">
          <div className="filter-group">
            <label>Price</label>
            <input
              type="range"
              min="0"
              max="5000"
              value={pricce}
              onChange={(e) => setPricce(Number(e.target.value))}
              style={{ background: getGradient(pricce, 5000) }}
            />
            <span>{pricce}</span>
          </div>

          {/* <div className="filter-group">
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
          </div> */}

          {/* <div className="filter-group">
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
          </div> */}

          {/* <div className="filter-group">
            <label>Destination</label>
            <input
              type="text"
              value={destination}
              onChange={(e) => setDestination(e.target.value)}
            />
          </div> */}

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
          { (user.role === 'Admin' || id) &&
            <div className="filter-group">
              <label>Approved</label>
              <input
                type="checkbox"
                className="custom-checkbox-filter"
                checked={isApprovedFilter}
                onChange={(e) => setIsApprovedFilter(e.target.checked)}
              />
            </div>
          }

          <div className="buttons">
            <button
              className="cancel"
              onClick={() =>{
                  swal("Cancelled", "Filters were not applied", "info")
                  setShowFilter(false);
                }
              }
            >
              ✖
            </button>
            <button className="apply" onClick={handleSubmit}>
              ✔
            </button>
          </div>
        </div>
    </>
  );
};

export default Filter;
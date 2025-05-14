import React, { useEffect, useState } from "react";
import "./BookingList.scss";
import { useNavigate, useParams } from "react-router-dom";
import { searchBookings } from "../../service/BookingService";
import { useSelector } from "react-redux";

const numberOfUsersPerPage = 9;

const BookingList = () => {
  const id = useParams().id;
  const { user } = useSelector((store) => store.info);
  const navigate = useNavigate();
  const [pageNumber, setPageNumber] = useState(1);
  const [numOfBooking, setNumOfBooking] = useState(0);
  const [bookings, setBookings] = useState([]);

  const numberOfPages = Math.ceil(numOfBooking / numberOfUsersPerPage);

  const [searchTerm, setSearchTerm] = useState("");

  const handleSearch = (event) => {
    setSearchTerm(event.target.value);
  };

  useEffect(() => {
    const start = (pageNumber - 1) * numberOfUsersPerPage;

    const fetchUsers = async () => {
      try {
        if(!user) return;
        const { bookings, totalCount } = await searchBookings(start, numberOfUsersPerPage, id, user?.id, 2);
        
        setNumOfBooking(totalCount);
        setBookings(bookings);
      } catch (error) {
        console.error("Error fetching users:", error);
      }
    };
    fetchUsers();
  }, [pageNumber, searchTerm, user?.id, id]);

  if (numberOfPages !== 0 && (pageNumber > numberOfPages || pageNumber < 1)) {
    setPageNumber(1);
  }

  const FunctionBtn = (i) => {
    if (i < 1 || i > numberOfPages) return <></>;
    return (
      <button
        onClick={() => setPageNumber(i)}
        key={i}
        style={i === pageNumber ? { backgroundColor: "#abdfa4" } : {}}
      >
        {i}
      </button>
    );
  };

  return (
    <>
      <br /> <br />
      <h2 className="users-list-title">List of All Booking</h2>
      <div className="users-list-header">
        <div className="search-bar">
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
            style={{
              border: "none",
              outline: "none",
              backgroundColor: "transparent",
              fontSize: "16px",
            }}
            placeholder="Search"
            value={searchTerm}
            onChange={handleSearch}
          />
        </div>
      </div>
      <div className="users-list-container">
        <br />
        {bookings.length === 0 && (
          <h2 className="users-list-title">No bookings Found</h2>
        )}
        <div className="users-list">
          {bookings.map((booking) => (
            <>{console.log(booking)}</>
            // <div key={booking.id} className="user-card" onClick={() => navigate(`/profile/${booking.id}`)}>
            //   <div className="user-image">
            //     <h5 style={{ margin: 0, fontSize: "16px" }}>{booking.name}</h5>
            //     <p
            //       style={{
            //         margin: 0,
            //         fontSize: "12px",
            //         color: "#801c29",
            //         fontWeight: "bold",
            //       }}
            //     >
            //       {booking.role}
            //     </p>
            //   </div>
            // </div>
          ))}
          <br />
        </div>
      </div>
      <div className="pagination">
        <button onClick={() => setPageNumber(Math.max(1, pageNumber - 1))}>
          &laquo;
        </button>

        {pageNumber - 3 >= 1 && <button>...</button>}

        {FunctionBtn(pageNumber - 2)}
        {FunctionBtn(pageNumber - 1)}
        {FunctionBtn(pageNumber)}
        {FunctionBtn(pageNumber + 1)}
        {FunctionBtn(pageNumber + 2)}

        {pageNumber + 3 <= numberOfPages && <button>...</button>}
        <button
          onClick={() => setPageNumber(Math.min(numberOfPages, pageNumber + 1))}
        >
          &raquo;
        </button>
      </div>
    </>
  );
};

export default BookingList;

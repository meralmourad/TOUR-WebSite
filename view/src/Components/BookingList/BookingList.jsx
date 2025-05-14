import React, { useEffect, useState } from "react";
import "./BookingList.scss";
import { useNavigate, useParams } from "react-router-dom";
import { searchBookings } from "../../service/BookingService";
import { useSelector } from "react-redux";
import Swal from "sweetalert2";
import { getTripById } from "../../service/TripsService";

const numberOfUsersPerPage = 9;

const BookingList = () => {
  const id = useParams().id;
  const { user } = useSelector((store) => store.info);
  const navigate = useNavigate();
  const [pageNumber, setPageNumber] = useState(1);
  const [numOfBooking, setNumOfBooking] = useState(0);
  const [bookings, setBookings] = useState([]);
  const [rejected, setRejected] = useState(false);

  const numberOfPages = Math.ceil(numOfBooking / numberOfUsersPerPage);

  useEffect(() => {
    const start = (pageNumber - 1) * numberOfUsersPerPage;

    const fetchUsers = async () => {
      try {
          const trip = await getTripById(id);
          // console.log(user);

          if(!user || !trip) return;

          if(user.role !== 'Admin' && trip.agenceId !== user.id) {
              Swal.fire({
                  icon: "error",
                  title: "Oops...",
                  text: "You are not allowed to access this page!"
              });
              navigate("/home");
              return;
          }
        const { bookings, totalCount } = await searchBookings(start, numberOfUsersPerPage, id, trip.agenceId, rejected? -1: 2);
        
        setNumOfBooking(totalCount);
        setBookings(bookings);
      } catch (error) {
        console.error("Error fetching users:", error);
      }
    };
    fetchUsers();
  }, [pageNumber, user?.id, id, rejected, user]);

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
          <span role="img" aria-label="search" style={{ marginRight: "10px", fontFamily: "sans-serif" }}>
            {/* <img
              src="/Icons/search.jpg"
              alt=""
              style={{
                width: "20px",
                height: "20px"
              }}
            ></img> */}
            rejected
            </span>
            <input
                type="checkbox"
                className="custom-checkbox-filter"
                value={rejected}
                onChange={() => setRejected(!rejected)}
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
              <div key={booking.id}
              className={`user-card`}
              style={{backgroundColor: `${booking.isApproved === -1? "#f2a59c":
                booking.isApproved === 0? "#fdf5cd":
                new Date(booking.trip.endDate) > new Date()? "#91bceb":
                "#a7e8a1"}`}}
                >
                {/* <>{console.log(booking)}</> */}
              <div className="user-image">
                <h5 style={{ margin: 0, fontSize: "16px" }} onClick={() => navigate(`/profile/${booking.tourist.id}`)}>
                    {booking.tourist.name}</h5>
                <p
                  style={{
                    margin: 0,
                    fontSize: "12px",
                    color: "#801c29",
                    fontWeight: "bold",
                  }}
                  onClick={() => navigate(`/Trip/${booking.trip.id}`)}
                >
                  {booking.trip.title}
                </p>
              </div>
            </div>
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

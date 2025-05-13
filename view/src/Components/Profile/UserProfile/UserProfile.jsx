import React, { use, useEffect, useState } from "react";
import "./UserProfile.scss";
import { FaArrowRight, FaUser } from "react-icons/fa";
import { updateUser } from "../../../service/UserService";
import { useDispatch } from "react-redux";
import { setUser } from "../../../Store/Slices/UserSlice";
import { useNavigate } from "react-router-dom";
import { searchBookings } from "../../../service/BookingService";

const UserProfile = ({ userprofile, myProfile }) => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [editMode, setEditMode] = useState(false);
  const [user, setUser1] = useState(userprofile);
  const [start, setStart] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const [bookings, setBookings] = useState([]);
  const [myBookings, setMyBookings] = useState([]);
  const itemsPerPage = 3;

  const changehandle = (e) => {
    setUser1({ ...user, [e.target.name]: e.target.value });
  };

  useEffect(() => {
    const fetching = async () => {
      try {
        const { bookings, totalCount } = await searchBookings(start, itemsPerPage, null, userprofile.id);
        setMyBookings(totalCount)
        setBookings(bookings);
        // console.log(bookingsData);
      }
      catch (error) {
        console.error("Error Fetching Data: ", error);
      }
    };

    fetching();
  }, [user, userprofile, start]);

  const updateUserForm = async () => {
      const { token } = JSON.parse(localStorage.getItem("Token"));
      try {
          await updateUser(userprofile.id, user);
          const new_data = {user, token};
          console.log(new_data);
          setEditMode(false);
          dispatch(setUser(new_data));
      } catch (error) {
          console.error("Error fetching profile:", error);
      }
  };

  const numberOfpages = Math.ceil(myBookings / itemsPerPage);

  return (
    <div className="user-profile-container">
      <div className="left-section">
          <div className="last-trips">{bookings?.length > 0? "Last Trips":"NO Trips Yet"}</div>
          {
            bookings.map((booking) => 
              <div className={`trip ${booking.isApproved === -1? "red": 
                                      booking.isApproved === 0? "yellow":
                                      new Date(booking.trip.endDate) < new Date()? "blue":
                                      "green"}`}
                    key={booking.id}>
                <div className="trip-text">
                  {booking.trip.locations.$values[0]}<br />
                  <span>{booking.trip.title}</span>
                </div>
                <div className="arrow-wrapper" onClick={() => navigate(`/Trip/${booking.tripId}`)}>
                  <FaArrowRight className="arrow-icon" />
                </div>
              </div>
            )
          }

          <div className="pagination">
            <button
              disabled={currentPage === 1}
              onClick={() => {
                setCurrentPage(currentPage - 1);
                setStart(start - itemsPerPage);
              }}
            >
              ◀
            </button>

            {Array.from({ length: numberOfpages }).map((_, index) => (
            <div key={index}>
              { Math.abs(currentPage - (index + 1)) <= 2 &&
                <button
                  
                  className={currentPage === index + 1 ? "active" : ""}
                  onClick={() => {
                    setCurrentPage(index + 1);
                    setStart(index * itemsPerPage);
                  }}
                >
                  {index + 1}
                </button>
              }
            </div>
            ))}
            <button
              disabled={currentPage === numberOfpages}
              onClick={() => {
                setCurrentPage(currentPage + 1);
                setStart(start + itemsPerPage);
              }}
            >
              ▶
            </button>
          </div>
      </div>

      <div className="right-section">
        <div className="profile-wrapper">
          {myProfile && <h2>Hello {userprofile.name.toUpperCase()}!</h2>}
          {!myProfile && <h2>{userprofile.name.toUpperCase()}'S PROFILE</h2>}
          <div className="profile-box">
            <div className="icon-section">
              <FaUser className="user-icon" />
            </div>
            <div className="info-section">
              {!editMode && (
                <>
                  <div>
                    <strong>Name :</strong> {user.name}
                  </div>
                  <div>
                    <strong>Email :</strong> {user.email}
                  </div>
                  <div>
                    <strong>Phone :</strong> {user.phoneNumber}
                  </div>
                  <div>
                    <strong>Country :</strong> {user.address}
                  </div>
                  <div>
                    {myProfile && <button
                      className="edit-btn"
                      onClick={() => setEditMode(true)}
                    >
                      Edit
                    </button>}
                  </div>
                </>
              )}
              {editMode && (
                <>
                  <div>
                    <strong>Name :</strong>{" "}
                    <input
                      type="text"
                      defaultValue={user.name}
                      name="name"
                      onChange={changehandle}
                    />
                  </div>
                  <div>
                    <strong>Email :</strong> {userprofile.email}
                  </div>
                  <div>
                    <strong>Phone :</strong>{" "}
                    <input
                      type="text"
                      defaultValue={user.phoneNumber}
                      name="phoneNumber"
                      onChange={changehandle}
                    />
                  </div>
                  <div>
                    <strong>Country :</strong>{" "}
                    <input
                      type="text"
                      defaultValue={user.address}
                      name="address"
                      onChange={changehandle}
                    />
                  </div>
                  <div className="button-group">
                    <button
                      className="discard-button"
                      onClick={() => setEditMode(false)}
                    >
                      Discard
                    </button>
                    <button
                      className="confirm-button"
                      onClick={updateUserForm}
                    >
                      Confirm
                    </button>
                  </div>
                </>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UserProfile;

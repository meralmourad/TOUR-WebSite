import React, { useState, useEffect, use } from "react";
import "./UserProfile.scss";
import { FaArrowRight, FaUser } from "react-icons/fa";
import swal from "sweetalert";
import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;
const token = localStorage.getItem("token");

const UserProfile = ({ userprofile }) => {
  const [editMode, setEditMode] = useState(false);
  const [user, setUser] = useState(userprofile);
  const [confirm, setConfirm] = useState(false);
  const [start, setStart] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 3;

  const changehandle = (e) => {
    setUser({ ...user, [e.target.name]: e.target.value });
  };

  // useEffect(() => {
  //   const fetchUserTrips = async () => {
  //     try {
  //       const response = await axios.get(`${API_URL}/User/${userprofile.id}/start?=${start}len=${itemsPerPage}`, {
  //         headers: {
  //           Authorization: `Bearer ${token}`,
  //         },
  //       });
  //       if(response.data.$values.length === 0) {
  //         setCurrentPage(1);
  //         setStart(0);
  //         swal("Sorry!", "No trips found", "error");
  //       }
  //     } catch (error) {
  //       console.error("Error fetching user profile:", error);
  //     }
  //   };

  //   fetchUserTrips();
  // }, start , currentPage);
  
  
  // api for fetching how many pages found ??

  return (
    <div className="user-profile-container">
      {console.log("userprofile", userprofile)}
      <div className="left-section">
        <div className="last-trips">Last Trips</div>

        <div className="trip green">
          <div className="trip-text">
            Egypt trip<br />
            <span>world Agency</span>
          </div>
          <div className="arrow-wrapper">
            <FaArrowRight className="arrow-icon" />
          </div>
        </div>

        <div className="trip red">
          <div className="trip-text">
            Egypt trip<br />
            <span>world Agency</span>
          </div>
          <div className="arrow-wrapper">
            <FaArrowRight className="arrow-icon" />
          </div>
        </div>

        <div className="trip blue">
          <div className="trip-text">
            Egypt trip<br />
            <span>world Agency</span>
          </div>
          <div className="arrow-wrapper">
            <FaArrowRight className="arrow-icon" />
          </div>
        </div>

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

          {Array.from({ length: 4 }).map((_, index) => (
            <button
              key={index}
              className={currentPage === index + 1 ? "active" : ""}
              onClick={() => {
                setCurrentPage(index + 1);
                setStart(index * itemsPerPage);
              }}
            >
              {index + 1}
            </button>
          ))}
          <button
            disabled={currentPage === 4}
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
          <h2>Hello {userprofile.name.toUpperCase()}!</h2>
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
                    <button
                      className="edit-btn"
                      onClick={() => setEditMode(true)}
                    >
                      Edit
                    </button>
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
                      onClick={() => {
                        setConfirm(true);
                      }}
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

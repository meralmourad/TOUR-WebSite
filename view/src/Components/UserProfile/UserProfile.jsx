import React from "react";
import "./UserProfile.scss";
import { FaArrowRight, FaUser } from "react-icons/fa";

const UserProfile = () => {
  return (
    <div className="user-profile-container">
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
          <span>{'‹'}</span>
          <span className="active">1</span>
          <span>2</span>
          <span>3</span>
          <span>....</span>
          <span>{'›'}</span>
        </div>
      </div>

      <div className="right-section">
        <div className="profile-wrapper">
          <h2>Hello Name !</h2>
          <div className="profile-box">
            <div className="icon-section">
              <FaUser className="user-icon" />
            </div>
            <div className="info-section">
              <div><strong>Name :</strong> Feby Ashraf</div>
              <div><strong>Email :</strong> febyashraf@example.com</div>
              <div><strong>Phone :</strong> 01012345678</div>
              <div><strong>Country :</strong> Egypt</div>
            </div>
            <button className="edit-btn">Edit</button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UserProfile;

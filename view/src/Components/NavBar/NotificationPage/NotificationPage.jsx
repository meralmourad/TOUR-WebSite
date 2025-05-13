import React, { useState } from "react";
import "./NotificationPage.scss";

const NotificationPage = () => {
  const numberOfUsersPerPage = 8;

  const [numOfUsers, setNumOfUsers] = useState(0);
  const [pageNumber, setPageNumber] = useState(1);

  const numberOfPages = Math.ceil(numOfUsers / numberOfUsersPerPage);

  // api fetching messages websocket

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
    <div className="notification-container">
      <h2 className="notification-title">Notifications</h2>
      <div className="notification-list">
        <div className="notification-item">
          <span className="notification-dot"></span>
          <p className="notification-text">You have a new message.</p>
        </div>
        <div className="notification-item">
          <span className="notification-dot"></span>
          <p className="notification-text">Your booking has been confirmed.</p>
        </div>
        <div className="notification-item">
          <span className="notification-dot"></span>
          <p className="notification-text">Your payment was successful.</p>
        </div>
        <div className="notification-item">
          <span className="notification-dot"></span>
          <p className="notification-text">Reminder: Your trip is tomorrow.</p>
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
    </div>
  );
};

export default NotificationPage;

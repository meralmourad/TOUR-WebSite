import React, { useEffect, useRef, useState } from "react";
import "./NotificationPage.scss";
import { getNotifications } from "../../../service/NotificationsService";
import { useDispatch, useSelector } from "react-redux";
import { setChat } from "../../../Store/Slices/ChatSlice";
import { useNavigate } from 'react-router-dom';

const WS_URL = process.env.REACT_APP_WS_URL;

const NotificationPage = () => {
  const { user, token } = useSelector((store) => store.info);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const numberOfUsersPerPage = 8;

  const [numOfUsers, setNumOfUsers] = useState(0);
  const [pageNumber, setPageNumber] = useState(1);
  const [notifications, setNotifications] = useState([]);

  const numberOfPages = Math.ceil(numOfUsers / numberOfUsersPerPage);

  
  useEffect(() => {
    const start = (pageNumber - 1) * numberOfUsersPerPage;
    const fetchData = async () => {
      try {
        const { allNotifications, totalCount } = await getNotifications(user.id);
        allNotifications.reverse();
        setNotifications(allNotifications);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, [user, pageNumber]);

  const ws = useRef(null);

  // useEffect(() => {
  //   ws.current = new WebSocket(`${WS_URL}/notification/${user.id}?token=${token}`);
  //   ws.current.onopen = () => {
  //     console.log("WebSocket connection opened");
  //   };
  //   ws.current.onmessage = (event) => {
  //     const newNotification = JSON.parse(event.data);
  //     const newArr = [newNotification, ...notifications];
  //     if(newArr.length > numberOfUsersPerPage) {
  //       newArr.pop();
  //     }
  //     setNotifications(newArr);
  //   }
  //   ws.current.onclose = () => {
  //     console.log("WebSocket connection closed");
  //   };
  //   ws.current.onerror = (error) => {
  //     console.error("WebSocket error:", error);
  //   };
  //   return () => {
  //     if (ws.current) {
  //       ws.current.close();
  //       console.log("WebSocket connection closed");
  //     }
  //   }
  // }
  // , []);

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


      {notifications.map((notification) => (
        <div key={notification.id} className="notification-list">
          <div className="notification-item" onClick={() => {
            if(notification?.senderId) {
              dispatch(setChat({receiverId: notification.senderId, senderId: user.id}));
            }
            else {
              navigate('Trip/' + notification.TripId);
            }
          }}>
            <span className="notification-dot"></span>
            <p className="notification-text">{ notification.context }</p>
          </div>
          {/* <br /> */}
        </div>
      ))}


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

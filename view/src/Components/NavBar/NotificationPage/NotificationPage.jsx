import React from "react";
import "./NotificationPage.scss";

const NotificationPage = () => {
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
                <span className="pagination-arrow">&lt;</span>
                <span className="pagination-page">1</span>
                <span className="pagination-page">2</span>
                <span className="pagination-page">3</span>
                <span className="pagination-dots">...</span>
                <span className="pagination-arrow">&gt;</span>
            </div>
        </div>
    );
};

export default NotificationPage;
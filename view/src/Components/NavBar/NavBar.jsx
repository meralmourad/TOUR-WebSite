import React from 'react';
import './NavBar.scss';

const NavBar = () => {
    return (
        <nav className="navbar">
            <div className="navbar-container">
                
                <div className="navbar-left">
                    {true && 
                    <>
                        <a href="#flag" className="nav-link">
                            <img src={'Icons/flag icon.jpg'} alt="Flag" className="icon" />
                        </a>
                        <a href="#chat" className="nav-link">
                            <img src={'Icons/chat icon.jpg'} alt="Chat" className="icon" />
                        </a>
                    </>
                    }
                </div>

                <ul className="navbar-links">
                    <li className="nav-item">
                        <a href="profile" className="nav-link">
                            <img src={'Icons/Profile.jpg'} alt="Profile" className="icon" />
                        </a>
                    </li>
                    <li className="nav-item">
                        <a href="#home" className="nav-link">
                            <img src={'Icons/HomeIcon.jpg'} alt="Home" className="icon" />
                        </a>
                    </li>
                    <li className="nav-item">
                        <a href="#notification" className="nav-link">
                            <img src={'Icons/Notification.jpg'} alt="Notification" className="icon" />
                        </a>
                    </li>
                    <li className="nav-item">
                        <a href="#settings" className="nav-link">
                            <img src={'Icons/Settings.jpg'} alt="Settings" className="icon" />
                        </a>
                    </li>
                </ul>
            </div>
        </nav>
    );
};

export default NavBar;
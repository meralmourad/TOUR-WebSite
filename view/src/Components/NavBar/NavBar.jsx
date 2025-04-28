import React from 'react';
import { Link } from 'react-router-dom';
import './NavBar.scss';

const NavBar = () => {
    return (
        <nav className="navbar">
            <div className="navbar-container">
                
                <div className="navbar-left">
                    {true && 
                    <>
                        <Link to="#flag" className="nav-link">
                            <img src={'Icons/flag icon.jpg'} alt="Flag" className="icon" />
                        </Link>
                        <Link to="#chat" className="nav-link">
                            <img src={'Icons/chat icon.jpg'} alt="Chat" className="icon" />
                        </Link>
                    </>
                    }
                </div>

                <ul className="navbar-links">
                    <li className="nav-item">
                        <Link to="profile" className="nav-link">
                            <img src={'Icons/Profile.jpg'} alt="Profile" className="icon" />
                        </Link>
                    </li>
                    <li className="nav-item">
                        <Link to="#home" className="nav-link">
                            <img src={'Icons/HomeIcon.jpg'} alt="Home" className="icon" />
                        </Link>
                    </li>
                    <li className="nav-item">
                        <Link to="#notification" className="nav-link">
                            <img src={'Icons/Notification.jpg'} alt="Notification" className="icon" />
                        </Link>
                    </li>
                    <li className="nav-item">
                        <Link to="#settings" className="nav-link">
                            <img src={'Icons/Settings.jpg'} alt="Settings" className="icon" />
                        </Link>
                    </li>
                </ul>
            </div>
        </nav>
    );
};

export default NavBar;
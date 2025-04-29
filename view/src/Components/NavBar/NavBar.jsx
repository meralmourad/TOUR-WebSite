import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './NavBar.scss';
import { useLocation } from "react-router-dom";
import { useDispatch, useSelector } from 'react-redux';
import { clearUser } from '../../Store/Slices/UserSlice';



const NavBar = () => {
    const { isLoggedIn } = useSelector((store) => store.info);
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const location = useLocation();

    const url = location.pathname;
    console.log(url);

    if(url === "/welcome") {
        return <></>;
    }

    const logout = () => {
        localStorage.removeItem("Token");
        dispatch(clearUser());
        navigate("/welcome");
    }

    return (
        <nav className="navbar">
            <div className="navbar-container">
                
                <div className="navbar-left">
                    {(url === "/AgencyProfile" || url === "/TouristProfile") &&
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

                {isLoggedIn && 
                    <ul className="navbar-links">
                        <li className="nav-item">
                            <Link to="/profile" className="nav-link">
                                <img src={'Icons/Profile.jpg'} alt="Profile" className="icon" />
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/" className="nav-link">
                                <img src={'Icons/HomeIcon.jpg'} alt="Home" className="icon" />
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="#notification" className="nav-link">
                                <img src={'Icons/Notification.jpg'} alt="Notification" className="icon" />
                            </Link>
                        </li>
                        <li className="nav-item">
                            <button onClick={logout} className="nav-link">
                                <img src={'Icons/logout.png'} alt="logout" className="icon" />
                            </button>
                        </li>
                    </ul>
                }

                {!isLoggedIn &&
                    <ul className="navbar-links">
                        <li className="nav-item">
                            <Link to="/" className="nav-link">
                                <img src={'Icons/HomeIcon.jpg'} alt="Home" className="icon" />
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/login" className="nav-link">
                            <img src={'Icons/login.png'} alt="Settings" className="icon" />
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/signup" className="nav-link">
                            <img src={'Icons/signup.png'} alt="Settings" className="icon" />
                            </Link>
                        </li>
                    </ul>
                }
            </div>
        </nav>
    );
};

export default NavBar;
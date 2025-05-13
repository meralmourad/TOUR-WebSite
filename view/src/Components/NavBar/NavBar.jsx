import './NavBar.scss';
import { Link, useNavigate } from 'react-router-dom';
import { useLocation } from "react-router-dom";
import { useDispatch, useSelector } from 'react-redux';
import { clearUser } from '../../Store/Slices/UserSlice';

const NavBar = ({ setShowChat }) => {
    const { isLoggedIn, user } = useSelector((store) => store.info);
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const location = useLocation();

    const url = location.pathname;
    const id = +url.split("/")[2];

    if(url === "/welcome" || url === '/login' || url === '/signup') {
        return <></>;
    }

    const logout = () => {
        document.body.style.cursor = 'wait';
        setTimeout(() => {
            document.body.style.cursor = 'default';
            window.localStorage.removeItem("Token");
            dispatch(clearUser());
            navigate("/");
        }, 1500);
    }

    return (
        <nav className="navbar">
            <div className="navbar-container">
                <div className="navbar-left">
                    {/* {console.log(id, user?.id)} */}
                    {(url.includes("/profile")) && id !== user?.id &&
                        <Link onClick={() => setShowChat(true)} className="nav-link">
                            <img src={'/Icons/chat icon.jpg'} alt="Chat" className="icon" />
                        </Link>
                    }
                </div>

                {isLoggedIn && 
                    <ul className="navbar-links">
                        {
                            user.role === "Admin" &&
                            <>
                                <li className="nav-item">
                                    <Link to={`/UsersPending`} className="nav-link">
                                        <img src={'/Icons/pendingUsers.png'} alt="pending users" title='pending users' className="icon" />
                                    </Link>
                                </li>
                                <li className="nav-item">
                                    <Link to={`/TripsPending`} className="nav-link">
                                        <img src={'/Icons/TripsPending.png'} alt="pending trips" title='pending trips' className="icon" />
                                    </Link>
                                </li>
                                <li className="nav-item">
                                    <Link to={`/userslist`} className="nav-link">
                                        <img src={'/Icons/usersList.png'} alt="users list" title='users list' className="icon" />
                                    </Link>
                                </li>
                            </>
                        }
                        <li className="nav-item">
                            <Link to={`/profile/${user.id}`} className="nav-link">
                                <img src={'/Icons/Profile.jpg'} alt="Profile" className="icon" />
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/home" className="nav-link">
                                <img src={'/Icons/HomeIcon.jpg'} alt="Home" className="icon" />
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="#notification" className="nav-link">
                                <img src={'/Icons/Notification.jpg'} alt="Notification" className="icon" />
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link onClick={logout} className="nav-link">
                                <img src={'/Icons/logout.png'} alt="logout" className="icon" />
                            </Link>
                        </li>
                    </ul>
                }

                {!isLoggedIn &&
                    <ul className="navbar-links">
                        <li className="nav-item">
                            <Link to="/" className="nav-link">
                                <img src={'/Icons/HomeIcon.jpg'} alt="Home" className="icon" />
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/login" className="nav-link">
                            <img src={'/Icons/login.png'} alt="" className="icon" />
                            </Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/signup" className="nav-link">
                            <img src={'/Icons/signup.png'} alt="" className="icon" />
                            </Link>
                        </li>
                    </ul>
                }
            </div>
        </nav>
    );
};

export default NavBar;
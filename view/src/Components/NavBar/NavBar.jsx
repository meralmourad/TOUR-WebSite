import React from 'react';
import './NavBar.scss';

const NavBar = () => {
    return (
        <nav className="navbar"> 
            <div className="navbar-header">          
            <ul className="navbar-links">
                <li className='nav-item' >
                <a href="profile">
                    <img src={'Profile.jpg'} alt="" className="icon" />
                </a>
                </li>
                <li className='nav-item'>
                    <a href="#home">
                        <img src={'HomeIcon.jpg'} alt="" className="icon" />
                    </a>
                </li>
                <li className='nav-item'>
                    <a href="#notification">
                        <img src={'Notification.jpg'} alt="" className="icon" />
                    </a>
                </li>
                <li className='nav-item'>
                    <a href="#settings">
                        <img src={'Settings.jpg'} alt="" className="icon" />
                    </a>
                </li>
            </ul>
            </div>
        </nav>
    );
};

export default NavBar;
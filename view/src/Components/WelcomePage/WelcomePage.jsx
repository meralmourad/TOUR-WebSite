import React from 'react';
import './WelcomePage.scss';

const WelcomePage = () => {
  return (
    <div className="welcome-page">
      <div className="welcome-page__left">
        <img
          src='/Icons/map.jpg'
          alt="Map"
          className="welcome-page__image"
        />
      </div>
      <div className="welcome-page__right">
        <h4 className="welcome-page__small-title">TOUR AGENCIES</h4>
        <h1 className="welcome-page__main-title">
          DISCOVER <br /> WORLD
        </h1>
        <p className="welcome-page__description">
          Take only memories, leave only footprints.
        </p>
        <button className="welcome-page__button">GET STARTED &gt;</button>
      </div>
    </div>
  );
};

export default WelcomePage;

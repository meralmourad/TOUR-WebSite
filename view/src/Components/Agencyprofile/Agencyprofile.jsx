import React from "react";
import "./Agencyprofile.css";

const AgencyProfile = () => {
    return (
        <div className="agency-profile">
            <header className="header">
                <div className="header-images">
                    <img
                        src="https://via.placeholder.com/600x300"
                        alt="Agency Banner 1"
                        className="header-image"
                    />
                    <img
                        src="https://via.placeholder.com/600x300"
                        alt="Agency Banner 2"
                        className="header-image"
                    />
                </div>
                <div className="header-text">
                    <h1>Agency</h1>
                    <h2>FREEDOM</h2>
                </div>
            </header>

            <main className="main-content">
                <section className="highest-rated">
                    <h3>HIGHEST RATED</h3>
                    <div className="rated-item">
                        <img
                            src="https://via.placeholder.com/100"
                            alt="London"
                            className="rated-image"
                        />
                        <div className="rated-info">
                            <h4>LONDON</h4>
                            <p>An exciting trip to London in 5 days</p>
                            <div className="stars">⭐⭐⭐⭐⭐</div>
                        </div>
                    </div>
                    <div className="rated-item">
                        <img
                            src="https://via.placeholder.com/100"
                            alt="London"
                            className="rated-image"
                        />
                        <div className="rated-info">
                            <h4>LONDON</h4>
                            <p>An exciting trip to London in 5 days</p>
                            <div className="stars">⭐⭐⭐⭐⭐</div>
                        </div>
                    </div>
                    <button className="see-more">SEE MORE →</button>
                </section>

                <section className="agency-details">
                    <div className="details-row">
                        <div className="detail">
                            <span>Name:</span> Miral Agency
                        </div>
                        <div className="detail">
                            <span>E-mail:</span> miral@gmail.com
                        </div>
                    </div>
                    <div className="details-row">
                        <div className="detail">
                            <span>Phone:</span> 0111111121
                        </div>
                        <div className="detail">
                            <span>Country:</span> EGYPT
                        </div>
                    </div>
                    <button className="edit-button">Edit</button>
                </section>
            </main>
        </div>
    );
};

export default AgencyProfile;
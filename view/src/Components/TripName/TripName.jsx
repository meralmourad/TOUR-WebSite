
import { useState } from "react";
import "./TripName.scss";

const images = [
  "https://ultimahoracol.com/sites/default/files/2024-12/PORTADAS%20ESCRITORIO%20-%202024-12-19T122111.264.jpg", 
  "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTCVuJDmSjVmO1RzJaVuLlix7evJoVWOhL4ghYK0mlJad4o_w2nu8H3UOUF&s=10", 
  "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ45rDg-QQbP8l4fp0IT1B1zDLU8BdxV_LIFToRuNG9KEPsc52B4B9rlcX4&s=10",  
];

const TripName = () => {
  const [currentIndex, setCurrentIndex] = useState(0);

  const prevSlide = () => {
    setCurrentIndex((prev) => (prev === 0 ? images.length - 1 : prev - 1));
  };

  const nextSlide = () => {
    setCurrentIndex((prev) => (prev === images.length - 1 ? 0 : prev + 1));
  };

  return (
    <div className="trip-container">
      <div className="trip-header">
        <img
          src={images[currentIndex]}
          alt="Trip"
          className="trip-image"
        />
        <button className="arrow-button left" onClick={prevSlide}>
          &#8249;
        </button>
        <button className="arrow-button right" onClick={nextSlide}>
          &#8250;
        </button>
      </div>

      <div className="trip-details">
        <div className="trip-section">
          <h3>Available Sets:</h3>
          <p className="highlight">300 Person</p>
          <h3>Agency Name:</h3>
          <p>Miral Agency</p>
        </div>

        <div className="trip-section">
          <h3>Destinations</h3>
          <ul>
            <li>GIZA</li>
            <li>CAIRO</li>
            <li>ASWAN</li>
          </ul>
          <p><strong>From:</strong> 4 - 4 - 2025</p>
          <p><strong>To:</strong> 14 - 4 - 2025</p>
        </div>

        <div className="trip-section">
          <h3>Price:</h3>
          <p className="highlight">500 Per Person</p>
          <h3>Description</h3>
          <p>a wonderful experience</p>
          <p>a wonderful experience</p>
        </div>
      </div>

      <div className="trip-footer">
        <button className="book-now-button">
          BOOK NOW!
          <span className="arrow">→</span>
        </button>
      </div>
    </div>
  );
};

export default TripName;
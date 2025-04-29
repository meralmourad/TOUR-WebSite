
import "./Agencyprofile.scss";

const AgencyProfile = () => {
    return (
        <div className="agency-profile">
           <div className="header">
  <div className="header-images">
    <img src="https://beta.sis.gov.eg/media/195319/pyramids-2371501-1920.jpg" alt="Alexandria" />
    <img src="https://kenzly.com/wp-content/uploads/2025/04/Cairo-to-Alexandria-Full-Day-Tour-3.webp" alt="Pyramids" />
    <div className="overlay-title">
      <h1>Agency <br /> FREEDOM</h1>
    </div>
  </div>
</div>
            <div className="content">
                <div className="highest-rated">
                    <h2>HIGHEST RATED</h2>
                    <div className="rated-item">
                        <img
                            src="https://pbs.twimg.com/media/DYafkmPW0AEYSEI.jpg:large"
                            alt="London"
                            className="rated-img"
                        />
                        <div className="rated-info">
                            <h3>LONDON</h3>
                            <p>An exciting trip to London in 5 days</p>
                            <div className="stars">⭐⭐⭐⭐⭐</div>
                        </div>
                    </div>
                    <div className="rated-item">
                        <img
                            src="https://erem-media-service.azurewebsites.net/api/ResizeImage?image=https://cdn.foochia.com/media/2bbfff8c-d39c-482e-8b4c-daff62096a4b.webp&height=780&width=780&fit=cover"
                            alt="London"
                            className="rated-img"
                        />
                        <div className="rated-info">
                            <h3>LONDON</h3>
                            <p>An exciting trip to London in 5 days</p>
                            <div className="stars">⭐⭐⭐⭐⭐</div>
                        </div>
                    </div>
                    <button className="see-more">SEE MORE →</button>
                </div>
                <div className="agency-info">
                    <div className="info-item">
                        <label>Name:</label>
                        <span>Miral Agency</span>
                    </div>
                    <div className="info-item">
                        <label>E-mail:</label>
                        <span>miral@gmail.com</span>
                    </div>
                    <div className="info-item">
                        <label>Phone:</label>
                        <span>0111111121</span>
                    </div>
                    <div className="info-item">
                        <label>Country:</label>
                        <span>EGYPT</span>
                    </div>
                    <button className="edit-button">Edit</button>
                </div>
            </div>
        </div>
    );
};

export default AgencyProfile;
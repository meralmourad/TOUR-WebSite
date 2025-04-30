import { useEffect, useState } from "react";
import axios from 'axios';
import "./Agencyprofile.scss";
import { useParams } from "react-router-dom";

const API_URL = process.env.REACT_APP_API_URL;

const AgencyProfile = () => {
    const id = useParams().id;
    const { token } = JSON.parse(localStorage.getItem("Token"));
    const [error, setError] = useState('');
    const [user, setUser] = useState(null);
    const [edit, setEdit] = useState(false);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get(`${API_URL}/User/${id}`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });
                console.log(response.data);
                
                setUser(response.data);
            } catch (error) {
                console.error(error.response.data);
                setError(error.response.data);
            }
        };
        fetchData();
    }, []);

    return (
        <div className="agency-profile">
            <div className="header">
            <div className="header-images">
                <img
                src="https://beta.sis.gov.eg/media/195319/pyramids-2371501-1920.jpg"
                alt="Alexandria"
                />
                <img
                src="https://kenzly.com/wp-content/uploads/2025/04/Cairo-to-Alexandria-Full-Day-Tour-3.webp"
                alt="Pyramids"
                />
                <div className="overlay-title">
                <h1>
                    Agency <br /> FREEDOM
                </h1>
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
            {user && <>
                <div className="info-item">
                <label>Name:</label>
                <span>{user.name}</span>
                </div>
                <div className="info-item">
                <label>E-mail:</label>
                <span>{user.email}</span>
                </div>
                <div className="info-item">
                <label>Phone:</label>
                <span>{user.phoneNumber}</span>
                </div>
                <div className="info-item">
                <label>Country:</label>
                <span>{user.address}</span>
                </div>
                <button className="edit-button" onClick={() => setEdit(!edit)}>Edit</button>
            </>}
            {error && <h2 style={{textAlign: "center"}}>User not found.</h2>}
            </div>
            </div>
        </div>
    );
};

export default AgencyProfile;

import axios from "axios";
import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import AgencyProfile from "../Agencyprofile/Agencyprofile";

const API_URL = process.env.REACT_APP_API_URL;

function Profile() {
    const [error, setError] = useState(null);
    const id = useParams().id;
    const { user, token } = useSelector((store) => store.info);
    const [userprofile, setUserProfile] = useState(null);

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const response = await axios.get(`${API_URL}/User/${id}`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });
                // console.log("Profile data:", response.data);
                setUserProfile(response.data);
            } catch (error) {
                setError(error.response.data);
                console.error("Error fetching profile:", error);
            }
        };
        if(user?.id)
            fetchProfile();
    }, [id, user, token]);

    if(!error && !userprofile) {
        return (
            <div style={{textAlign: "center", marginTop: "20px"}}>
                <h2>Loading...</h2>
            </div>
        );
    }


    if(userprofile) {
        if(userprofile.role === "Agency") {
            return <AgencyProfile userprofile={userprofile} myProfile={userprofile.id === user.id} />;
        }
        else { // admidn or user
            return (
                <div style={{textAlign: "center", marginTop: "20px"}}>
                    <h2>{userprofile.firstName} {userprofile.lastName}</h2>
                    <img src={userprofile.profilePicture} alt="Profile" style={{width: "200px", height: "200px"}} />
                    <p>Email: {userprofile.email}</p>
                    <p>Phone: {userprofile.phone}</p>
                    <p>Role: {userprofile.role}</p>
                </div>
            );
        }
    } 


    return (
        <div style={{textAlign: "center", marginTop: "20px"}}>
            <h2>{error}</h2>
        </div>
    );
}

export default Profile;

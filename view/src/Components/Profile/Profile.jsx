import axios from "axios";
import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import AgencyProfile from "./Agencyprofile/Agencyprofile";
import UserProfile from "./UserProfile/UserProfile";

const API_URL = process.env.REACT_APP_API_URL;

function Profile() {
    const [error, setError] = useState(null);
    const { user, token } = useSelector((store) => store.info);
    const [userprofile, setUserProfile] = useState(null);

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const response = await axios.get(`${API_URL}/User/${user.id}`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });
                console.log("Profile data:", response.data);
                setUserProfile(response.data);
            } catch (error) {
                setError(error.response.data);
                console.error("Error fetching profile:");
            }
        };
        if(user?.id)
            fetchProfile();
    }, [ user, token]);

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
            return <UserProfile userprofile={userprofile} />;
        }
    } 


    return (
        <div style={{textAlign: "center", marginTop: "20px"}}>
            <h2>error</h2>
        </div>
    );
}

export default Profile;

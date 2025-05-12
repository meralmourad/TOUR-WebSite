import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import AgencyProfile from "./Agencyprofile/Agencyprofile";
import UserProfile from "./UserProfile/UserProfile";
import { getUserById } from "../../service/UserService";

function Profile() {
    const { id } = useParams();
    const [error, setError] = useState(null);
    const { user } = useSelector((store) => store.info);
    const [userprofile, setUserProfile] = useState(null);

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const profile = await getUserById(id);
                setUserProfile(profile);
                // console.log("Fetched profile:", profile);
                
            } catch (error) {
                setError(error.response.data);
                console.error("Error fetching profile:");
            }
        };
        if(user?.id)
            fetchProfile();
    }, [user, id]);

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
        else { // admin or user
            return <UserProfile userprofile={userprofile} myProfile={userprofile.id === user.id} />;
        }
    } 


    return (
        <div style={{textAlign: "center", marginTop: "20px"}}>
            <h2>error: { error }</h2>
        </div>
    );
}

export default Profile;

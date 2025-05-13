import { useSelector } from "react-redux";
import "./TripsPending.scss";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { approveTrip, SearchTrips } from "../../service/TripsService";

const TripsPending = () => {
    const { user } = useSelector((store) => store.info);
    const navigate = useNavigate();
    const [searchTerm, setSearchTerm] = useState("");
    const [trips, setTrips] = useState([]);
    const [render, setRender] = useState(false);

    const handleSearchChange = (event) => {
        setSearchTerm(event.target.value);
    }

    useEffect(() => {
        const fetch = async () => {
            try {
                if(!user) return;
                if(user.role !== 'Admin') {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: "You are not allowed to access this page!"
                    });
                    navigate("/home");
                    return;
                }

                const { trips } = await SearchTrips(null, null, null, null, null, null, false, searchTerm);
                // console.log(trips);
                setTrips(trips);
            }
            catch (error) {
                console.error("Error fetching Agencys:", error);
            }
        }
        fetch();
    }, [user, searchTerm, render, navigate]);


    const accept = async (TripId) => {
        Swal.fire({
            title: "Are you sure?",
            text: "You are about to accept this Trip.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, accept it!"
        }).then(async (result) => {
            if (result.isConfirmed) {
                try {
                    await approveTrip(TripId, 1);
                    setRender(!render);
                    Swal.fire("Accepted!", "The Trip has been accepted.", "success");
                } catch (error) {
                    console.error("Error accepting Trip:", error);
                    Swal.fire("Error!", "There was an error accepting the Trip.", "error");
                }
            }
        });
    }

    const reject = async (TripId) => {
        Swal.fire({
            title: "Are you sure?",
            text: "You are about to reject this Trip.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#d33",
            cancelButtonColor: "#3085d6",
            confirmButtonText: "Yes, reject it!"
        }).then(async (result) => {
            if (result.isConfirmed) {
            try {
                await approveTrip(TripId, -1);
                setRender(!render);
                Swal.fire("Rejected!", "The Trip has been rejected.", "success");
            } catch (error) {
                console.error("Error rejecting Trip:", error);
                Swal.fire("Error!", "There was an error rejecting the Trip.", "error");
            }
            }
        });
    }

    return (
        <div className="Trip-pending-container travel-cards-container">
        <h2>Trip Pending</h2>
        <div className="search-filter">
            <span role="img" aria-label="search" style={{ marginRight: "10px" }}>
                <img
                src="/Icons/search.jpg"
                alt=""
                style={{
                    width: "20px",
                    height: "20px",
                }}
                ></img>
            </span>
            <input
                style={{ backgroundColor: "white" }}
                type="text"
                value={searchTerm}
                onChange={handleSearchChange}
                placeholder="Search by name"
            />
        </div>
            <div className="Trip-list">
                {trips?.length === 0 && <div className="no-Trip">No Trip Found</div>}
                {trips.map((Trip) => (
                    <div key={Trip.id} className="Trip-item" >
                        <div onClick={() => navigate(`/Trip/${Trip.id}`)}>
                            <span className="Trip-name">{Trip.title}   |   </span> 
                            <span className="Trip-small">{Trip?.description}</span>
                        </div>
                        <button className="reject-btn" onClick={() => reject(Trip.id)}>reject</button>
                        <button className="accept-btn" onClick={() => accept(Trip.id)}>accept</button>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default TripsPending;
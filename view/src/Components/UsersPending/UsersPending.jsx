import { useSelector } from "react-redux";
import "./UsersPending.scss";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { approveAgency, SearchUsers } from "../../service/UserService";

const UsersPending = () => {
    const { user } = useSelector((store) => store.info);
    const navigate = useNavigate();
    const [searchTerm, setSearchTerm] = useState("");
    const [users, setUsers] = useState([]);
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

                const { users } = await SearchUsers(null, null, false, true, false, false, searchTerm);
                // console.log(list);
                setUsers(users);
            }
            catch (error) {
                console.error("Error fetching Agencys:", error);
            }
        }
        fetch();
    }, [user, searchTerm, render, navigate]);


    const accept = async (AgencyId) => {
        Swal.fire({
            title: "Are you sure?",
            text: "You are about to accept this Agency.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, accept it!"
        }).then(async (result) => {
            if (result.isConfirmed) {
                try {
                    await approveAgency(AgencyId, 1);
                    setRender(!render);
                    Swal.fire("Accepted!", "The Agenct has been accepted.", "success");
                } catch (error) {
                    console.error("Error accepting Agency:", error);
                    Swal.fire("Error!", "There was an error accepting the Agency.", "error");
                }
            }
        });
    }

    const reject = async (agencyId) => {
        Swal.fire({
            title: "Are you sure?",
            text: "You are about to reject this Agency.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#d33",
            cancelButtonColor: "#3085d6",
            confirmButtonText: "Yes, reject it!"
        }).then(async (result) => {
            if (result.isConfirmed) {
            try {
                await approveAgency(agencyId, -1);
                setRender(!render);
                Swal.fire("Rejected!", "The Agency has been rejected.", "success");
            } catch (error) {
                console.error("Error rejecting Agency:", error);
                Swal.fire("Error!", "There was an error rejecting the Agency.", "error");
            }
            }
        });
    }

    return (
        <div className="Agency-pending-container travel-cards-container">
        <h2>Agency Pending</h2>
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
            <div className="Agency-list">
                {users?.length === 0 && <div className="no-Agency">No Agency Found</div>}
                {users.map((Agency) => (
                    <div key={Agency.id} className="Agency-item" >
                        <div onClick={() => navigate(`/profile/${Agency.id}`)}>
                            <span className="Agency-name">{Agency.name}   |   </span> 
                            <span className="Agency-small">{Agency.role}</span>
                        </div>
                        <button className="reject-btn" onClick={() => reject(Agency.id)}>reject</button>
                        <button className="accept-btn" onClick={() => accept(Agency.id)}>accept</button>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default UsersPending;
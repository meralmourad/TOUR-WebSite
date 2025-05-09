import axios from "axios";
import { useState } from "react";
import { setUser } from "../../../Store/Slices/UserSlice";
import { useDispatch } from "react-redux";

const API_URL = process.env.REACT_APP_API_URL;

function EditAgency({ user, setEdit }) {
    const dispatch = useDispatch();
    const [error, setError] = useState(null);
    const [name, setName] = useState(user.name);
    const [address, setAddress] = useState(user.address);
    const [phoneNumber, setPhoneNumber] = useState(user.phoneNumber);

    const updateUser = async () => {
        const { token } = JSON.parse(localStorage.getItem("Token"));
        setError(null);
        // console.log(`${API_URL}/User/${user.id}`);
        // console.log(token);
        try {
            const response = await axios.put(`${API_URL}/User/${user.id}`,
                {
                    name,
                    phoneNumber,
                    address
                },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    }
                }
            );
            const new_data = {user, token};
            new_data.name = name;
            new_data.phoneNumber = phoneNumber;
            new_data.address = address;
            dispatch(setUser(new_data));
            setEdit(false);
        } catch (error) {
            setError(error.response.data);
            console.error("Error fetching profile:", error);
        }
    }

    return (
        <div className="agency-info">
            {error && <h1 className="error-message">{error}</h1>}
            <div className="info-item">
                <label className="info-label">Name:</label>
                <input
                    className="info-input"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                />
            </div>
            <div className="info-item">
                <label className="info-label">E-mail:</label>
                <input
                    className="info-input"
                    value={user.email}
                    disabled
                    />
            </div>
            <div className="info-item">
                <label className="info-label">Phone:</label>
                <input
                    className="info-input"
                    value={phoneNumber}
                    onChange={(e) => setPhoneNumber(e.target.value)}
                />
            </div>
            <div className="info-item">
                <label className="info-label">Country:</label>
                <input
                    className="info-input"
                    value={address}
                    onChange={(e) => setAddress(e.target.value)}
                />
            </div>

            <div className="button-group">
                <button
                    className="discard-button styled-button"
                    onClick={() => setEdit(false)}
                >
                    Discard
                </button>
                <button
                    className="confirm-button styled-button"
                    onClick={updateUser}
                >
                    Confirm
                </button>
            </div>
        </div>
    );
}

export default EditAgency;

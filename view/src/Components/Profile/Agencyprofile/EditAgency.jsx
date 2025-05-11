import { useState } from "react";
import { setUser } from "../../../Store/Slices/UserSlice";
import { useDispatch } from "react-redux";
import { updateUser } from "../../../service/UserService";

function EditAgency({ user, setEdit }) {

    const dispatch = useDispatch();
    const [error, setError] = useState(null);
    const [name, setName] = useState(user.name);
    const [address, setAddress] = useState(user.address);
    const [phoneNumber, setPhoneNumber] = useState(user.phoneNumber);

    const updateUserForm = async () => {
        const { token } = JSON.parse(localStorage.getItem("Token"));
        setError(null);
        // console.log(`${API_URL}/User/${user.id}`);
        // console.log(token);
        try {
            await updateUser(user.id, {
                name,
                phoneNumber,
                address
            });
            // console.log("User updated successfully");
            const new_data = {user, token};
            new_data.user.name = name;
            new_data.user.phoneNumber = phoneNumber;
            new_data.user.address = address;
            dispatch(setUser(new_data));
            setEdit(false);
        } catch (error) {
            setError(error.response.data.errors);
            console.error("Error fetching profile:", error);
        }
    }

    return (
        <div className="agency-info">
            <div className="info-item">
                <label className="info-label">Name:</label>
                <input
                    className="info-input"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                />

                {error?.Name && <span className="error-message">{error.Name[0]}</span>}
            </div>
            <div className="info-item">
                <label className="info-label">E-mail:</label>
                <input
                    className="info-input"
                    value={user.email}
                    disabled
                    />
                {error?.Email && <span className="error-message">{error.Email[0]}</span>}
            </div>
            <div className="info-item">
                <label className="info-label">Phone:</label>
                <input
                    className="info-input"
                    value={phoneNumber}
                    onChange={(e) => setPhoneNumber(e.target.value)}
                />
                {error?.PhoneNumber && <span className="error-message">{error.PhoneNumber[0]}</span>}
            </div>
            <div className="info-item">
                <label className="info-label">Country:</label>
                <input
                    className="info-input"
                    value={address}
                    onChange={(e) => setAddress(e.target.value)}
                />
                {error?.Address && <span className="error-message">{error.Address[0]}</span>}
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
                    onClick={updateUserForm}
                >
                    Confirm
                </button>
            </div>
        </div>
    );
}

export default EditAgency;

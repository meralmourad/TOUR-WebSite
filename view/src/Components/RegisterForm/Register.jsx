import './Register.scss';
import { useState } from 'react';
import axios from 'axios';
import { Link, useNavigate } from 'react-router-dom';
const API_URL = process.env.REACT_APP_API_URL;

function RegisterForm() {
    const [apiError, setA5piError] = useState('');
    const [passwordError, setPasswordError] = useState('');
    const [confirmError, setConfirmError] = useState('');
    const navigate = useNavigate();

    const [Data, setData] = useState({
        name: '',
        email: '',
        password: '',
        confirmPassword: '',
        phone: '',
        country: '',
        role: '',
    });

    const OnChangeHandler = (event) => {
        const { name, value } = event.target;

        setData((Data) => ({
            ...Data,
            [name]: value,
        }));
    };

    const SubmitHandler = async (event) => {
        event.preventDefault();

        setA5piError('');
        setPasswordError('');
        setConfirmError('');

        let error = false;
        if (Data.password.length < 8) {
            setPasswordError('Password must be Atleast 8 chars');
            error = true;
        }

        if (Data.password !== Data.confirmPassword) {
            setConfirmError("Password Doesn't Match!");
            error = true;
        }
        if (error) {
            return;
        }

        try {
            const response = await axios.post(`${API_URL}/User/signup`, {
                fullName: Data.name,
                email: Data.email,
                password: Data.password,
                confirmPassword: Data.confirmPassword,
                phoneNumber: Data.phone,
                role: Data.role,
                address: Data.country,
            });

            console.log('aregato');
        } 
        
        catch (error) {
            console.error('Fetch error:', error);
            setA5piError(error.response.data)
        }

    };

    return (
        <>
            <title>RegisterForm</title>
            <h1>Register</h1>
            <div className="Register-container">
                <form onSubmit={SubmitHandler}>
                    <div className="input-group">
                        {apiError && (
                            <p style={{ color: 'white' }}>{apiError}</p>
                        )}
                        <input
                            placeholder="Name"
                            type="text"
                            name="name"
                            id="name"
                            value={Data.name}
                            onChange={OnChangeHandler}
                            required
                        />
                        <input
                            placeholder="E-mail"
                            type="email"
                            id="email"
                            name="email"
                            value={Data.email}
                            onChange={OnChangeHandler}
                            required
                        />
                        {passwordError && (
                            <p style={{ color: 'white' }}>{passwordError}</p>
                        )}
                        <input
                            placeholder="Password"
                            type="password"
                            id="password"
                            name="password"
                            value={Data.password}
                            onChange={OnChangeHandler}
                            required
                        />
                        {confirmError === "Password Doesn't Match!" && (
                            <p style={{ color: 'white' }}>{confirmError}</p>
                        )}
                        <input
                            placeholder="ConfirmPassword"
                            type="password"
                            id="confirmPassword"
                            name="confirmPassword"
                            value={Data.confirmPassword}
                            onChange={OnChangeHandler}
                            required
                        />
                        <input
                            placeholder="Phone"
                            type="text"
                            id="phone"
                            name="phone"
                            value={Data.phone}
                            onChange={OnChangeHandler}
                            required
                        />
                        <input
                            placeholder="Country"
                            type="text"
                            id="country"
                            name="country"
                            value={Data.country}
                            onChange={OnChangeHandler}
                            required
                        />
                        <div className="custom-select">
                            <select
                                className=""
                                id="role"
                                name="role"
                                value={Data.role}
                                onChange={OnChangeHandler}
                                required
                            >
                                <option value="" disabled>
                                    Register As
                                </option>
                                <option value="Tourist">Tourist</option>
                                <option value="Agency">Agency</option>
                            </select>
                        </div>
                        <div className="button-group">
                            <button type="button" onClick={()=>navigate('/login')}>BACK</button>
                            <button type="submit" onClick={()=>navigate('/login')} >SIGN UP</button>
                        </div>
                    </div>
                </form>
            </div>
        </>
    );
}

export default RegisterForm;
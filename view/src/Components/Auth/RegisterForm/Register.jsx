import './Register.scss';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { register } from '../../../service/AuthService';

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
            await register(Data);
            console.log('aregato');
            navigate('/login');
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
                            <button type="submit">SIGN UP</button>
                        </div>
                    </div>
                </form>
            </div>
        </>
    );
}

export default RegisterForm;
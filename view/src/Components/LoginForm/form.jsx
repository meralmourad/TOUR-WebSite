import './form.scss';
import { useState } from "react";
import axios from 'axios';
import { useSelector, useDispatch } from 'react-redux';
import { setUser } from '../../Store/Slices/UserSlice';
import { Link } from 'react-router-dom';

function LoginForm() {
  // const user = useSelector((state) => state.user);
  const dispatch = useDispatch();

  const [error, setError] = useState('');

  const [Data, setData] = useState({
    email: "",
    password: ""
  });
    
  const OnChangeHandler = (event) => {
    const { name, value } = event.target;

    setData((Data) => ({
      ...Data,
      [name]: value
    }));
  };
    
  const SubmitHandler = async (event) => {
    event.preventDefault();
    
    setError('');

    if (Data.email === "" || Data.password === "") {
      setError("Please fill in all fields!");
      return;
    }

    try {
      const response = await axios.post("http://localhost:5129/api/User/login", {
          email: Data.email,
          password: Data.password
      });

      dispatch(setUser(response.data));

    } catch (error) {
      setError("invalid username or password!");
    }
  };
  
  return (
    <>
      <title>LoginForm</title>
      <h1>LOGIN</h1>
      <div className="login-container">
        <form onSubmit={SubmitHandler}>
          <div className="input-group">
            {error && <p style={{ color: 'white' }}>{error}</p>}
            <input 
              placeholder="E-mail" 
              type="email" 
              id="email" 
              name="email" 
              value={Data.email}
              onChange={OnChangeHandler}
              required
            />
            <input 
              placeholder="Password" 
              type="password" 
              id="password" 
              name="password"  
              value={Data.password} 
              onChange={OnChangeHandler} 
              required
            />
            <div className='button-group'>
              <button type="button" className="btn">
                <Link to="/signup" style={{textDecoration: "none"}} className="btn">SIGN UP</Link>
              </button>
              <button type="submit" className="btn">LOGIN</button>
            </div>
          </div>
        </form>
      </div>
    </>
  );
}

export default LoginForm;
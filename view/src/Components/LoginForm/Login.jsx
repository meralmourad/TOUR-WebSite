import './form.scss';
import { useState } from "react";
import axios from 'axios';
import { useSelector, useDispatch } from 'react-redux';
import { setUser } from '../../Store/Slices/UserSlice';
import { Link , useNavigate} from 'react-router-dom';

const API_URL = process.env.REACT_APP_API_URL;

function LoginForm() {
  // const user = useSelector((state) => state.user);
  const dispatch = useDispatch();
  const navigate = useNavigate();

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
      const response = await axios.post(`${API_URL}/User/login`, {
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
              {error && <><p className='error'>{error}</p>
              <p className='error'>signup if you don't have an account </p>
              </>}
            <div className='button-group'>
              <button type="button" onClick={()=>navigate('/signup')}>SIGN UP</button>
              <button type="submit" onClick={()=>navigate('/home')}>LOGIN</button>
            </div>
          </div>
        </form>
      </div>
    </>
  );
}

export default LoginForm;
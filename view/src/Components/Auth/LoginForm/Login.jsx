import './form.scss';
import { useState } from "react";
import { useDispatch } from 'react-redux';
import { setUser } from '../.././../Store/Slices/UserSlice';
import { useNavigate} from 'react-router-dom';
import { login } from '../../../service/AuthService';

function LoginForm() {
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
      const data = await login(Data);

      dispatch(setUser(data));
      localStorage.setItem('Token', JSON.stringify({token: data.token, id: data.user.id}));
      navigate('/home');

    } catch (error) {
      console.error(error);
      setError(error.response?.data);
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
              {error && <p className='error'>{error}</p>}
            <div className='button-group'>
              <button type="button" onClick={() => navigate('/signup')}>SIGN UP</button>
              <button type="submit">LOGIN</button>
            </div>
          </div>
        </form>
      </div>
    </>
  );
}

export default LoginForm;
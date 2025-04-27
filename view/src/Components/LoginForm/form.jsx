import './form.scss';
import { useState } from "react";
import axios from 'axios';

function LoginForm() {

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

    try {
      const response = await axios.post("http://localhost:5129/api/User/login", {
      headers: {
        email: Data.email,
        password: Data.password 
      }
      });
  
      if (response.status == 200) {
      return (<><h1>hello</h1></>);
      } else {
      setError("Please SignUp first!");
      }

    } catch (error) {
      setError("Please SignUp first!");
      console.error("Fetch error:", error);
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
              <button type="button" className="btn">SIGN UP</button>
              <button type="submit" className="btn">LOGIN</button>
            </div>
          </div>
        </form>
      </div>
    </>
  );
}

export default LoginForm;
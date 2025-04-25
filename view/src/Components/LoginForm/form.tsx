import './form.scss';
import RegisterForm from '../RegisterForm/Register'
import { useState, ChangeEvent, FormEvent } from "react";

interface FormData {
    email: string ;
    password: string; 
}


function LoginForm() {

    const [Data, setData] = useState<FormData>({
        email: "",
        password: ""
      });
      
    const OnChangeHandler = (event: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;

        setData((Data) => ({
            ...Data,
            [name]: value
        }));
    
    };
      
    // const SubmitHandler = async (event : FormEvent<HTMLFormElement>) => {
    //     event.preventDefault();
    
    //     try {
    //       const response = await fetch("https:://Tourist-App/api/person/login", {
    //         method: "POST",
    //         body: JSON.stringify(Data)
    //       });
    
    //       const data = await response.json();
    
    //       if (response.ok) {
    //         sessionStorage.setItem("token", data.token);
    //       } 
    //       else {
                // return (
                // 
                // <RegisterForm/>
                // 
                // ) ;
    //       }
    //     }
    //      catch (error) {
    //       console.error("Fetch error:", error);
    //     }

    //   };
    
    return(
        <>
        console.log(Data);
        
            <title>LoginForm</title>
            <h1>LOGIN</h1>
            <title>LoginForm</title>
            <div className="login-container">
                <form>
                    <div className="input-group">
                        <input placeholder ="E-mail" type="email" id="email" name="email" value= {Data.email}
                            onChange={OnChangeHandler}
                        required/>
                        <input placeholder = "Password"type="password" id="password" name="password"  value={Data.password} 
                        onChange={ OnChangeHandler} required/>
                        <div className='button-group'>
                            <button type="button" className="btn">SIGN UP</button>
                            <button type="submit" className="btn">LOGIN</button>
                        </div>
                    </div>
                </form>
            </div>
        </>
    )
}

export default LoginForm ;
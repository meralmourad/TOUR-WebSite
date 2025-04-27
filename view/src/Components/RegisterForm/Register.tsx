
import './Register.scss'
import { useState, ChangeEvent , FormEvent } from "react";
import axios from 'axios';
import { Link , useNavigate } from 'react-router-dom';



interface FormData {
    name:string ;
    email: string ;
    password: string;
    confirmPassword : string ;
    phone: string ;
    country : string ;
    role: string ; 
}

function RegisterForm() {
    
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const [Data, setData] = useState<FormData>({
        name: "" ,
        email: "" ,
        password: "",
        confirmPassword : "" ,
        phone: "" ,
        country : "" ,
        role: "" , 
      });
      
    const OnChangeHandler = (event : ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = event.target;

        setData((Data) => ({
            ...Data,
            [name]: value
        }));
    
    };
      
    const SubmitHandler = async (event : FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if(Data.password.length < 8 ){
            setError("Password must be Atleast 8 chars");
        }
        
        if(Data.password != Data.confirmPassword){
            setError("Password Doesn't Match!");
        }
        
        try {
            const response = await axios.post("http://localhost:5129/api/User/signup", {
                fullName: Data.name,
                email: Data.email,
                password: Data.password,
                confirmPassword: Data.confirmPassword,
                phoneNumber: Data.phone,
                role: Data.role,
                address: Data.country,
            })
            
          if (response.status == 200) {
           console.log("aregato");
           navigate('/login');
           
          } 
            else {
                console.log(response.data);
            }
        }
         catch (error) {
          console.error("Fetch error:", error);
        }

      };
    
    return(
        <>
            <title>RegisterForm</title>
            <h1>Register</h1>
            <title>RegisterForm</title>
            <div className="Register-container">
                <form onSubmit={SubmitHandler}>
                    <div className="input-group">
                        <input placeholder = "Name" type = "text" name="name" id="name" value = {Data.name} onChange={OnChangeHandler} required />
                        <input placeholder ="E-mail" type="email" id="email" name="email" value= {Data.email} onChange={OnChangeHandler} required/>
                        {error == "Password must be Atleast 8 chars" && <p style={{ color: 'white' }}>{error}</p>}
                        <input placeholder = "Password"type="password" id="password" name="password"  value={Data.password} onChange={ OnChangeHandler} required/>
                        {error == "Password Doesn't Match!"  && <p style={{ color: 'white' }}>{error}</p>}
                        <input placeholder = "ConfirmPassword"type="password" id="confirmPassword" name="confirmPassword"  value={Data.confirmPassword} onChange={ OnChangeHandler} required/>
                        <input placeholder = "Phone"type="text" id="phone" name="phone"  value={Data.phone} onChange={ OnChangeHandler} required/>
                        <input placeholder = "Country"type="text" id="country" name="country"  value={Data.country} onChange={ OnChangeHandler} required/>
                        <div className="custom-select">
                            <select className='' id="role" name="role" value={Data.role} onChange={OnChangeHandler} required>
                                <option value="" disabled>Register As </option>
                                <option value="Tourist">Tourist</option>
                                <option value="Agency">Agency</option>
                            </select>
                        </div>
                        <div className='button-group'>
                            <button type="button"> <Link to ='/login' className="btn"> BACK </Link></button>
                            <button type="submit">SIGN UP</button>
                        </div>
                    </div>
                </form>
            </div>
        </>
    )
}

export default RegisterForm ;
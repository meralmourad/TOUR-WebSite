import './Register.scss'
import { useState, ChangeEvent, FormEvent } from "react";

interface FormData {
    name:string ;
    email: string ;
    password: string;
    phone: string ;
    country : string ;
    role: string ; 
}


function RegisterForm() {

    const [Data, setData] = useState<FormData>({
        name: "" ,
        email: "" ,
        password: "",
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
                // return (<h1>BadReqesut<h1/>)
    //       }
    //     }
    //      catch (error) {
    //       console.error("Fetch error:", error);
    //     }

    //   };
    
    return(
        <>
        { console.log(Data)}
        
            <title>RegisterForm</title>
            <h1>Register</h1>
            <title>RegisterForm</title>
            <div className="Register-container">
                <form>
                    <div className="input-group">
                        <input placeholder = "Name" type = "text" name="name" id="name" value = {Data.name} onChange={OnChangeHandler} required />
                        <input placeholder ="E-mail" type="email" id="email" name="email" value= {Data.email} onChange={OnChangeHandler} required/>
                        <input placeholder = "Password"type="password" id="password" name="password"  value={Data.password} onChange={ OnChangeHandler} required/>
                        <input placeholder = "Phone"type="text" id="phone" name="phone"  value={Data.phone} onChange={ OnChangeHandler} required/>
                        <input placeholder = "Country"type="text" id="country" name="country"  value={Data.country} onChange={ OnChangeHandler} required/>
                        <div className="custom-select">
                            <select className='' id="role" name="role" value={Data.role} onChange={OnChangeHandler} required>
                                <option value="" disabled>Register As </option>
                                <option value="student">Tourist</option>
                                <option value="teacher">Agency</option>
                            </select>
                        </div>
                        <div className='button-group'>
                            <button type="button" className="btn">BACK</button>
                            <button type="submit" className="btn">SIGN UP</button>
                        </div>
                    </div>
                </form>
            </div>
        </>
    )
}

export default RegisterForm ;
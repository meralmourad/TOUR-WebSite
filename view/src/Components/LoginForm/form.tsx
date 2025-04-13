import { Fragment } from 'react/jsx-runtime';
import './form.scss'

function Form(){

    return(
        <>
            <title>LoginForm</title>

            <div className="login-container">
                <h1>LOGIN</h1>
                <form>
                    <div className="input-group">
                        <label htmlFor="email">E-MAIL</label>
                        <input type="email" id="email" name="email" required/>
                        <label htmlFor="password">PASSWORD</label>
                        <input type="password" id="password" name="password" required/>
                        <div className='btns'>
                            <button type="button" className="btn">SIGN UP</button>
                            <button type="submit" className="btn">LOGIN</button>
                        </div>
                    </div>
                </form>
            </div>
        </>
    )
}

export default Form ;
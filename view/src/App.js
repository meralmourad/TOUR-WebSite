import { BrowserRouter, Route, Routes } from "react-router-dom";
import { useSelector } from "react-redux";
import UsersList from "./Components/UsersList/UsersList";
import NavBar from "./Components/NavBar/NavBar.jsx";
import LoginForm from "./Components/LoginForm/Login";
import RegisterForm from "./Components/RegisterForm/Register";
import { useEffect } from "react";

function App() {
  const { user, isLoggedIn } = useSelector((store) => store.info);

  useEffect(() => console.log(user), [user]);
  
  useEffect(() => {
    const token = localStorage.getItem("Token");
    if (token) {
      
    }
  }, []);

  return (
    <BrowserRouter>
      {true && <NavBar />}
      <Routes>
        <Route path="/" element={<h1> Home Page </h1>} />
        <Route path="/login" element={ <LoginForm /> } />
        <Route path="/signup" element={ <RegisterForm /> } />
        <Route path="/userslist" element={ <UsersList /> } />
        <Route path="*" element={ <h1> Page not found </h1> } />
      </Routes>
    </BrowserRouter>
  );
}

export default App;

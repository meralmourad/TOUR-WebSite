import { BrowserRouter, Route, Routes } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import UsersList from "./Components/UsersList/UsersList";
import NavBar from "./Components/NavBar/NavBar.jsx";
import LoginForm from "./Components/LoginForm/Login";
import RegisterForm from "./Components/RegisterForm/Register";
import { useEffect } from "react";
import { fetchUser } from "./Store/Slices/UserSlice.js";

function App() {
  const { user, isLoggedIn } = useSelector((store) => store.info);
  const usedispatch = useDispatch();

  useEffect(() => console.log(user), [user]);

  useEffect(() => {
    const ls = JSON.parse(localStorage.getItem("Token"));
    if (ls?.token) {
      usedispatch(fetchUser(ls));
    }
  }, []);

  return (
    <BrowserRouter>
      {isLoggedIn && <NavBar />}
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

import { BrowserRouter, Route, Routes } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import UsersList from "./Components/UsersList/UsersList";
import NavBar from "./Components/NavBar/NavBar.jsx";
import LoginForm from "./Components/LoginForm/Login";
import RegisterForm from "./Components/RegisterForm/Register";
import { useEffect } from "react";
import { fetchUser } from "./Store/Slices/UserSlice.js";
import ChatApp from "./Components/Chat/Chat.jsx";
import Home from "./Components/Home/Home.jsx";
import Rate from "./Components/Rate/Rate.jsx";
import WelcomePage from "./Components/WelcomePage/WelcomePage";
import AgencyProfile from "./Components/Agencyprofile/Agencyprofile.jsx";
import TripName  from "./Components/TripName/TripName.jsx";

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
        <Route path="/" element={ <Home/> } />
        <Route path="/login" element={ <LoginForm /> } />
        <Route path="/signup" element={ <RegisterForm /> } />
        <Route path="/AgencyProfile" element={ <AgencyProfile /> } />
        <Route path="/TripName" element={ <TripName /> } />
        <Route path="/userslist" element={ <UsersList /> } />
        <Route path="*" element={ <h1> Page not found </h1> } />
        <Route path="/welcome" element={<WelcomePage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;

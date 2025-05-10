import { BrowserRouter, Route, Routes } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import UsersList from "./Components/UsersList/UsersList";
import NavBar from "./Components/NavBar/NavBar.jsx";
import LoginForm from "./Components/Auth/LoginForm/Login.jsx";
import RegisterForm from "./Components/Auth/RegisterForm/Register.jsx";
import { useEffect } from "react";
import { fetchUser } from "./Store/Slices/UserSlice.js";
import ChatApp from "./Components/Chat/Chat.jsx";
import Home from "./Components/Home/Home.jsx";
import Rate from "./Components/Rate/Rate.jsx";
import WelcomePage from "./Components/WelcomePage/WelcomePage";
import Profile from "./Components/Profile/Profile.jsx";
import Trip  from "./Components/Trip/Trip.jsx";
import BookingPending from "./Components/BookingPending/BookingPending.jsx";
import Filter from "./Components/Trip/Filter/Filter.jsx"

import Button from "./Components/button/button.jsx";
import AddTrip from "./Components/AddTrip/AddTrip.jsx";
import NotificationPage from "./Components/NotificationPage/NotificationPage.jsx";
function App() {
  const { user } = useSelector((store) => store.info);
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
      <NavBar />
      <Routes>
        <Route path="/" element={ <WelcomePage/> } />
        <Route path="/home" element={ <Home /> } /> 
        <Route path="/Profile" element={ <Profile/> } />
        <Route path="/login" element={ <LoginForm /> } />
        <Route path="/signup" element={ <RegisterForm /> } />
        <Route path="/AgencyProfile" element={ <AgencyProfile /> } />
        <Route path="/TripName" element={ <TripName /> } />
        <Route path="/BookingPending" element={ <BookingPending /> } />
        <Route path="/userslist" element={ <UsersList /> } />
        <Route path="/welcome" element={<WelcomePage />} />
        <Route path="*" element={ <h1> Page not found </h1> } />
        <Route path="/filter" element={ <Filter /> } />
      </Routes>
    </BrowserRouter>
  );
}

export default App;

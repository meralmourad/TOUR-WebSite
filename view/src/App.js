import { BrowserRouter, Route, Routes } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import UsersList from "./Components/UsersList/UsersList";
import NavBar from "./Components/NavBar/NavBar.jsx";
import LoginForm from "./Components/Auth/LoginForm/Login.jsx";
import RegisterForm from "./Components/Auth/RegisterForm/Register.jsx";
import { useEffect, useState } from "react";
import { fetchUser } from "./Store/Slices/UserSlice.js";
import TripsList from "./Components/TripsList/TripsList.jsx";
import Trip from "./Components/TripsList/Trip/Trip";
import WelcomePage from "./Components/WelcomePage/WelcomePage";
import Profile from "./Components/Profile/Profile.jsx";
import BookingPending from "./Components/BookingPending/BookingPending.jsx";
// import AddTrip from "./Components/AddTrip/AddTrip.jsx";

function App() {
  const { user } = useSelector((store) => store.info);
  const [showChat, setShowChat] = useState(false);
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
      <NavBar setShowChat={setShowChat} />

      <div onClick={() => setShowChat(false)}>
        <Routes>
          <Route path="/" element={<WelcomePage />} />
          <Route path="/home/:id?" element={<TripsList />} />
          <Route path="/Profile/:id" element={<Profile showChat={showChat} setShowChat={setShowChat} />} />
          <Route path="/login" element={<LoginForm />} />
          <Route path="/signup" element={<RegisterForm />} />
          <Route path="/Trip/:id" element={<Trip />} />
          <Route path="/BookingPending/:id" element={<BookingPending />} />
          <Route path="/userslist" element={<UsersList />} />
          <Route path="*" element={<h1> Page not found </h1>} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;

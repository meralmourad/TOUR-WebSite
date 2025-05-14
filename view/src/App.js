import { BrowserRouter, Route, Routes } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import UsersList from "./Components/UsersList/UsersList";
import NavBar from "./Components/NavBar/NavBar.jsx";
import LoginForm from "./Components/Auth/LoginForm/Login.jsx";
import RegisterForm from "./Components/Auth/RegisterForm/Register.jsx";
import { useEffect } from "react";
import { fetchUser } from "./Store/Slices/UserSlice.js";
import TripsList from "./Components/TripsList/TripsList.jsx";
import Trip from "./Components/TripsList/Trip/Trip";
import EditTrip from "./Components/TripsList/Trip/EditTrip/EditTrip.jsx";
import WelcomePage from "./Components/WelcomePage/WelcomePage";
import Profile from "./Components/Profile/Profile.jsx";
import BookingPending from "./Components/BookingPending/BookingPending.jsx";
import UsersPending from "./Components/UsersPending/UsersPending.jsx";
import TripsPending from "./Components/TripsPending/TripsPending.jsx";
import Chat from "./Components/Chat/Chat.jsx";
import { clearChat } from "./Store/Slices/ChatSlice.js";
import BookingList from "./Components/BookingList/BookingList.jsx";
import Report from "./Components/Report/Report.jsx";
// import AddTrip from "./Components/AddTrip/AddTrip.jsx";

function App() {
  const { user } = useSelector((store) => store.info);
  const { showChat } = useSelector((store) => store.chat);
  const dispatch = useDispatch();

  useEffect(() => console.log(user), [user]);

  useEffect(() => {
    const ls = JSON.parse(localStorage.getItem("Token"));
    if (ls?.token) {
      dispatch(fetchUser(ls));
    }
  }, [dispatch]);

  return (
    <BrowserRouter>
      <NavBar />

      {showChat && <Chat />}

      <div onClick={() => dispatch(clearChat())}>
        <Routes>
          <Route path="/" element={<WelcomePage />} />
          <Route path="/home/:id?" element={<TripsList />} />
          <Route path="/Profile/:id" element={<Profile />} />
          <Route path="/login" element={<LoginForm />} />
          <Route path="/signup" element={<RegisterForm />} />
          <Route path="/Report/:id" element={<Report />} />
          <Route path="/Trip/:id" element={<Trip />} />
          <Route path="/EditTrip/:id" element={<EditTrip />} />
          <Route path="/BookingPending/:id" element={<BookingPending />} />
          <Route path="/UsersPending" element={<UsersPending />} />
          <Route path="/TripsPending" element={<TripsPending />} />
          <Route path="/userslist" element={<UsersList />} />
          <Route path="/BookingList/:id" element={<BookingList />} />
          <Route path="*" element={<h1> Page not found </h1>} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;

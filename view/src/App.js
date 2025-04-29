import { BrowserRouter, Route, Routes } from "react-router-dom";
import { useSelector } from "react-redux";
import UsersList from "./Components/UsersList/UsersList";
import NavBar from "./Components/NavBar/NavBar.jsx";
import LoginForm from "./Components/LoginForm/Login";
import RegisterForm from "./Components/RegisterForm/Register";
import ChatApp from "./Components/Chat/Chat.jsx";
import Home from "./Components/Home/Home.jsx";
import Rate from "./Components/Rate/Rate.jsx";

function App() {
  const { user, isLoggedIn } = useSelector((store) => store.info);
  console.log(user);
  return (
    <BrowserRouter>
      {true && <NavBar />}
      <Routes>
        <Route path="/" element={<Home/>} />
        <Route path="/login" element={ <LoginForm /> } />
        <Route path="/signup" element={ <RegisterForm /> } />
        <Route path="/userslist" element={ <UsersList /> } />
        <Route path="*" element={ <h1> Page not found </h1> } />
      </Routes>
    </BrowserRouter>
  );
}

export default App;

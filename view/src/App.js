import { BrowserRouter, Route, Routes } from "react-router-dom";
import { useSelector } from "react-redux";
import UsersList from "./Components/UsersList/UsersList";
import NavBar from "./Components/NavBar/NavBar.jsx";
import LoginForm from "./Components/LoginForm/Login";
import RegisterForm from "./Components/RegisterForm/Register";
import WelcomePage from "./Components/WelcomePage/WelcomePage";



function App() {
  const { user, isLoggedIn } = useSelector((store) => store.info);
  console.log(user);
  return (
    <BrowserRouter>
      {isLoggedIn && <NavBar />}
      <Routes>
        <Route path="/" element={<h1> Home Page </h1>} />
        <Route path="/login" element={ <LoginForm /> } />
        <Route path="/signup" element={ <RegisterForm /> } />
        <Route path="/userslist" element={ <UsersList /> } />
        <Route path="*" element={ <h1> Page not found </h1> } />
        <Route path="/welcome" element={<WelcomePage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;

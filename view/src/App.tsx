import { BrowserRouter, Route, Routes } from "react-router-dom";
import LoginForm from "./Components/LoginForm/form.jsx";
import RegisterForm from "./Components/RegisterForm/Register";
import { useSelector } from "react-redux";
import { store } from "./Store/Store.js";
import UsersList from "./Components/UsersList/UsersList";
import NavBar from "./Components/NavBar/NavBar";

function App() {
  const user = useSelector((store) => store.user);
  console.log(user);
  return (
    <BrowserRouter>
      <NavBar />
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

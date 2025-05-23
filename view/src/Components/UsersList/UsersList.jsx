import React, { useEffect, useState } from "react";
import "./UsersList.scss";
import { useNavigate } from "react-router-dom";
import { SearchUsers } from "../../service/UserService";

const numberOfUsersPerPage = 9;

const UsersList = () => {
  const navigate = useNavigate();
  const [Admin, setAdmin] = useState(false);
  const [Agency, setAgency] = useState(false);
  const [Tourist, setTourist] = useState(false);
  const [pageNumber, setPageNumber] = useState(1);
  const [numOfUsers, setNumOfUsers] = useState(0);
  const [users, setUsers] = useState([]);

  const numberOfPages = Math.ceil(numOfUsers / numberOfUsersPerPage);
  
  const [searchTerm, setSearchTerm] = useState("");

  const handleSearch = (event) => {
    setSearchTerm(event.target.value);
  };

  useEffect(() => {
    const start = (pageNumber - 1) * numberOfUsersPerPage;

    const fetchUsers = async () => {
      try {
        const { users, totalCount } = !Tourist && !Admin && !Agency? await SearchUsers(start, numberOfUsersPerPage, true, true, true, true, searchTerm): await SearchUsers(start, numberOfUsersPerPage, Tourist, Agency, Admin, true, searchTerm);

        setNumOfUsers(totalCount);
        setUsers(users);
      } catch (error) {
        console.error("Error fetching users:", error);
      }
    };
    fetchUsers();
  }, [Admin, Agency, Tourist, pageNumber, searchTerm]);


  if (numberOfPages !== 0 && (pageNumber > numberOfPages || pageNumber < 1)) {
    setPageNumber(1);
  }

  const FunctionBtn = (i) => {
    if (i < 1 || i > numberOfPages) return <></>;
    return (
      <button
        onClick={() => setPageNumber(i)}
        key={i}
        style={i === pageNumber ? { backgroundColor: "#abdfa4" } : {}}
      >
        {i}
      </button>
    );
  };

  return (
    <>
      <br /> <br />
      <h2 className="users-list-title">List of All Users</h2>
      <div className="users-list-header">
        <div className="search-bar">
          <span role="img" aria-label="search" style={{ marginRight: "10px" }}>
            <img
              src="Icons/search.jpg"
              alt=""
              style={{
                width: "20px",
                height: "20px",
              }}
            ></img>
          </span>
          <input
            type="text"
            style={{
              border: "none",
              outline: "none",
              backgroundColor: "transparent",
              fontSize: "16px",
            }}
            placeholder="Search"
            value={searchTerm}
            onChange={handleSearch}
          />
        </div>
        <button
          className={`add-user-button ${Agency ? "active-user-button" : ""}`}
          onClick={() => setAgency(!Agency)}
        >
          AGENCY
        </button>
        <button
          className={`add-user-button ${Admin ? "active-user-button" : ""}`}
          onClick={() => setAdmin(!Admin)}
        >
          Admin
        </button>
        <button
          className={`add-user-button ${Tourist ? "active-user-button" : ""}`}
          onClick={() => setTourist(!Tourist)}
        >
          Tourist
        </button>
      </div>
      <div className="users-list-container">
        <br />
        {users.length === 0 && <h2 className="users-list-title">No users Found</h2>}
        <div className="users-list">
          {users.length !== 0 && users.map((user) => (
            <div key={user.id} className="user-card" onClick={() => navigate(`/profile/${user.id}`)}>
              <div className="user-image">
                <h5 style={{ margin: 0, fontSize: "16px" }}>{user.name}</h5>
                <p
                  style={{
                    margin: 0,
                    fontSize: "12px",
                    color: "#801c29",
                    fontWeight: "bold",
                  }}
                >
                  {user.role}
                </p>
              </div>
            </div>
          ))}
          <br />
        </div>
      </div>
      <div className="pagination">
        <button onClick={() => setPageNumber(Math.max(1, pageNumber - 1))}>
          &laquo;
        </button>

        {pageNumber - 3 >= 1 && <button>...</button>}

        {FunctionBtn(pageNumber - 2)}
        {FunctionBtn(pageNumber - 1)}
        {FunctionBtn(pageNumber)}
        {FunctionBtn(pageNumber + 1)}
        {FunctionBtn(pageNumber + 2)}

        {pageNumber + 3 <= numberOfPages && <button>...</button>}
        <button
          onClick={() => setPageNumber(Math.min(numberOfPages, pageNumber + 1))}
        >
          &raquo;
        </button>
      </div>
    </>
  );
};

export default UsersList;

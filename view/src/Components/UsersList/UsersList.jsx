import React, { useState } from "react";
import "./UsersList.scss";

const UsersList = () => {
  const [Admin, setAdmin] = useState(false);
  const [Agency, setAgency] = useState(false);
  const [Tourist, setTourist] = useState(false);
  const [pageNumber, setPageNumber] = useState(1);

  const [searchTerm, setSearchTerm] = useState("");

  const handleSearch = (event) => {
    setSearchTerm(event.target.value.toLowerCase());
  };

  const users = [
    { id: 1, name: "omar", role: "AGENCY" },
    { id: 2, name: "ahmed", role: "Admin" },
    { id: 3, name: "feby", role: "AGENCY" },
    { id: 4, name: "fady", role: "Tourist" },
    { id: 5, name: "miral", role: "AGENCY" },
    { id: 6, name: "hany", role: "Tourist" },
    { id: 7, name: "hany", role: "Tourist" },
    { id: 8, name: "sara", role: "Admin" },
    { id: 9, name: "john", role: "Tourist" },
    { id: 10, name: "linda", role: "AGENCY" },
    { id: 11, name: "mike", role: "Admin" },
    { id: 12, name: "jane", role: "Tourist" },
    { id: 13, name: "paul", role: "AGENCY" },
    { id: 14, name: "emma", role: "Admin" },
    { id: 15, name: "noah", role: "Tourist" },
    { id: 16, name: "olivia", role: "AGENCY" },
    { id: 17, name: "liam", role: "Admin" },
    { id: 18, name: "ava", role: "Tourist" },
    { id: 19, name: "elijah", role: "AGENCY" },
    { id: 20, name: "sophia", role: "Admin" },
    { id: 21, name: "william", role: "Tourist" },
    { id: 22, name: "isabella", role: "AGENCY" },
    { id: 23, name: "james", role: "Admin" },
    { id: 24, name: "mia", role: "Tourist" },
    { id: 25, name: "benjamin", role: "AGENCY" },
    { id: 26, name: "amelia", role: "Admin" },
    { id: 27, name: "lucas", role: "Tourist" },
    { id: 27, name: "lucas", role: "Tourist" },
  ];

  const filteredUsers = users.filter((user) => {
    const isAgency = Agency && user.role === "AGENCY";
    const isAdmin = Admin && user.role === "Admin";
    const isTourist = Tourist && user.role === "Tourist";
    const isSearchMatch = user.name.toLowerCase().includes(searchTerm);
    const isRoleMatch =
      isAgency || isAdmin || isTourist || (!Agency && !Admin && !Tourist);
    return isRoleMatch && isSearchMatch;
  });

  const paginatedUsers = filteredUsers.slice(
    (pageNumber - 1) * 9,
    pageNumber * 9
  );

  const numberOfPages = Math.ceil(filteredUsers.length / 9);

  if (pageNumber > numberOfPages || pageNumber < 1) {
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
        <div className="users-list">
          {paginatedUsers.map((user) => (
            <div key={user.id} className="user-card">
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

        {pageNumber - 3 > 0 && <button>...</button>}

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

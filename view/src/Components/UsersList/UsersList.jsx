import React, { useState } from "react";
import "./UsersList.scss";

const UsersList = () => {
  const [filters, setFilters] = useState([]);

  const users = [
    { name: "omar", role: "AGENCY" },
    { name: "ahmed", role: "Admin" },
    { name: "feby", role: "AGENCY" },
    { name: "fady", role: "Tourist" },
    { name: "miral", role: "AGENCY" },
    { name: "hany", role: "Tourist" },
    { name: "hany", role: "Tourist" },
    { name: "hany", role: "Tourist" },
    { name: "hany", role: "Tourist" },
    { name: "hany", role: "Tourist" },
    { name: "hany", role: "Tourist" },
    { name: "hany", role: "Tourist" },
    { name: "hany", role: "Tourist" },
    { name: "hany", role: "Tourist" },
    { name: "hany", role: "Tourist" },
    { name: "hany", role: "Tourist" },
    { name: "hany", role: "Tourist" },
    { name: "hany", role: "Tourist" },
  ];

  const toggleFilter = (role) => {
    setFilters((prevFilters) =>
      prevFilters.includes(role)
        ? prevFilters.filter((filter) => filter !== role)
        : [...prevFilters, role]
    );
  };

  const filteredUsers =
    filters.length === 0
      ? users
      : users.filter((user) => filters.includes(user.role));

  const [searchTerm, setSearchTerm] = useState("");

  const handleSearch = (event) => {
    setSearchTerm(event.target.value.toLowerCase());
  };

  const finalFilteredUsers = filteredUsers.filter((user) =>
    user.name.toLowerCase().includes(searchTerm)
  );

  return (
    <>
      <br /> <br />
      <h2
        style={{
          marginBottom: "20px",
          fontSize: "32px",
          color: "black",
          textAlign: "center",
        }}
      >
        List of All Users
      </h2>
      <div
        style={{
          display: "flex",
          marginLeft: "20.5%",
          alignItems: "center",
          marginBottom: "20px",
          gap: "10px",
        }}
      >
        <div
          style={{
            display: "flex",
            alignItems: "center",
            border: "1px solid #ccc",
            borderRadius: "25px",
            padding: "10px 15px",
            backgroundColor: "#f9f9f9",
          }}
        >
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
          style={{
            width: "60px",
            height: "50px",
            borderRadius: "50%",
            backgroundColor: filters.includes("AGENCY") ? "#cce5cc" : "#f9f9f9",
            border: "1px solid #ccc",
            cursor: "pointer",
            fontSize: "14px",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
          }}
          onClick={() => toggleFilter("AGENCY")}
        >
          AGENCY
        </button>
        <button
          style={{
            width: "60px",
            height: "50px",
            borderRadius: "50%",
            backgroundColor: filters.includes("Admin") ? "#cce5cc" : "#f9f9f9",
            border: "1px solid #ccc",
            cursor: "pointer",
            fontSize: "14px",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
          }}
          onClick={() => toggleFilter("Admin")}
        >
          Admin
        </button>
        <button
          style={{
            width: "60px",
            height: "50px",
            borderRadius: "50%",
            backgroundColor: filters.includes("Tourist")
              ? "#cce5cc"
              : "#f9f9f9",
            border: "1px solid #ccc",
            cursor: "pointer",
            fontSize: "14px",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
          }}
          onClick={() => toggleFilter("Tourist")}
        >
          Tourist
        </button>
      </div>
      <div
        style={{
          height: "79%",
          width: "65%",
          margin: "10px auto",
          fontFamily: "Arial, sans-serif",
          color: "#333",
          maxWidth: "90%",
          textAlign: "center",
          backgroundColor: "white",
          padding: "20px",
          borderRadius: "50px",
        }}
      >
        <br />
        <div
          style={{
            height: "100%",
            display: "grid",
            gridTemplateColumns: "repeat(3, 1fr)",
            gap: "20px",
            justifyContent: "center",
          }}
        >
          {finalFilteredUsers.map((user, index) => (
            <div
              key={index}
              style={{
                border: "1px solid #ccc",
                borderRadius: "50px",
                textAlign: "center",
                padding: "10px",
                backgroundColor: "#fff",
                fontSize: "16px",
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                justifyContent: "center",
                width: "90%",
                margin: "0 auto",
              }}
            >
              <div
                style={{
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "center",
                  width: "100%",
                  padding: "0 10px",
                }}
              >
                <h5 style={{ margin: 0, fontSize: "16px" }}>{user.name}</h5>
                <p style={{ margin: 0, fontSize: "12px", color: "#801c29", fontWeight: "bold" }}>
                  {user.role}
                </p>
              </div>
            </div>
          ))}
          <br />
        </div>
      </div>
      <div
        style={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          marginTop: "20px",
          gap: "10px",
        }}
      >
        <button
          style={{
            padding: "10px",
            border: "1px solid #ccc",
            borderRadius: "5px",
            cursor: "pointer",
            backgroundColor: "#f9f9f9",
            fontSize: "14px",
          }}
        >
          &laquo;
        </button>
        <button
          style={{
            padding: "10px",
            border: "1px solid #ccc",
            borderRadius: "5px",
            cursor: "pointer",
            backgroundColor: "#abdfa4",
            fontSize: "14px",
          }}
        >
          1
        </button>
        <button
          style={{
            padding: "10px",
            border: "1px solid #ccc",
            borderRadius: "5px",
            cursor: "pointer",
            backgroundColor: "#f9f9f9",
            fontSize: "14px",
          }}
        >
          2
        </button>
        <button
          style={{
            padding: "10px",
            border: "1px solid #ccc",
            borderRadius: "5px",
            cursor: "pointer",
            backgroundColor: "#f9f9f9",
            fontSize: "14px",
          }}
        >
          3
        </button>
        <button
          style={{
            padding: "10px",
            border: "1px solid #ccc",
            borderRadius: "5px",
            cursor: "pointer",
            backgroundColor: "#f9f9f9",
            fontSize: "14px",
          }}
        >
          ...
        </button>
        <button
          style={{
            padding: "10px",
            border: "1px solid #ccc",
            borderRadius: "5px",
            cursor: "pointer",
            backgroundColor: "#f9f9f9",
            fontSize: "14px",
          }}
        >
          &raquo;
        </button>
      </div>
    </>
  );
};

export default UsersList;

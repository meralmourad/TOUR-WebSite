import React, { useState } from "react";

const UsersList = () => {
    const [filters, setFilters] = useState([]);

    const users = [
        { name: "omar", role: "AGENCY" },
        { name: "ahmed", role: "Admin" },
        { name: "feby", role: "AGENCY" },
        { name: "fady", role: "Tourist" },
        { name: "miral", role: "AGENCY" },
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

    return (
        <div style={{ margin: "20px" }}>
            <h2 style={{ textAlign: "center", marginBottom: "20px" }}>List of All Users</h2>
            <div style={{ display: "flex", justifyContent: "center", marginBottom: "20px" }}>
                <input
                    type="text"
                    style={{
                        width: "25%",
                        marginRight: "10px",
                        padding: "10px",
                        border: "1px solid #ccc",
                        borderRadius: "5px",
                    }}
                    placeholder="Search"
                />
                <button
                    style={{
                        marginRight: "10px",
                        borderRadius: "50%",
                        padding: "10px 20px",
                        backgroundColor: filters.includes("AGENCY") ? "#ddd" : "transparent",
                        border: "1px solid #ccc",
                        cursor: "pointer",
                    }}
                    onClick={() => toggleFilter("AGENCY")}
                >
                    AGENCY
                </button>
                <button
                    style={{
                        marginRight: "10px",
                        borderRadius: "50%",
                        padding: "10px 20px",
                        backgroundColor: filters.includes("Admin") ? "#ddd" : "transparent",
                        border: "1px solid #ccc",
                        cursor: "pointer",
                    }}
                    onClick={() => toggleFilter("Admin")}
                >
                    Admin
                </button>
                <button
                    style={{
                        borderRadius: "50%",
                        padding: "10px 20px",
                        backgroundColor: filters.includes("Tourist") ? "#ddd" : "transparent",
                        border: "1px solid #ccc",
                        cursor: "pointer",
                    }}
                    onClick={() => toggleFilter("Tourist")}
                >
                    Tourist
                </button>
            </div>
            <div style={{ display: "flex", flexWrap: "wrap", gap: "20px" }}>
                {filteredUsers.map((user, index) => (
                    <div
                        key={index}
                        style={{
                            flex: "1 1 calc(33.333% - 20px)",
                            marginBottom: "20px",
                            border: "1px solid #ccc",
                            borderRadius: "20px",
                            textAlign: "center",
                            padding: "20px",
                        }}
                    >
                        <h5>{user.name}</h5>
                        <p>{user.role}</p>
                    </div>
                ))}
            </div>
            <div style={{ display: "flex", justifyContent: "center", marginTop: "20px" }}>
                <button style={{ margin: "0 5px", padding: "10px", cursor: "pointer" }}>
                    &laquo;
                </button>
                <button
                    style={{
                        margin: "0 5px",
                        padding: "10px",
                        backgroundColor: "#ddd",
                        cursor: "pointer",
                    }}
                >
                    1
                </button>
                <button style={{ margin: "0 5px", padding: "10px", cursor: "pointer" }}>2</button>
                <button style={{ margin: "0 5px", padding: "10px", cursor: "pointer" }}>3</button>
                <button style={{ margin: "0 5px", padding: "10px", cursor: "pointer" }}>
                    &raquo;
                </button>
            </div>
        </div>
    );
};

export default UsersList;
import React, { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";

const UsersList: React.FC = () => {
    const [filters, setFilters] = useState<string[]>([]);

    const users = [
        { name: "omar", role: "AGENCY" },
        { name: "ahmed", role: "Admin" },
        { name: "feby", role: "AGENCY" },
        { name: "fady", role: "Tourist" },
        { name: "miral", role: "AGENCY" },
        { name: "hany", role: "Tourist" },
    ];

    const toggleFilter = (role: string) => {
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
        <div className="container mt-5">
            <h2 className="text-center mb-4">List of All Users</h2>
            <div className="d-flex justify-content-center mb-4">
                <input
                    type="text"
                    className="form-control w-25 me-3"
                    placeholder="Search"
                />
                <button
                    className={`btn btn-outline-secondary me-2 ${
                        filters.includes("AGENCY") ? "active" : ""
                    }`}
                    style={{ borderRadius: "50%", padding: "10px 20px" }}
                    onClick={() => toggleFilter("AGENCY")}
                >
                    AGENCY
                </button>
                <button
                    className={`btn btn-outline-success me-2 ${
                        filters.includes("Admin") ? "active" : ""
                    }`}
                    style={{ borderRadius: "50%", padding: "10px 20px" }}
                    onClick={() => toggleFilter("Admin")}
                >
                    Admin
                </button>
                <button
                    className={`btn btn-outline-success ${
                        filters.includes("Tourist") ? "active" : ""
                    }`}
                    style={{ borderRadius: "50%", padding: "10px 20px" }}
                    onClick={() => toggleFilter("Tourist")}
                >
                    Tourist
                </button>
            </div>
            <div className="row">
                {filteredUsers.map((user, index) => (
                    <div key={index} className="col-md-4 mb-3">
                        <div className="card text-center" style={{ borderRadius: "50px" }}>
                            <div className="card-body">
                                <h5 className="card-title">{user.name}</h5>
                                <p className="card-text">{user.role}</p>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
            <nav className="d-flex justify-content-center mt-4">
                <ul className="pagination">
                    <li className="page-item">
                        <a className="page-link" href="#">
                            &laquo;
                        </a>
                    </li>
                    <li className="page-item active">
                        <a className="page-link" href="#">
                            1
                        </a>
                    </li>
                    <li className="page-item">
                        <a className="page-link" href="#">
                            2
                        </a>
                    </li>
                    <li className="page-item">
                        <a className="page-link" href="#">
                            3
                        </a>
                    </li>
                    <li className="page-item">
                        <a className="page-link" href="#">
                            &raquo;
                        </a>
                    </li>
                </ul>
            </nav>
        </div>
    );
};

export default UsersList;
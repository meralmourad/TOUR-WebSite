import React from 'react';
import './Report.scss';

const Report = () => {
    const reports = [
        "Relax and unwind in the tropical paradise of Maldives.",
        "Explore the beauty of Sydney.",
        "Explore the beauty of Paris with this amazing adventure package.",
        "Discover the ancient history of Rome.",
        "Experience the vibrant culture of Tokyo.",
        "Enjoy the breathtaking landscapes of New Zealand.",
        "Take a journey through the majestic mountains of Switzerland.",
        "Immerse yourself in the rich traditions of India.",
        "Witness the stunning Northern Lights in Iceland.",
        "Relax on the pristine beaches of the Bahamas."
    ];

    return (
        <div className="report-container">
            {reports.map((report, index) => (
                <div key={index} className="report-card">
                    <p>{report}</p>
                </div>
            ))}
        </div>
    );
};

export default Report;
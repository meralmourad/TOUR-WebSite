import { useEffect, useState } from 'react';
import './Report.scss';
import swal from "sweetalert";
import { useNavigate, useParams } from 'react-router-dom';
import { getReportsByTripId } from '../../service/ReportService';

const Report = () => {
    const id = useParams().id;
    const [reports, setReports] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchReport = async () => {
            try {
                const reports = await getReportsByTripId(id);
                setReports(reports);
                // console.log(reports);
            } catch (error) {
                console.error('Error fetching report:', error);
                swal("Opps!", error.response.data, "error");
                navigate(`/Trip/${id}`);
            }
        };

        fetchReport();
    }, [id, reports, navigate]);

    return (
        <div className="report-container">
            {reports.map((report) => (
                <div key={reports} className="report-card">
                    <p>{report.content}</p>
                </div>
            ))}
        </div>
    );
};

export default Report;
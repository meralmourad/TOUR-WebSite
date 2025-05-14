import { useEffect, useState } from 'react';
import './Report.scss';
import { useParams } from 'react-router-dom';
import { getReportsByTripId } from '../../service/ReportService';

const Report = () => {
    const id = useParams().id;
    const [reports, setReports] = useState([]);

    useEffect(() => {
        const fetchReport = async () => {
            try {
                const reports = await getReportsByTripId(id);
                setReports(reports);
            } catch (error) {
                console.error('Error fetching report:', error);
            }
        };

        fetchReport();
    }, [id]);

    return (
        <div className="report-container">
            {reports.map((report) => (
                <div key={reports.id} className="report-card">
                    <p>{report.content}</p>
                </div>
            ))}
        </div>
    );
};

export default Report;
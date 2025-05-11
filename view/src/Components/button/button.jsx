import Swal from "sweetalert";
import ReactDOM from "react-dom/client";
import Rate from "../Rate/Rate";    
import "./button.scss";

const RateNowButton = () => {
    const handleRateNow = () => {
        Swal.fire({
            html: `
                <div class="modal-title">Rate<br><span class="modal-subtitle">NOW!</span></div>
                <div id="rating-component-root"></div>
                <div class="modal-buttons">
                    <button id="report-button" class="modal-button report">Report!</button>
                    <button id="send-button" class="modal-button send">Send</button>
                </div>
            `,
            showConfirmButton: false,
            customClass: {
                popup: "custom-swal-popup",
            },
            didRender: () => {
                const container = document.getElementById("rating-component-root");
                if (container) {
                    const root = ReactDOM.createRoot(container);
                    root.render(<Rate />);
                }

                const reportBtn = document.getElementById("report-button");
                if (reportBtn) {
                    reportBtn.addEventListener("click", () => {
                        Swal.fire({
                            html: `
                                <textarea class="report-textarea" rows="4" placeholder="Write your report..."></textarea>
                                <div class="modal-buttons">
                                    <button id="cancel-report" class="modal-button report">Cancel</button>
                                    <button id="send-report" class="modal-button send">Send</button>
                                </div>
                            `,
                            showConfirmButton: false,
                            customClass: {
                                popup: "custom-swal-popup",
                            },
                            didRender: () => {
                                const cancelBtn = document.getElementById("cancel-report");
                                if (cancelBtn) cancelBtn.onclick = () => Swal.close();

                                const sendBtn = document.getElementById("send-report");
                                if (sendBtn) {
                                    sendBtn.onclick = () => {
                                        Swal.fire({
                                            icon: "success",
                                            title: "Thank you!",
                                            text: "Your report has been submitted.",
                                        });
                                    };
                                }
                            },
                        });
                    });
                }
            },
        });
    };

    return (
        <button className="rate-now-button" onClick={handleRateNow}>
            Rate NOW!
        </button>
    );
};

export default RateNowButton;

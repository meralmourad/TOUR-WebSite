import { useState, useEffect, use } from "react";
import "./Trip.scss";
import { useNavigate, useParams } from "react-router-dom";
import ReactDOMServer from "react-dom/server";
import { getTripById, updateTrip } from "../../../service/TripsService";
import {rateTrip , searchBookings } from '../../../service/BookingService'
import {sendReport} from '../../../service/ReportService'
import { useSelector } from "react-redux";
import Swal2 from "sweetalert2";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { addBooking } from "../../../service/BookingService";
import Rate from "../../Rate/Rate";

const MySwal = withReactContent(Swal2);

const Trip = () => {
  const { user } = useSelector((store) => store.info);
  const id = useParams()?.id;
  const navigate = useNavigate();
  const [tripData, setTripData] = useState(null);
  const [imageIndex, setImageIndex] = useState(0);
  const [txt, settxt] = useState("");
  const [rate, setRate] = useState(0);
  const [confirm, setConfirm] = useState(false);
  const [report, setReport] = useState("");
  const [BookedId , setBookedId ] = useState();

  useEffect(() => {
    if (tripData) {
      const now = new Date();
      const enddate = new Date(tripData.endDate);
      const startdate = new Date(tripData.startDate);
      if (startdate < now && enddate < now) {
        settxt("This trip has already ended.");
      } else if (startdate < now && enddate > now) {
        settxt("This trip has already started.");
      } else {
        settxt("This trip is upcoming. Book your spot now!");
      }
    }
  }, [tripData]);


  useEffect(() => {
    const getbooking = async () => {
      try {
        const bookid = await searchBookings(0, 1, tripData.id, user.id);
        setBookedId(bookid.id); 
      } catch (error) {
        console.log("bookingId", error);
      }
    };
    getbooking();
    
    if (confirm) {
    const sendRate = async () => {
        try {
          const rating = await rateTrip(BookedId, rate);
        } catch (error) {
          console.log("error in updating rate", error);
        }
      };
      sendRate();
    }
  }, [confirm]);
  


  useEffect(()=>{

    if(report){
      const AddReport = async ()=>{

        try {
          const now = new Date().toISOString();
          const sendrepo = await sendReport({
            tripId: tripData.id ,
            senderId: user.id ,
            content: report ,
            createdAt : now 
          })

        } 
      catch(error){
        console.log("adding report error" , error) ;
      }
    }

    AddReport() ;
  }},[report])

  useEffect(() => {
    const fetchTripData = async () => {
      try {
        const trip = await getTripById(id);
        setTripData(trip);
      } catch (error) {
        console.error("Error fetching trip data:", error);
      }
    };

    fetchTripData();
  }, [user, id]);

  const handleBooking = async () => {
    const result = await MySwal.fire({
      title: "Booking Details",
      html: `
          <div class="custom-form">
            <div class="row">
              <span>Name :</span><span>${user.name}</span>
              <span>Email :</span><span>${user.email}</span>
            </div>
            <div class="row">
              <span>Phone :</span><input id="swal-input-phone" class="swal2-input" value="${user.phoneNumber}" />
            </div>
            <div class="row">
              <span>Payment :</span><span>Cash Only!</span>
              <span>Seats :</span><input id="swal-input-seats" min=1 class="swal2-input" type="number" value="1"/>
            </div>
          </div>
        `,
      showCancelButton: true,
      confirmButtonText: "Confirm",
      cancelButtonText: "Discard",
      focusConfirm: false,
      preConfirm: () => {
        const phone = document.getElementById("swal-input-phone").value;
        const seats = document.getElementById("swal-input-seats").value;

        if (!phone || !seats) {
          Swal2.showValidationMessage("Please fill out all fields.");
          return;
        }

        return { phone, seats };
      },
    });

    if (result.isConfirmed) {
      const { phone, seats } = result.value;

      try {
        await addBooking({
          touristId: user.id,
          tripId: tripData.id,
          seatsNumber: seats,
          phoneNumber: phone,
        });
        MySwal.fire({
          title: "Booking Requested successfully!",
          text: `We will confirm you when Your booking of ${seats} seats approved!.`,
          icon: "success",
        });
      } catch (error) {
        console.error("Error adding booking:", error);
        MySwal.fire({
          title: "Booking Failed!",
          text: error.response.data.message,
          icon: "error",
        });
        return;
      }
    } else {
      MySwal.fire({
        title: "Booking Discarded!",
        text: "Booking Discarded!",
        icon: "error",
      });
    }
  };

  const handlePrev = () => {
    if (!tripData) return;
    setImageIndex((prev) =>
      prev === 0 ? tripData.images.$values.length - 1 : prev - 1
    );
  };

  const handleNext = () => {
    if (!tripData) return;
    setImageIndex((prev) =>
      prev === tripData.images.$values.length - 1 ? 0 : prev + 1
    );
  };

  return (
    <>
      {tripData && (
        <div className="trip-container">
          <div className="trip-header">
            <img
              src={
                tripData.images.$values[imageIndex] ||
                "https://media-public.canva.com/MADQtrAClGY/2/screen.jpg"
              }
              alt="Trip"
              className="trip-image"
            />
            {tripData.images.$values.length > 1 && (
              <>
                <button className="arrow-button left" onClick={handlePrev}>
                  &#8592;
                </button>
                <button className="arrow-button right" onClick={handleNext}>
                  &#8594;
                </button>
              </>
            )}
          </div>

          <div className="notification-item">
            <span className="notification-dot"></span>
            <p className="notification-text">{txt}</p>
            {txt === "This trip has already ended." && user.role!=='Tourist' && 
            
            <button className="button"
            
            onClick={()=>navigate(`/Report/${id}`)}
            
            > VIEW REPORTS </button>}
          </div>

          <div className="trip-details">
            <div className="trip-section">
              <h3>Available Sets:</h3>
              <p className="highlight">{tripData.availableSets}</p>
              <div onClick={() => navigate("/profile/" + tripData.agence.id)}>
                <h3>Agency Name:</h3>
                <p>{tripData.agence.name}</p>
              </div>
            </div>

            <div className="trip-section" onClick={() => navigate("/BookingList/" + id)}>
              <h3>Destinations</h3>
              <ul>
                {tripData.locations.$values.map((loc) => (
                  <li key={loc}>{loc}</li>
                ))}
              </ul>
              <p>
                <strong>From:</strong> {tripData.startDate}
              </p>
              <p>
                <strong>To:</strong> {tripData.endDate}
              </p>
            </div>

            <div className="trip-section">
              <h3>Price:</h3>
              <p className="highlight">{tripData.price} Per Person</p>
              <h3>Description</h3>
              <p>{tripData.description}</p>
            </div>
          </div>

          {tripData.status === 1 &&
            (user.role === "Admin" || user.id === tripData.agenceId) && (
              <div className="trip-buttons">
                <button
                  className="edit_butt"
                  onClick={() => navigate(`/EditTrip/${tripData.id}`)}
                >
                  Edit Trip
                  <span className="arrow">→</span>
                </button>
                <button
                  className="pending-bookings-button"
                  onClick={() => navigate(`/BookingPending/${tripData.id}`)}
                >
                  pending bookings
                  <span className="arrow">→</span>
                </button>
              </div>
            )}

          {user.role === "Tourist" && (
            <>
              {txt === "This trip is upcoming. Book your spot now!" && (
                <button className="book-now-button" onClick={handleBooking}>
                  BOOK NOW!
                  <span className="arrow">→</span>
                </button>
              )}
              {txt === "This trip has already ended." && (
                <>
                  <button
                    className="book-now-button"
                    onClick={() =>
                      MySwal.fire({
                        title: "RATE NOW!",
                        html: (
                          <div style={{ textAlign: "center" }}>
                            <Rate SendRate={setRate} />
                          </div>
                        ),
                        showCancelButton: true,
                        confirmButtonText: "Send",
                        cancelButtonText: "Report!",
                        showCloseButton: true,
                        customClass: {
                          popup: "rounded-popup",
                          confirmButton: "custom-confirm",
                          cancelButton: "custom-cancel",
                        },
                        preConfirm: () => {
                          return rate;
                        },
                      }).then((result) => {
                        if (result.isConfirmed) {
                          setConfirm(true);
                            MySwal.fire({
                            title: "Thank You!",
                            text: "Your rating has been submitted successfully.",
                            icon: "success",
                            });
                          } else if (
                            result.dismiss === Swal.DismissReason.cancel
                          ) {
                            MySwal.fire({
                            title: "Report Issue",
                            input: "textarea",
                            inputPlaceholder: "Write your report here...",
                            showCancelButton: true,
                            confirmButtonText: "Submit",
                            cancelButtonText: "Cancel",
                            }).then((reportResult) => {
                            if (reportResult.isConfirmed) {
                                setReport(reportResult.value);
                              MySwal.fire({
                              title: "Reported!",
                              text: "Your report has been submitted.",
                              icon: "info",
                              });
                            }
                            });
                          }
                      })
                    }
                  >
                    RATE NOW!
                  </button>
                </>
              )}
            </>
          )}
        </div>
      )}
    </>
  );
};

export default Trip;

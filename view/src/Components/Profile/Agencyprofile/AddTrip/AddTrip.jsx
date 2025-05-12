import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import swal from "sweetalert";
import "./AddTrip.scss";
import { addTrip , GetTripCategories, GetTripLocations  } from "../../../../service/TripsService";
import Select from "react-select";

const AddTrip = ({ setShowAddTrip }) => {
  const { user } = useSelector((store) => store.info);
  const [confirm , setConfirm] = useState(false);

    const [categoryOptions, setCategoryOptions] = useState([{
        value: "",
        label: "",
    }]);
    const [locationsOptions, setLocationsOptions] = useState([{
        value: "",
        label: "",
    }]);

  const [formData, setFormData] = useState({
    title: "",
    price: "",
    sets: "",
    description: "",
    startDate: "",
    endDate: "",
    photos: [],
    category: [],
    Locations: []
  });

    useEffect(() => {   
        const fetchCategories = async () => {
            try {
                const categories = await GetTripCategories();

                const options = categories.map((category) => ({
                    value: category.id,
                    label: category.name,
                }));
                setCategoryOptions(options);
            }
             catch (error) {
                console.error("Error fetching categories:", error);
            }
        };
        const fetchLocations = async () => {
            try {
                const locations = await GetTripLocations();
                const options = locations.map((location) => ({
                    value: location.id,
                    label: location.name,
                }));
                
                setLocationsOptions(options);
            }
             catch (error) {
                console.error("Error fetching categories:", error);
            }
        };
        fetchCategories();
        fetchLocations();
    }, [user]);

    useEffect(() => {
        if(confirm) {
            setConfirm(false);
            const hasEmptyFields = Object.values(formData).some(
                (value) => value === "" || value.length === 0);

            if (hasEmptyFields) {
                  swal({
                    title: "Error",
                    text: "Please fill in all required fields.",
                    icon: "error",
                    button: "Ok",
                });
                return;
            }

            try{
                const Handelsubmit = async ()=> {
                    console.log("formData", formData);
                    const dataToSend = new FormData();
                    
                    dataToSend.append("AgenceId", user.id);
                    dataToSend.append("Title", formData.title);
                    dataToSend.append("Price", +formData.price);
                    dataToSend.append("StartDate", formData.startDate);
                    dataToSend.append("EndDate", formData.endDate);
                    dataToSend.append("Description", formData.description);
                    dataToSend.append("AvailableSets", +formData.sets);
                    for(let i = 0; i < formData.Locations.length; i++) {
                        dataToSend.append("LocationIds", formData.Locations[i]);
                    }
                    for(let i = 0; i < formData.category.length; i++) {
                        dataToSend.append("CategoryIds", formData.category[i]);
                    }
                    for(let i = 0; i < formData.photos.length; i++) {
                        dataToSend.append("images", formData.photos[i], formData.photos[i].name);
                    }
                    
                    const submit = await addTrip(dataToSend);
                }

                Handelsubmit();
                swal({
                    title: "Done!",
                    text: "Trip added successfully.",
                    icon: "success",
                    button: "Ok",
                });
                setShowAddTrip(false);
            }
            catch (error) { 
                console.error("Error adding trip:", error);
                swal({
                    title: "Error",
                    text: "Failed to add trip.",
                    icon: "error",
                    button: "Ok",
                });
            }
        }
    }, [confirm, user]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value,
        }));
    };
        
    const handlePhotoUpload = (e) => {
        const file = e.target.files[0];
        if (file) {
            setFormData((prev) => ({
                ...prev,
                photos: [...prev.photos, file],
            }));
            e.target.value = ""; // clear after upload
        }
    };
        
    const handleCategoryChange = (selectedOptions) => {
        const selectedValues = selectedOptions.map((option) => option.value);
        setFormData((prev) => ({
            ...prev,
            category: selectedValues,
        }));
    };
    const handleLocationsChange = (selectedOptions) => {
        const selectedValues = selectedOptions.map((option) => option.value);
        setFormData((prev) => ({
            ...prev,
            Locations: selectedValues,
        }));
    };

    const handleDiscard = () => {
        setFormData({
            title: "",
            price: "",
            sets: "",
            description: "",
            startDate: "",
            duration: "",
            photos: [],
            category: [],
            Locations: [],
        });

        swal({
            title: "Discarded",
            text: "Trip Discarded Successfully.",
            icon: "error",
            button: "Ok",
        });

    setShowAddTrip(false);
  };

return (
    <>
        <div className="header">
            <div className="header-images">
                <img
                    src="https://beta.sis.gov.eg/media/195319/pyramids-2371501-1920.jpg"
                    alt="Alexandria"
                />
                <img
                    src="https://kenzly.com/wp-content/uploads/2025/04/Cairo-to-Alexandria-Full-Day-Tour-3.webp"
                    alt="Pyramids"
                />
                <div className="overlay-title">
                    <h1>
                      {user.name.toUpperCase()}
                    </h1>
                </div>
            </div>
        </div>

        <div className="add-trip-container">
            <div className="form-group">
                <label>Title:</label>
                <input
                    type="text"
                    name="title"
                    value={formData.title}
                    onChange={handleInputChange}
                />
            </div>

            <div className="form-group">
                <label>Price:</label>
                <input
                    type="number"
                    name="price"
                    value={formData.price}
                    onChange={handleInputChange}
                />
            </div>

            <div className="form-group">
                <label>Photos:</label>
                <input
                    type="file"
                    name="photo"
                    onChange={handlePhotoUpload}
                    style={{ display: "none" }}
                    id="photoInput"
                />
                <button
                    type="button"
                    onClick={() => document.getElementById("photoInput").click()}
                    style={{
                        backgroundColor: "#4CAF50",
                        color: "white",
                        padding: "10px 20px",
                        border: "none",
                        borderRadius: "5px",
                        cursor: "pointer",
                        fontSize: "14px",
                        marginTop: "10px",
                    }}
                >
                    Add Photo
                </button>

                {formData.photos.length > 0 && (
                    <div
                        className="preview-container"
                        style={{ display: "flex", flexWrap: "wrap", marginTop: "10px" }}
                    >
                        {formData.photos.map((file, index) => (
                            <div
                                key={index}
                                style={{
                                    position: "relative",
                                    marginRight: 10,
                                    marginBottom: 10,
                                }}
                            >
                                <img
                                    src={URL.createObjectURL(file)}
                                    alt={`preview-${index}`}
                                    className="photo-preview"
                                    style={{
                                        width: 80,
                                        height: 80,
                                        objectFit: "cover",
                                        borderRadius: 8,
                                        border: "1px solid #ccc",
                                    }}
                                />
                                <button
                                    onClick={() => {
                                        setFormData((prev) => ({
                                            ...prev,
                                            photos: prev.photos.filter((_, i) => i !== index),
                                        }));
                                    }}
                                    style={{
                                        position: "absolute",
                                        top: -5,
                                        right: -5,
                                        backgroundColor: "red",
                                        color: "white",
                                        border: "none",
                                        borderRadius: "50%",
                                        width: 20,
                                        height: 20,
                                        cursor: "pointer",
                                        display: "flex",
                                        alignItems: "center",
                                        justifyContent: "center",
                                        fontSize: "12px",
                                    }}
                                >
                                    ×
                                </button>
                            </div>
                        ))}
                    </div>
                )}
            </div>

            <div className="form-group">
                <label>Category:</label>
                <Select
                    isMulti
                    options={categoryOptions}
                    onChange={handleCategoryChange}
                    className="react-select-container"
                    classNamePrefix="react-select"
                    menuPortalTarget={document.body}
                    styles={{
                        menuPortal: (base) => ({ ...base, zIndex: 9999 }),
                    }}
                />
            </div>

            <div className="form-group">
                <label>Sets:</label>
                <input
                    type="number"
                    name="sets"
                    value={formData.sets}
                    onChange={handleInputChange}
                />
            </div>
            <div className="form-group">
                <label>Destinations:</label>
                <Select
                    isMulti
                    options={locationsOptions}
                    onChange={handleLocationsChange}
                    className="react-select-container"
                    classNamePrefix="react-select"
                    menuPortalTarget={document.body}
                    styles={{
                        menuPortal: (base) => ({ ...base, zIndex: 9999 }),
                    }}
                />
            </div>

            <div className="form-group">
                <label>Start Date:</label>
                <input
                    type="date"
                    name="startDate"
                    value={formData.startDate}
                    onChange={handleInputChange}
                />
            </div>
            <div className="form-group">
                <label>Description:</label>
                <textarea
                    name="description"
                    value={formData.description}
                    onChange={handleInputChange}
                />
            </div>


            <div className="form-group">
                <label>End Date:</label>
                <input
                    type="date"
                    name="endDate"
                    value={formData.endDate}
                    onChange={handleInputChange}
                />
            </div>
            <div className="button-group">
                <button className="discard-button" onClick={handleDiscard}>
                    Discard
                </button>
                <button className="confirm-button" onClick={()=>setConfirm(true)}>
                    Confirm
                </button>
            </div>
        </div>
    </>
);
};

export default AddTrip;

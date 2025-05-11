import React, { useState } from "react";
import { useSelector } from "react-redux";
import swal from "sweetalert";
import "./AddTrip.scss";
import { addTrip } from "../../../../service/TripsService";
import Select from "react-select";

const AddTrip = ({ setShowAddTrip }) => {
  const { user } = useSelector((store) => store.info);
  const [error, setError] = useState(null);
  const categoryOptions = [
    { value: "1", label: "Adventure" },
    { value: "2", label: "Relaxation" },
    { value: "3", label: "Cultural" },
    { value: "4", label: "Nature" },
    { value: "5", label: "Historical" },
    { value: "6", label: "Luxury" },
    { value: "7", label: "Family" },
    { value: "8", label: "Romantic" },
    { value: "9", label: "Wildlife" },
    { value: "10", label: "Sports" },
    { value: "11", label: "Beach" },
    { value: "12", label: "Adventure Sports" },
  ];

  const [formData, setFormData] = useState({
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

  const handleChange = (e) => {
    const { name, value, files, options, type } = e.target;
      setFormData((prev) => ({
        ...prev,
        [name]: value,
      }));
    
  };
  const handlePhotoChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setFormData((prev) => ({
        ...prev,
        photos: [...prev.photos, file],
      }));
      e.target.value = ""; 
    }
  };
  
  const handleCategoryChange = (selectedOptions) => {
    setFormData((prev) => ({
      ...prev,
      category: selectedOptions.map((opt) => opt.value),
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

  const handleConfirm = () => {
    const hasEmptyFields = Object.values(formData).some(
      (value) => value === "" || value.length === 0
    );

    if (hasEmptyFields) {
      swal({
        title: "Error",
        text: "Please fill in all required fields.",
        icon: "error",
        button: "Ok",
      });
      return;
    }

    const submit = async () => {
      try {
        await addTrip({
          AgenceId: user.id,
          Title: formData.title,
          Price: formData.price,
          StartDate: formData.startDate,
          Duration: formData.duration,
          Description: formData.description,
          AvailableSets: formData.sets,
          Category: formData.category,
          Images: formData.photos,
          LocationIds: formData.Locations,
        });

        swal({
          title: "Done!",
          text: "Trip added successfully.",
          icon: "success",
          button: "Ok",
        });

        setShowAddTrip(false);
      } catch (error) {
        setError(error.response?.data?.errors?.[0] || "An error occurred");
        console.error("Error adding trip:", error);
      }
    };

    submit();
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
                    onChange={handleChange}
                />
            </div>

            <div className="form-group">
                <label>Price:</label>
                <input
                    type="number"
                    name="price"
                    value={formData.price}
                    onChange={handleChange}
                />
            </div>

            <div className="form-group">
                <label>Photos:</label>
                <input
                    type="file"
                    name="photo"
                    onChange={handlePhotoChange}
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
                    onChange={handleChange}
                />
            </div>

            <div className="form-group">
                <label>Description:</label>
                <textarea
                    name="description"
                    value={formData.description}
                    onChange={handleChange}
                />
            </div>

            <div className="form-group">
                <label>Start Date:</label>
                <input
                    type="date"
                    name="startDate"
                    value={formData.startDate}
                    onChange={handleChange}
                />
            </div>

            <div className="form-group">
                <label>Duration:</label>
                <input
                    type="text"
                    name="duration"
                    value={formData.duration}
                    onChange={handleChange}
                />
            </div>

            <div className="button-group">
                <button className="discard-button" onClick={handleDiscard}>
                    Discard
                </button>
                <button className="confirm-button" onClick={handleConfirm}>
                    Confirm
                </button>
            </div>
        </div>
    </>
);
};

export default AddTrip;

import React, { useState } from "react";
import swal from "sweetalert";  
import "./AddTrip.scss";

const AddTrip = () => {
    const [formData, setFormData] = useState({
        title: "",
        price: "",
        photos: [],
        category: "",
        sets: "",
        description: "",
        startDate: "",
        duration: "",
    });

    const handleChange = (e) => {
        const { name, value, files } = e.target;

        if (name === "photos") {
            setFormData({ ...formData, photos: Array.from(files) });
        } else {
            setFormData({ ...formData, [name]: value });
        }
    };

    const handleDiscard = () => {
        setFormData({
            title: "",
            price: "",
            photos: [],
            category: "",
            sets: "",
            description: "",
            startDate: "",
            duration: "",
        });
    };

    const handleConfirm = () => {
        const { title, price, category, startDate } = formData;

        if (!title || !price || !category || !startDate) {
            swal({
                title: "Error",
                text: "Please fill in all required fields.",
                icon: "error",
                button: "Ok",
            });
            return;
        }

        swal({
            title: "Done !",
            text: "Trip added successfully.",
            icon: "success",
            button: "Ok",

        });

        handleDiscard();
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
              Agency <br /> FREEDOM
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
                        name="photos"
                        multiple
                        onChange={handleChange}
                    />
                    {formData.photos.length > 0 && (
                        <div className="preview-container">
                            {formData.photos.map((file, index) => (
                                <img
                                    key={index}
                                    src={URL.createObjectURL(file)}
                                    alt={`preview-${index}`}
                                    className="photo-preview"
                                />
                            ))}
                        </div>
                    )}
                </div>
                <div className="form-group">
                    <label>Category:</label>
                    <select
                        name="category"
                        value={formData.category}
                        onChange={handleChange}
                    >
                        <option value="">Select</option>
                        <option value="adventure">Adventure</option>
                        <option value="relaxation">Relaxation</option>
                        <option value="cultural">Cultural</option>
                    </select>
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

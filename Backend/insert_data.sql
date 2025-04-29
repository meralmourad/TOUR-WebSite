INSERT INTO Users (Name, Email, Role, PhoneNumber, Address, Password, Discriminator, CreatedAt, UpdatedAt) VALUES
('Alice', 'alice@example.com', 'Admin', '1234567890', '123 Main St', 'password1', 'User', GETDATE(), GETDATE()),
('Bob', 'bob@example.com', 'Agency', '2345678901', '456 Elm St', 'password2', 'User', GETDATE(), GETDATE()),
('Charlie', 'charlie@example.com', 'Tourist', '3456789012', '789 Oak St', 'password3', 'User', GETDATE(), GETDATE()),
('Diana', 'diana@example.com', 'Tourist', '4567890123', '321 Pine St', 'password4', 'User', GETDATE(), GETDATE()),
('Eve', 'eve@example.com', 'Agency', '5678901234', '654 Maple St', 'password5', 'User', GETDATE(), GETDATE()),
('Frank', 'frank@example.com', 'Tourist', '6789012345', '987 Cedar St', 'password6', 'User', GETDATE(), GETDATE()),
('Grace', 'grace@example.com', 'Tourist', '7890123456', '159 Birch St', 'password7', 'User', GETDATE(), GETDATE()),
('Heidi', 'heidi@example.com', 'Tourist', '8901234567', '753 Spruce St', 'password8', 'User', GETDATE(), GETDATE()),
('Ivan', 'ivan@example.com', 'Agency', '9012345678', '852 Willow St', 'password9', 'User', GETDATE(), GETDATE()),
('Judy', 'judy@example.com', 'Tourist', '0123456789', '951 Poplar St', 'password10', 'User', GETDATE(), GETDATE());


////////////// check vendor-idssssssss /////////////
INSERT INTO Trips (Title, Description, VendorId, StartDate, Price, Images, CreatedAt, UpdatedAt, Rating, Status, AvailableSets) VALUES
('Nile Cruise', 'A relaxing cruise on the Nile.', 4, '2025-06-01', 1200, 'nile.jpg', GETDATE(), GETDATE(), -1, 0, 20),
('Desert Safari', 'Adventure in the Sahara desert.', 5, '2025-07-10', 900, 'desert.jpg', GETDATE(), GETDATE(), -1, 0, 15),
('Red Sea Diving', 'Explore the Red Sea reefs.', 9, '2025-08-05', 1500, 'redsea.jpg', GETDATE(), GETDATE(), -1, 0, 10),
('Cairo City Tour', 'Discover Cairo''s history.', 4, '2025-05-20', 400, 'cairo.jpg', GETDATE(), GETDATE(), -1, 0, 25),
('Alexandria Beaches', 'Relax on Alexandria''s beaches.', 5, '2025-06-15', 600, 'alexandria.jpg', GETDATE(), GETDATE(), -1, 0, 18),
('Luxor Temples', 'Visit the ancient temples of Luxor.', 9, '2025-09-01', 1100, 'luxor.jpg', GETDATE(), GETDATE(), -1, 0, 12),
('Sinai Hiking', 'Hiking adventure in Sinai.', 4, '2025-10-10', 800, 'sinai.jpg', GETDATE(), GETDATE(), -1, 0, 16),
('Fayoum Oasis', 'Nature trip to Fayoum.', 5, '2025-11-01', 500, 'fayoum.jpg', GETDATE(), GETDATE(), -1, 0, 14),
('Siwa Oasis', 'Cultural trip to Siwa.', 9, '2025-12-05', 1300, 'siwa.jpg', GETDATE(), GETDATE(), -1, 0, 8),
('Aswan Tour', 'Tour of Aswan and surroundings.', 4, '2025-12-20', 1000, 'aswan.jpg', GETDATE(), GETDATE(), -1, 0, 22);




INSERT INTO Places (Name, Description, ImagesURL, Country) VALUES
('Pyramids of Giza', 'Ancient pyramids and Sphinx.', 'giza.jpg', 'Egypt'),
('Karnak Temple', 'Largest ancient religious site.', 'karnak.jpg', 'Egypt'),
('Valley of the Kings', 'Royal tombs in Luxor.', 'valley.jpg', 'Egypt'),
('Abu Simbel', 'Massive rock temples.', 'abusimbel.jpg', 'Egypt'),
('Mount Sinai', 'Biblical mountain.', 'sinai.jpg', 'Egypt'),
('Siwa Oasis', 'Desert oasis with springs.', 'siwa.jpg', 'Egypt'),
('Alexandria Library', 'Modern library and museum.', 'library.jpg', 'Egypt'),
('Red Sea Coral Reef', 'Diving hotspot.', 'redsea.jpg', 'Egypt'),
('Fayoum Lake', 'Natural lake and birdwatching.', 'fayoum.jpg', 'Egypt'),
('Islamic Cairo', 'Historic mosques and markets.', 'cairo.jpg', 'Egypt');



INSERT INTO TripPlaces(TripsId, LocationsId) VALUES
(6, 3),  -- Nile Cruise -> Pyramids of Giza
(6, 4),  -- Nile Cruise -> Karnak Temple
(7, 5),  -- Desert Safari -> Valley of the Kings
(8, 10), -- Red Sea Diving -> Red Sea Coral Reef
(9, 3),  -- Cairo City Tour -> Pyramids of Giza
(9, 12), -- Cairo City Tour -> Islamic Cairo
(10, 9), -- Alexandria Beaches -> Alexandria Library
(11, 6), -- Luxor Temples -> Abu Simbel
(12, 7), -- Sinai Hiking -> Mount Sinai
(13, 11),-- Fayoum Oasis -> Fayoum Lake
(14, 8), -- Siwa Oasis -> Siwa Oasis
(15, 6); -- Aswan Tour -> Abu Simbel




//////////// check trip TouristIdssssssssss /////////////

INSERT INTO Bookings(TouristId , TripId, IsApproved, PhoneNumber, Comment, Rating) VALUES
(5, 6, 1, '3456789012', 'Amazing trip!', 3),
(4, 7, 0, '4567890123', 'Looking forward to it.', -1),
(6, 8, 1, '6789012345', 'Great diving experience.', 2),
(7, 9, 1, '7890123456', 'Loved the city tour.', 3),
(8, 10, 0, '8901234567', 'Can’t wait!', -1),
(10, 11, 1, '0123456789', 'Luxor was beautiful.', 2),
(7, 12, 0, '3456789012', 'Ready for hiking.', -1),
(4, 13, 1, '4567890123', 'Fayoum is nice.', 1),
(6, 14, 1, '6789012345', 'Siwa was magical.', 3),
(7, 15, 0, '7890123456', 'Excited for Aswan.', -1);
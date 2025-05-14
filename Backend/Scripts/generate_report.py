import sys
import os
import pyodbc
import pandas as pd
import matplotlib.pyplot as plt
from fpdf import FPDF

if len(sys.argv) != 3:
    print("Usage: python generate_report.py <agency_id> <output_path>")
    sys.exit(1)

try:
    agency_id = int(sys.argv[1])
    output_path = sys.argv[2]
except ValueError:
    print("Please provide a valid integer for agency_id")
    sys.exit(1)

output_dir = os.path.dirname(output_path)
if not os.path.exists(output_dir):
    os.makedirs(output_dir)

temp_dir = os.path.join(output_dir, "temp")
if not os.path.exists(temp_dir):
    os.makedirs(temp_dir)

chart1_path = os.path.join(temp_dir, f"trip_report_chart_{agency_id}.png")

server = 'DESKTOP-9BN1EQE'
database = 'TourDb'
driver = '{ODBC Driver 17 for SQL Server}'
conn_string = f'DRIVER={driver};SERVER={server};DATABASE={database};Trusted_Connection=yes'

try:
    conn = pyodbc.connect(conn_string)
    print("Database connection successful")

    query_summary = f"""
    SELECT Trips.Title, COUNT(Bookings.Id) AS NumberOfBookings, AVG(Trips.Rating) AS AverageRating
    FROM Trips
    JOIN Bookings ON Trips.Id = Bookings.TripId
    WHERE Trips.VendorId = {agency_id}
    GROUP BY Trips.Title
    """

    df_summary = pd.read_sql(query_summary, conn)
    print(f"Found {len(df_summary)} trips")
    
    if len(df_summary) == 0:
        print("No trip data found for this agency ID")
        pdf = FPDF()
        pdf.add_page()
        pdf.set_font("Arial", size=12)
        pdf.cell(200, 10, txt=f"Trip Booking Summary Report - Agency {agency_id}", ln=True, align="C")
        pdf.cell(200, 10, txt="No trip data found for this agency ID", ln=True, align="C")
        pdf.output(output_path)
        print(f"PDF report with no data message generated at: {output_path}")
        sys.exit(0)
        
    plt.figure(figsize=(10, 6))
    plt.bar(df_summary["Title"], df_summary["NumberOfBookings"], color="skyblue")
    plt.xlabel("Trip Title")
    plt.ylabel("Number of Bookings")
    plt.title(f"Trip Booking Summary for Agency {agency_id}")
    plt.xticks(rotation=45)
    plt.tight_layout()
    plt.savefig(chart1_path)
    plt.close()
    print(f"Generated trip chart at {chart1_path}")

    pdf = FPDF()
    pdf.add_page()
    pdf.set_font("Arial", size=12)
    pdf.cell(200, 10, txt=f"Trip Booking Summary Report - Agency {agency_id}", ln=True, align="C")
    
    pdf.cell(50, 10, txt="Trip", border=1)
    pdf.cell(50, 10, txt="Bookings", border=1)
    pdf.cell(50, 10, txt="Avg Rating", border=1)
    pdf.ln()

    for _, row in df_summary.iterrows():
        title = str(row["Title"])
        if len(title) > 25:
            title = title[:22] + "..."
        pdf.cell(50, 10, txt=title, border=1)
        pdf.cell(50, 10, txt=str(row["NumberOfBookings"]), border=1)
        
        rating = row["AverageRating"]
        rating_str = str(round(float(rating), 2)) if pd.notnull(rating) else "N/A"
        pdf.cell(50, 10, txt=rating_str, border=1)
        pdf.ln()

    if os.path.exists(chart1_path):
        pdf.add_page()
        pdf.cell(200, 10, txt="Trip Booking Summary Chart", ln=True, align="C")
        pdf.image(chart1_path, x=20, y=30, w=170)

    pdf.output(output_path)
    print(f"PDF report generated at: {output_path}")

    if os.path.exists(chart1_path):
        os.remove(chart1_path)

except Exception as e:
    print(f"Error: {str(e)}")
    sys.exit(1)
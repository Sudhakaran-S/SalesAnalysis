Project Explanation: Sales Data Analysis System

Overview
The Sales Data Analysis System is a comprehensive solution designed to analyze and manage sales data. It supports importing sales records from CSV files into a relational database, performing data validations, and generating insightful reports like total revenue within a specified date range.

This project leverages technologies like ASP.NET Core, Entity Framework Core, and Postman for testing APIs. It aims to streamline sales data management and provide businesses with actionable insights.
Key Features
CSV Data Import:

Allows bulk import of sales records from CSV files.
Handles validations for missing or invalid data.
Inserts new records for products, customers, and orders into the database.
Revenue Calculation API:

A RESTful endpoint that calculates total revenue for a specified date range.
Includes error handling and validation for incorrect inputs.
Database Management:

Utilizes Entity Framework Core to manage database operations like CRUD for products, customers, and orders.
Error Handling and Logging:

Captures detailed error messages for debugging and user feedback.
Testing with Postman:

Ensures robust API functionality through manual testing.

Technology	Purpose
ASP.NET Core	Backend API development.
Entity Framework	ORM for database interactions.
SQL Server	Relational database for storing sales data.
CsvHelper	Library for parsing and reading CSV files.
Postman	API testing and validation.
![image](https://github.com/user-attachments/assets/4e55f1b0-c55c-4c51-add6-2911ecc8ae8f)

Project Flow
Step 1: Importing Sales Data
Input: A CSV file containing sales data (e.g., Product ID, Customer ID, Order details).
Processing:
Read the file using the CsvHelper library.
Validate and parse each record.
Check if the product or customer already exists in the database.
Add new products, customers, or orders incrementally to the database.
Output: Records are stored in the database with appropriate relationships.
Step 2: Revenue Calculation
Input: Start and end dates via a RESTful GET API.
Processing:
Validate the date range.
Query the Orders table for orders within the specified date range.
Calculate total revenue as QuantitySold * UnitPrice for each order.
Output: Total revenue as JSON.
Step 3: Error Handling
Handles scenarios like:
Missing or invalid CSV fields.
Invalid date ranges in API calls.
Database exceptions like duplicate keys or foreign key violations.

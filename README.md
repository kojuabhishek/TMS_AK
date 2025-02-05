# Task Management System

## Overview

The **Task Management System** is an ASP.NET Core MVC application designed to manage tasks efficiently. The application uses SQL Server as its database, with **Dapper** for lightweight and fast data access. The system includes features such as task CRUD operations, dynamic search and filtering, PDF report generation, user authentication with ASP.NET Core Identity, and robust logging using Serilog.

## Features

- **Task CRUD Operations:**  
  Create, read, update, and delete tasks using a repository pattern implemented with Dapper.

- **Search & Filter:**  
  Dynamically filter tasks by title or status.

- **Report Generation (PDF):**  
  Generate a Task Status Report that provides:  
  - Total number of tasks  
  - Count of tasks by status (Pending, In Progress, Completed)  
  - Percentage of completed tasks  
  - Export the report as a PDF file using iTextSharp


- **Logging:**  
  Application and database errors are logged using Serilog, with logs stored in SQL Server.

- **API Documentation:**  
  Swagger UI is available for exploring API endpoints.

## Setup Instructions

### 1. Clone the Repository

Clone the repository to your local machine and change connectionstring
Create db  named 'TMS' in ssms then
Run the .sql script in ssms

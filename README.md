JobRecorderNet ASP.NET Version 1.00 (08/07/25)
# JobRecorderNet
JobRecorderNet is a port of a quality assurance application (university-based project) that was made for an external client & his company. 

## About
Originally a Laravel-based application, it allowed the client's company to manage and document completed electrical and IT service jobs. It supports job creation, assignment of supervisors and technicians, collection of worksite evidence, supervisor sign-offs, and file uploads in various formats. The following outlines the features included in final delivered product:

## Features
- **Role-Based Job Access**: Supervisors and Technicians can only view and interact with jobs assigned to them.
- **Evidence Uploads**: Upload and manage images, PDFs, Word, and Excel files as proof of job completion.
- **Supervisor Sign-Off**: Supervisors can review and sign off on completed jobs directly in the system.
- **Job Filtering and Search**: Search by job number, type, client, assigned staff, address, and status.
- **Address Management**: Centralised address model linked to jobs with suburb, postcode, and state.
- **Audit-Ready Logs**: Each job includes creation and completion timestamps and a trail of changes.

## Functionality Overview
- **Dashboard**: Job totals, quick access to assigned roles, and system summary.
- **My Jobs Page**: Displays filtered jobs for supervisors and technicians.
- **File Upload Management**: Upload images, PDFs, Word, and Excel documents per job.
- **Role-Based Views**: Views differ for technicians vs supervisors.
- **Supervisor Sign-Off**: Supervisor-exclusive feature to confirm job completion.
- **Job Filtering**: Column filter dropdown + keyword search to locate specific jobs.
- **Pagination & Sorting**: Paginated table views ensure optimal performance.
- **Confirmation Modals**: Reusable deletion modals for users, jobs, and clients.

Note: Not all features may be implemented/work as intended, as progress on porting application is still ongoing. 

## Installation
To use this application, simply clone the repository and open it in your chosen IDE, then ensure all dependencies are properly installed.

## Usage
After cloning the repository, run the following commands in the terminal:
- dotnet build
- dotnet watch run

The application will automatically open in a new window (of your default browser).

## Authors
Stefan Barosan, Lucas Setiady   

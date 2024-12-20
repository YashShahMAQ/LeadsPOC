# LeadsPOC

This repository is a solution to the problem statement asked in an Interview:

---

## **Problem Statement**

You are required to create a simple microservice using .NET that interacts with an external service/API to retrieve data. This microservice should perform the following tasks:

1. **Data Retrieval**:
   - Call an external service/API every 10 minutes to pull a list of Leads.
   - Each Lead should have a unique email address.
   - Store the retrieved Leads in a database.

2. **API Endpoint**:
   - Implement an endpoint that returns the stored Leads.
   - The endpoint should accept query parameters to allow sorting and searching of the result set.

3. **Logging and Authorization**:
   - Implement necessary logging throughout the service for monitoring and debugging purposes.
   - Protect the endpoint with appropriate authorization.

---

## **Solution Overview**

The repository contains three Microservices designed to implement the above requirements:

### **1. ExternalAPI**
- **Purpose**: Simulates an external API that provides leads data.
- **Features**:
  - Returns a list of leads in JSON format.
  - Secured with an API Key for authorized access.
- **Endpoint**:
  - `GET /leads` - Returns mock leads data.
- **Configurations**:
  - API Key: Configurable in `appsettings.json`.

---

### **2. PullLeadsMicroService**
- **Purpose**: Pulls leads data periodically from the `ExternalAPI` and stores it in a SQL Server database.
- **Features**:
  - **Background Service**: Automatically triggers every 10 minutes to fetch data.
  - **Duplicate Handling**: Ensures no duplicate emails are inserted into the database.
  - **File Logging**: Logs operations and errors to a local file.
- **Key Components**:
  - Timer triggers the pull operation.
  - Logs fetched leads and database insertions.

---

### **3. GetLeadsMicroService**
- **Purpose**: Provides a REST API to retrieve leads data with filtering and sorting options.
- **Features**:
  - **JWT Authentication**: Secures endpoints with token-based authentication.
  - **In-Memory Caching**: Speeds up frequent data requests using a TTL-based cache.
  - **Filtering**: Search by `InterestArea`.
  - **Sorting**: Sort by `EmailAddress` in ascending or descending order.
- **Endpoints**:
  - `GET /api/leads?sortParam={asc|desc}&searchParam={InterestArea}`
    - Parameters:
      - `sortParam`: Sorts by email (ascending or descending).
      - `searchParam`: Filters by `InterestArea`.

---

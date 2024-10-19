# StellarStay Reservations API

Description
-----------

The **StellarStay Reservations API** is a hotel reservation management system. It allows users to check available rooms, create reservations, retrieve details of specific reservations, cancel reservations, and list all reservations. The system includes dynamic pricing rules, such as adjustments for the number of days, weekends, and additional options like breakfast.

Technologies Used
-----------------

-   **.NET Core 8** - Framework for building the API.
-   **Entity Framework Core** - ORM for interacting with the PostgreSQL database.
-   **PostgreSQL** - Database for storing reservation and room information.
-   **Swagger** - Interactive API documentation.
-   **xUnit** - Testing framework for unit tests.
-   **Moq** - Library for creating mock objects in tests.

Installation and Setup
----------------------

1.  **Clone the repository**:

    ```bash

    `git clone https://github.com/your-username/StellarStayReservationsAPI.git
    cd StellarStayReservationsAPI`

2.  **Database Setup**:

    Ensure that PostgreSQL is installed and running. Create a database for the API and update the connection string in `appsettings.json`:

    ```bash
    json

    `"ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Database=stellarstaydb;Username=your_user;Password=your_password"
    }`

3.  **Apply Migrations**:

    Apply the migrations to create the necessary tables in the database:

    ```bash   

    `dotnet ef database update`

4.  **Run the Application**:

    Start the API by running:

    ```bash   

    `dotnet run`

    The API will be available at `https://localhost:5001` or `http://localhost:5000`.

5.  **Access Swagger**:

    To explore the interactive API documentation, navigate to `https://localhost:5001/swagger` or `http://localhost:5000/swagger`.

Main Endpoints
--------------

### 1\. Get Available Rooms

-   **GET** `api/reservations/available-rooms`
-   **Description**: Returns a list of available rooms based on search criteria.
-   **Parameters**: `CheckInDate`, `CheckOutDate`, `NumberOfGuests`, `IncludesBreakfast`, `RoomType`.

### 2\. Create a Reservation

-   **POST** `api/reservations/create-reservation`
-   **Description**: Creates a new reservation for a room.
-   **Body**: `CreateRservationDto` with reservation details.

### 3\. Get Reservation Details

-   **GET** `api/reservations/{id}`
-   **Description**: Returns the details of a specific reservation.

### 4\. Cancel a Reservation

-   **DELETE** `api/reservations/{id}`
-   **Description**: Cancels an existing reservation.

### 5\. Get All Reservations

-   **GET** `api/reservations/all-reservations`
-   **Description**: Returns a list of all reservations, categorized as past, ongoing, and future.

Testing
-------

The project includes unit tests using **xUnit** and **Moq**. To run the tests, use the following command:

  `dotnet test`

This will run all the tests and provide a summary of the results, indicating which tests passed and which failed.

Contribution
------------

If you want to contribute to this project:

1.  Fork the repository.
2.  Create a new branch for your feature (`git checkout -b feature/new-feature`).
3.  Commit your changes (`git commit -m 'Add new feature'`).
4.  Push to the branch (`git push origin feature/new-feature`).
5.  Open a Pull Request.

License
-------

This project is licensed under the **GPL v3** license. See the `LICENSE` file for more details.

Contact
-------

If you have any questions or suggestions, you can reach me at:

-   **Email**: ingenieroleonardo@outlook.com
-   **LinkedIn**: [Leonardo Hernández LinkedIn](https://www.linkedin.com/in/leo7962/)

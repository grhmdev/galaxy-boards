# Galaxy Boards 🌌

Galaxy Boards is a web application for creating Kanban-style boards for managing personal projects.

It is composed from the following tiers:

- **Frontend**
    * Single page application that allows the user to create, view and edit project data
    * Built with:
        - TypeScript & React
        - Redux for state management
        - Axios for HTTP requests
        - TailwindCSS for styling
- **Backend**
    * REST API server that handles requests from the frontend to get or modify stored project data
    * Built with:
        - C# & ASP.NET for the API endpoints
        - Entity Framework for defining the database schema as code and object-relational mapping
- **Database**
    * A Postgres instance (container) to store project data

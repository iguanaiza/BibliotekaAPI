# About the project
API project for library management, created as one of the projects during informatics bachelor's studies (6th semester).

The API is intended to manage book resources and allow for the addition of supplemental data, such as specific book copies.

The project objectives were:
*To create the database model.
*To develop an API for data management.
*To provide a visual presentation of the solution using Swagger.

## Technologies
The project was developed as an ASP.NET Core 9 API. It runs in a local environment using a local Microsoft SQL Server database. The project was developed in Visual Studio 2022.

The Entity Framework Core package was installed to manage database communication. It is a lightweight, extensible, and cross-platform open-source version of the popular Entity Framework data access technology.

Swagger is used to demonstrate and test the application; it is an interactive tool for documenting and testing REST APIs. It automatically generates a clear user interface based on the application's code, allowing users to browse available endpoints, view their parameters, and execute requests directly from the browser.

The project implements three controllers: BookController, CatalogController, and CopyController. They are responsible, respectively, for:
*Handling CRUD operations on books.
*Handling GET operations used for the book catalog view (for the reader).
*Handling CRUD operations on the physical copies of books.

Each controller implements specific HTTP methods.

## Full project documentation
**Full project documentation** (in polish) is available in the file [w71345_Biblioteka_szkolna_API.pdf](https://github.com/iguanaiza/BibliotekaAPI/blob/master/w71345_Biblioteka_szkolna_API.pdf)

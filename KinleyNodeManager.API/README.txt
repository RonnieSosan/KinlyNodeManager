
API Documentation (Postman collection) - https://documenter.getpostman.com/view/753620/UV5WEJE8
SWAGGER DOC - loads by default on start up
Overview
This application is designed to perform the basic operation of adding, getting updating and deleting nodes. A post man collection has been made available to provide full information on how to call the APIs. The application has been designed to go with the option of connecting to an MSSQL database as opposed to a json file.

The application has also been designed with a default swagger documentation which will load on start up once the project has been built successfully.


**STRUCTURE""
The project has been structured with the individual functions in mind core, data, logic and API. At its core (.core) it contains all the basic models that are used application wide projects like the data, logic and API reference this project. Because the application sets up nodes and with their respective labels, hence we designed two models
one for the node and another for the node label creating a one to many relationship between both of them, meaning one node can have multiple labels. we also create service entities which are used as payloads for the api as we do not like to expose the model to be saved in the database directly to the APi since we might have some manipulation to do
before soring the data in the database. The service entities are like request and response model for receiving and sending messages via the API. All the APIs exposed have a response code and on a basic level "00" is for success and "06" is for a failure (this codes could be extended later as the project grows).

The persistence layer contains the individual repositories to be used to interact with the individual tables in the database, this layer has been built using the .net core entityframework core. Thiere is a migration file created using hte application DBContext and the model for the entities set in the core project.
On runtime the projects check for changes in the applicartiondb snapshot and uses the migration to perform any necessary migration to be done. The repositories inherit form a main generic repository that implement all the basic add, update, delete and get, thi is just to empasise on reuseability.

The service layer contains all the services available to the API. There is currently only one service that reads through the perfors all the basic CRUD operations on the entities. It references the core and persistence projects for performing the CRUD operations.

The API layer is the project that exposes all the created services through controllers. This The APIs also haev the options to be secured by a JWT Token which has already been implemented as an extension.

The structure has been separated in this format to avoid **cyclic referencing** and guarantee separation of concers at a very granular level.

**UPDATES**
- Dependendency Injection for the json file implementation: the project could create an alternative implementation for reading the service/nodes form a json file and this could have implemented by making use of an interface and having an implementation for json as an alternative to the database implementation. We would then inject these implementation based on a configuration setting.

SetUp
- The release verion of the .Net core project is available. This release folder could be set up on IIS as an application in the default website. Set the alias name  and reference the physical path. The application is built to connect to a DB server (MSSQL server) so the appsettings.json file will need to be updated with a connectionstring
for the database server The connection string has a key Kinly_DB which has a value representing the current connection string being used.

- ON start up the application automatically creates the data base and seeds some sample data.


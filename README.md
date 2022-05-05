![MySQLBadge](https://img.shields.io/badge/MySQL-005C84?style=for-the-badge&logo=mysql&logoColor=white)
![C#Badge](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

![Build](https://github.com/Rionzagal/MCPackServer/actions/workflows/dotnet.yml/badge.svg)

# MC-PACK Manager Server application

This application is designed for general management of the **MC-Engineering S.A. de C.V.** company, containing different modules such as *"Purchase module", "Materials' catalog and quotes"*, among others. This application is built as a *Blazor Server app*, using [*.NET 6*](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and [*ASP-NET Core*](https://github.com/dotnet/aspnetcore) for component development, due to their long-term support. This app uses a [*MySQL*](https://www.mysql.com/) database for storing the relevant data, as well as [*Entity Framework*](https://docs.microsoft.com/en-us/ef/) and [*Dapper*](https://www.nuget.org/packages/Dapper/) to access and interact with it. This app uses the [*ASP-NET Core Identity Framework*](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0&tabs=visual-studio) to enable user management and security levels, since it would be dangerous to let any user mingle with sensitive data, such as the company providers, clients, or even other users... 

For a user-friendly UI, the [*MudBlazor*](https://mudblazor.com/) library was chosen for the development of the UI components, since it provides an extended and customizable list of components and features to implement in the app. Also, since it is an open-source project, the library can work in most environments without much trouble. With the use of *MudBlazor*, the UI is generated mostly using *Razor components*, which enable the combined use of **HTML** and **C#** languages to establish the structure, as well as the behavior of each page or separate UI component present in the app. This alternative to **JavaScript** lets the developer reuse the established classes and services that connect to the database without having to reach them using a separate language. Also, for browser-specific methods, where **JavaScript** is needed, or just utterly better, a dependency named *JSInterop* can be used to access *JS* scripts in order to perform those tasks where **JavaScript** is better.

## Project services and core classes

This project is structured using abstract services for injection in different components, all derived from a `BaseService.cs` class, which lends its methods to each of the other services when needed, then subscribed in the main file `program.cs`. The services are structured in a folder named *Services*, which contains all of the methods that will access the database information, as well as a sub-folder with the name of *Interfaces*, which contains the abstractions for each service methods found in the *Services* folder.

![Services structure image][ServiceStructure]
![DbContext and Entities folders][DataAndEntities]

Each of the project entities are based on a database table, using the [*EFCore Powertools*](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools) package, as well as the **DBContext** component. The folder *Entities* contains the generated objects, referencing each of the database tables and views. On the other side, the *Data* folder contains the Identity *(for security)* and Entity *(for database interaction)* **DbContext** components, which use *Entity Framework* to access their relevant objects found in the database.

## Project pages and UI structure

This project is meant to be a single-page application (SPA), which means that only one page will be shown, and the content of that page will be rendered based on the actions and permissions of the user. The app starts with a not-authenticated view of the first page, which shows only the log-in button. When the user is already logged-in, the page will change to the logged-in *Index* page, which will show a nav-menu with the module links the user will have access to, given it has the necessary permission. 
![Not authorized page][NotLoggedInIndex]

When the user is authorized, and the Logged-in *Index* page is rendered, the menu on the left, known as *NavMenu* appears with all of the different navigation links for each different module. Clicking in one of the links will take the user to its corresponding module page, which is rendered as a *Single Page Module*.
![Authorized index page][LoggedInIndex]

Each module renders a table containing the most relevant information of the database object that it represents, which can be filtered depending on its attributes. Depending on the user's permissions over the module, different action buttons will appear, enabling different actions for the user *(such as create, edit or delete)*.
![Clients module first render][ClientsTable]

## Publishing and installation
First, a database server must be enabled using [*MySQL Workbench*](https://dev.mysql.com/doc/workbench/en/) in the server computer. The database schema can then be generated using the [Initial Queries](./MCPackServer/MySQL/MC_Pack_MySql_Script.sql) file in the *MySQL* folder. Then, the database will be generated as a schema with the name of `mcpackdb`. Once generated, the schema must be referenced in the `appsettings.json` file as such: 

```
{
  "ConnectionStrings": {
    "MySqlConnection": "Server=127.0.0.1;Port=3306;database=mcpackdb;user id=MySuperSecretUser;password=MySuperSecretPassword"
  },
  "AllowedHosts": "*"
}
```

This project is then published as an *Application Group* for *IIS* in the destined server machine, generating a specific website linked to the server's static IP Address. Each of the machines connected to the server will have access to the app, by entering the IP Address and the socket. If you like, you can add a domain by adding it to the *Hosts* file of your server, specifying the domain and the IP Address of the application. 

***
# Issues and Tasks
Here is a section for possible tasks and future actions for this project, even if it is already running.

## TO DO
Here is a list of things to do in order to complete/repair issues within the app or modules that could be added or upgraded.

- [ ] Add a module for storage management of the materials that arrived from the purchase orders. *Módulo de Almacén*
- [ ] Upgrade the Identity pages and their behavior in order to have different UserNames and Emails, as well as a registration page.
- [ ] Add *SMTP* functionality for e-mail sending
- [ ] Generate a *History* module for historical data visualization and management
- [ ] Add a *Logs* module in order to visualize what actions have been done in the app at all moments (this is for issue solving and bug-fixing)
- [ ] Upgrade the *Index* page in order to visualize relevant, first-sight data based on the user permissions
- [ ] Upgrade the query models for *Operators* support

## Would be nice to...
Here is a list of things that could make a better experience for the end-user, and make the app more user-friendly, but do not affect functionality.

- [ ] Add a Dark theme to the app
- [ ] Generate a standarized color scheme for lesser buttons (specially for the tabs tooltips)
- [ ] Add Real-Time functionality
- [ ] Add notifications and logged-in views
- [ ] Add a User settings table for UI customization
- [ ] Add [*Font Awesome*](https://fontawesome.com/) to app in order to expand the icons' gallery

[ServiceStructure]: https://user-images.githubusercontent.com/82832934/159050567-5671bf25-bf18-4c7d-909a-d3461f58c13e.png "Services file structure"
[DataAndEntities]: https://user-images.githubusercontent.com/82832934/159054108-afa274b7-61dd-453f-ab9b-c77be1517999.png "Data and Entities folders"
[LoggedInIndex]: https://user-images.githubusercontent.com/82832934/159054779-d466a9eb-3fc7-4468-be2a-d4c2573215e1.png "Logged-in Index page"
[NotLoggedInIndex]: https://user-images.githubusercontent.com/82832934/159055058-baf5784d-33c8-41af-8246-359c8aae4d19.png "Not logged-in Index page"
[ClientsTable]: https://user-images.githubusercontent.com/82832934/159069024-f02c90ec-2144-4c78-801a-67634117c4e5.png "Clients module table"

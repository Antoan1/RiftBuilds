# RiftBuilds

RiftBuilds is a League of Legends build planner developed for the SoftUni ASP.NET Fundamentals course.

Users can explore champions and items, create builds for different positions, and manage their builds through a responsive web interface.

## Features

- Browse 25 champions with descriptions and images.
- Explore 35 items with descriptions, images, and gold costs.
- Create builds with a title, champion, position, and optional description.
- Select between one and six distinct items using a visual item selector.
- View build details, selected items, and total item cost.
- Edit and delete existing builds.
- Use a responsive dark theme with a Nightbringer Yasuo home page.

The application currently uses a shared build collection without user accounts. All visitors can view, edit, and delete builds.

## Technologies

- ASP.NET Core MVC
- .NET 8
- Entity Framework Core 8
- SQL Server Express
- Razor views, layouts, and partial views
- Bootstrap
- HTML and CSS
- JavaScript and jQuery Validation
- Dependency Injection


## Database Models

| Model | Purpose |
|---|---|
| Champion | Stores champion names, descriptions, and image paths. |
| Item | Stores item names, descriptions, prices, and image paths. |
| Build | Stores a build's title, optional description, position, creation time, and champion. |
| BuildItem | Connects builds to items and stores each item's slot. |

A champion can have multiple builds. Each build belongs to one champion.

Builds and items have a many-to-many relationship through `BuildItem`. Its composite primary key consists of `BuildId` and `ItemId`, preventing the same item from appearing twice in a build.

Deleting a build removes its item associations while preserving the champion and item catalogs.

## Requirements for Local Setup

- Windows with Visual Studio 2022 supporting .NET 8.
- The **ASP.NET and web development** workload.
- .NET 8 SDK.
- SQL Server Express installed with an instance named `SQLEXPRESS`.
- Windows Authentication access to that instance, with permission to create the application database.
- Internet access for the initial package restore.

SQL Server Management Studio is optional.

## Getting Started

### 1. Download the project

Clone the repository:

```bash
git clone https://github.com/Antoan1/RiftBuilds.git
```

Alternatively, use **Code → Download ZIP** on GitHub and extract the archive.

Open `RiftBuilds.sln` in Visual Studio.

### 2. Restore dependencies

Right-click the solution in Solution Explorer and select **Restore NuGet Packages**.

### 3. Check the database configuration

The default connection string in `appsettings.json` is:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=RiftBuildsDb;Trusted_Connection=True;Encrypt=False;"
}
```

The application uses the local SQL Server Express instance and Windows Authentication. No database username, password, API key, or environment variable is required for the default local setup.

Ensure that the **SQL Server (SQLEXPRESS)** service is running.

If your SQL Server instance has a different name, configure `DefaultConnection` for that instance. The supplied configuration targets local development.

### 4. Create the database

In Visual Studio, open:

**Tools → NuGet Package Manager → Package Manager Console**

Set **Default project** to **RiftBuilds**, then run:

```powershell
Update-Database
```

This applies the existing migrations, creates `RiftBuildsDb`, and inserts the seeded champion and item data.

You do not need to create a new migration when installing the project.

### 5. Run the application

Set `RiftBuilds` as the startup project and select the `https` launch profile.

Press **Ctrl + F5**.

If prompted, trust the ASP.NET Core development certificate for local HTTPS.

The browser should open the application. Use the address shown by Visual Studio or the application console.

## Using the Application

1. Open **Champions** or **Items** to explore the catalogs.
2. Go to **Builds → Create Build**.
3. Enter a title, select a champion and position, and choose one to six items.
4. Optionally add a description.
5. Create the build and open **Details** to see its items and total cost.
6. Use **Edit** to update the build or **Delete** to open the deletion confirmation page.

## Validation

The application uses server-side validation and client-side validation for supported form fields.

Build validation includes:

- A required title with a maximum length of 100 characters.
- An optional description with a maximum length of 500 characters.
- A valid champion and position.
- Between one and six selected items.
- No duplicate items.
- Verification that selected champions and items exist in the database.

Form submissions that modify data use anti-forgery validation.

## Catalog and Images

Champion and item images are stored locally under `wwwroot/images`. An external API connection is not required while using the application.

Artwork is sourced from Riot Games Data Dragon. Most item images use version 14.19.1; the historical Luden's Echo image uses version 10.22.1.

The catalog is a curated educational collection containing historical content and illustrative prices. It does not reproduce the current live patch or enforce all in-game purchasing restrictions.

League of Legends, its characters, and associated artwork belong to Riot Games. RiftBuilds is an independent fan project and is not endorsed by Riot Games.

## Current Limitations

- No registration, authentication, or build ownership.
- Builds are shared between all visitors.
- Item slots follow the submitted selection order; manual drag-and-drop reordering is not implemented.
- No automatic synchronization with live game updates.

## Author

[Antoan1 on GitHub](https://github.com/Antoan1)
# Ticket System Backend

The backend API for the Ticket System. 

- Live link: http://mqumairi-ticketsystem.herokuapp.com/
- Front end repo: https://github.com/MQumairi/ticket-system-client

# Tech Stack

|         | Langs           | Frameworks | Tools                                   |
|---------|-----------------|------------|-----------------------------------------|
| Server  | C#              | .NET Core  | EF Core, Identity, Mediator, Cloudinary |
| Client  | Typescript      | React      | Semantic-UI, MobX, Axios                |
| Hosting | -               | -          | Docker, PostGres, Heroku                       |


## Server App

This comprises the system's backend, which is a restful API built using C# and ASP .NET Core 3.1.

Entity Framework Core is used to query the database. This helps abstract the database layer, allowing for quick integration of different database providers depending on hosting.

ASP.NET Core Identity was used to manage users and roles. JWT tokens were used for authentication.

I followed the mediator pattern, to minimize controller logic, and increase the app's modularity.

For handling images- user profile pictures, and ticket/comment attachments- I used a third party service, [Cloudinary](https://cloudinary.com/), given their generous free plan. Cloudinary integration is in the backend, so it is completely invisible to users, ensuring that their experience is as first-party-like as possible.

## Client App

The client app is a React SPA, written in Typescript, using Hooks throughout.

Semantic UI was the component library of choice. MobX was used for state management, and Axios for the data fetching.
The application is also fully mobile responsive!

## Hosting
This web app is hosted on Heroku. Despite the lack of official support for .NET web apps, I was able to host on Heroku by dockerizing the source code, and pushing to the Heroku container registry.

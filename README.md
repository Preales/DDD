### Este proyecto es basado en la siguiente playlist de [# Curso .NET Clean Architecture](https://www.youtube.com/watch?v=hCG38mYnrMc&list=PLOnQtvVd3KIRVH8jk8mEyGaYD-wua5sXC)

# DDD
Clean Architecture and DDD

# Crearción de proyectos
***Comandos para crear solucion y proyectos***
> dotnet new sln -o EasyPos 
> cd EasyPos 
> 
> dotnet new classlib -o Domain -f net7.0  
> dotnet new classlib -o Application -f net7.0  
> dotnet new classlib -o Infraestructure -f net7.0  
> dotnet new webapi -o Web.API -f net7.0     

***Comandos para referenciar proyectos***
> dotnet add Application/Application.csproj reference Domain/Domain.csproj 
> dotnet add Infraestructure/Infraestructure.csproj reference Domain/Domain.csproj Application/Application.csproj
> dotnet add Web.API/Web.API.csproj reference Application/Application.csproj Infraestructure/Infraestructure.csproj

***Commando para agregar proyectos a la solucion***
> dotnet sln add Web.API/Web.API.csproj Application/Application.csproj Infraestructure/Infraestructure.csproj

***Compilar solucion y ejecutar proyecto Web.API***
> dotnet build
> dotnet run -p Web.API

# Definicion de Domain
* AggregateRoot
Identificar todas las entidades de dominio que sean las raices
* DomainEvent
Representa los eventos de dominio, para integrar entre diferentes Agregate

* ValueObject

# Migraciones
> dotnet ef migrations add InitialMigration -p Infraestructure -s Web.API -o Persistence/Migrations
> dotnet ef database update -p Infraestructure -s Web.API

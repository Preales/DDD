# DDD
Clean Architecture and DDD

> # Comandos para crear solucion y proyectos
> dotnet new sln -o EasyPos 
> cd EasyPos 
> 
> dotnet new classlib -o Domain -f net7.0  
> dotnet new classlib -o Application -f net7.0  
> dotnet new classlib -o Infraestructure -f net7.0  
> dotnet new webapi -o Web.API -f net7.0     

> # Comandos para referenciar proyectos
> dotnet add Application/Application.csproj reference Domain/Domain.csproj 
> dotnet add Infraestructure/Infraestructure.csproj reference Domain/Domain.csproj Application/Application.csproj
> dotnet add Web.API/Web.API.csproj reference Application/Application.csproj Infraestructure/Infraestructure.csproj

> # Commando para agregar proyectos a la solucion
> dotnet sln add Web.API/Web.API.csproj Application/Application.csproj Infraestructure/Infraestructure.csproj

> # Compilar solucion y ejecutar proyecto Web.API
> dotnet build
> dotnet run -p Web.API


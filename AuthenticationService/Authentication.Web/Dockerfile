FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /app

COPY *.sln .
COPY Authentication.Web/*.csproj ./Authentication.Web/
COPY Authentication.Controllers/*.csproj ./Authentication.Controllers/
COPY Authentication.DataAccess/*.csproj ./Authentication.DataAccess/
COPY Authentication.Entities/*.csproj ./Authentication.Entities/
COPY Authentication.Infrastructure.Implementation/*.csproj ./Authentication.Infrastructure.Implementation/ 
COPY Authentication.Infrastructure.Interfaces/*.csproj ./Authentication.Infrastructure.Interfaces/ 
COPY Authentication.UseCases/*.csproj ./Authentication.UseCases/
COPY Authentication.UseCases.UnitTests/*.csproj ./Authentication.UseCases.UnitTests/ 
COPY Authentication.Utils/*.csproj ./Authentication.Utils/
RUN dotnet restore

COPY Authentication.Web/. ./Authentication.Web/
COPY Authentication.Controllers/. ./Authentication.Controllers/
COPY Authentication.DataAccess/. ./Authentication.DataAccess/
COPY Authentication.Entities/. ./Authentication.Entities/
COPY Authentication.Infrastructure.Implementation/ ./Authentication.Infrastructure.Implementation/ 
COPY Authentication.Infrastructure.Interfaces/ ./Authentication.Infrastructure.Interfaces/ 
COPY Authentication.UseCases/. ./Authentication.UseCases/ 
COPY Authentication.UseCases.UnitTests/. ./Authentication.UseCases.UnitTests/
COPY Authentication.Utils/. ./Authentication.Utils/

WORKDIR /app/Authentication.Web
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /app/Authentication.Web/out ./
ENTRYPOINT ["dotnet", "Authentication.Web.dll"]
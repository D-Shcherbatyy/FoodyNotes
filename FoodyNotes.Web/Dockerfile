FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /app

COPY *.sln .
COPY FoodyNotes.Web/*.csproj ./FoodyNotes.Web/
COPY FoodyNotes.Controllers/*.csproj ./FoodyNotes.Controllers/
COPY FoodyNotes.DataAccess/*.csproj ./FoodyNotes.DataAccess/
COPY FoodyNotes.Entities/*.csproj ./FoodyNotes.Entities/
COPY FoodyNotes.Infrastructure.Implementation/*.csproj ./FoodyNotes.Infrastructure.Implementation/ 
COPY FoodyNotes.Infrastructure.Interfaces/*.csproj ./FoodyNotes.Infrastructure.Interfaces/ 
COPY FoodyNotes.UseCases/*.csproj ./FoodyNotes.UseCases/
COPY FoodyNotes.UseCases.UnitTest/*.csproj ./FoodyNotes.UseCases.UnitTest/ 
COPY FoodyNotes.Utils/*.csproj ./FoodyNotes.Utils/
RUN dotnet restore

COPY FoodyNotes.Web/. ./FoodyNotes.Web/
COPY FoodyNotes.Controllers/. ./FoodyNotes.Controllers/
COPY FoodyNotes.DataAccess/. ./FoodyNotes.DataAccess/
COPY FoodyNotes.Entities/. ./FoodyNotes.Entities/
COPY FoodyNotes.Infrastructure.Implementation/ ./FoodyNotes.Infrastructure.Implementation/ 
COPY FoodyNotes.Infrastructure.Interfaces/ ./FoodyNotes.Infrastructure.Interfaces/ 
COPY FoodyNotes.UseCases/. ./FoodyNotes.UseCases/ 
COPY FoodyNotes.Utils/. ./FoodyNotes.Utils/

WORKDIR /app/FoodyNotes.Web
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app

COPY --from=build /app/FoodyNotes.Web/out ./
ENTRYPOINT ["dotnet", "FoodyNotes.Web.dll"]
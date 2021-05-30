
# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /source

# copy fsproj and restore as distinct layers
COPY *.sln .
COPY RentalCars.Web/*.csproj ./RentalCars.Web/
COPY RentalCars.Tests/*.csproj ./RentalCars.Tests/
RUN dotnet restore

# copy everything else and build app
COPY RentalCars.Web/. ./RentalCars.Web/
COPY RentalCars.Tests/. ./RentalCars.Tests/

WORKDIR /source/RentalCars.Web
RUN dotnet publish -c release -o /app --no-restore --no-cache 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "RentalCars.Web.dll"]

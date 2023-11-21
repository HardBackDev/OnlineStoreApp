FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /home/app
COPY ./*.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done
RUN dotnet restore
COPY . .
RUN dotnet publish ./OnlineStoreServer/OnlineStoreServer.csproj -o /publish/
COPY ./OnlineStoreServer/IdentityTables.sql /publish/
WORKDIR /publish
ENV ASPNETCORE_URLS=https://+:5001;http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Docker
ENTRYPOINT ["dotnet", "OnlineStoreServer.dll"]

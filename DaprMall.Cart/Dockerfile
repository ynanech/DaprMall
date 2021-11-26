#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DaprMall.Cart/DaprMall.Cart.csproj", "DaprMall.Cart/"]
COPY ["../DaprMall.Share/DaprMall.Share.csproj", "../DaprMall.Share/"]
RUN dotnet restore "DaprMall.Cart/DaprMall.Cart.csproj"
COPY . .
WORKDIR "/src/DaprMall.Cart"
RUN dotnet build "DaprMall.Cart.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprMall.Cart.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaprMall.Cart.dll"]

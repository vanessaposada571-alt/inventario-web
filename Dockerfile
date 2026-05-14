# Capa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar el archivo del proyecto manejando el espacio en el nombre
COPY ["Inventario Web.csproj", "./"]
RUN dotnet restore "Inventario Web.csproj"

# Copiar todo lo demás y compilar
COPY . ./
RUN dotnet publish "Inventario Web.csproj" -c Release -o out

# Capa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Configurar el puerto para Render
ENV ASPNETCORE_URLS=http://+:10000

# Punto de entrada apuntando a la dll generada
ENTRYPOINT ["dotnet", "Inventario Web.dll"]
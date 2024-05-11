# Utilizar la imagen de SDK de .NET Core como base
FROM ubuntu/dotnet-aspnet:8.0
WORKDIR /app
EXPOSE 5242
EXPOSE 7070

RUN dotnet dev-certs https --trust
RUN dotnet tool install --global dotnet-ef
RUN dotnet ef migrations add CreacionInicial
RUN dotnet ef database update

# Copiar el archivo del proyecto y restaurar las dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código fuente y construir la aplicación
COPY . ./
RUN dotnet build

# Ejecutar la aplicación
CMD ["dotnet", "run", "--project", "backendnet.csproj"]

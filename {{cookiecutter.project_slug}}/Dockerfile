FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
RUN mkdir -p ./{{ "{" }}{{cookiecutter.project_slug}}.Api,{{cookiecutter.project_slug}}.Cli{{ "}" }}
COPY src/{{cookiecutter.project_slug}}.Api/*.csproj ./{{cookiecutter.project_slug}}.Api/{{cookiecutter.project_slug}}.Api.csproj
COPY src/{{cookiecutter.project_slug}}.Cli/*.csproj ./{{cookiecutter.project_slug}}.Cli/{{cookiecutter.project_slug}}.Cli.csproj
RUN dotnet restore ./{{cookiecutter.project_slug}}.Api/{{cookiecutter.project_slug}}.Api.csproj
RUN dotnet restore ./{{cookiecutter.project_slug}}.Cli/{{cookiecutter.project_slug}}.Cli.csproj

# Copy everything else and build
COPY src/ /app
RUN dotnet publish -c Release -o out {{cookiecutter.project_slug}}.Api/*.csproj
RUN dotnet publish -c Release -o out {{cookiecutter.project_slug}}.Cli/*.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app

RUN useradd -ms /bin/bash {{cookiecutter.docker_user}}

USER {{cookiecutter.docker_user}}

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "{{cookiecutter.project_slug}}.Api.dll"]
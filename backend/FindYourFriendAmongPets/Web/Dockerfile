FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Web/Web.csproj", "Web/"]
COPY ["Accounts/PetFriend.Accounts.Domain/PetFriend.Accounts.Domain.csproj", "Accounts/PetFriend.Accounts.Domain/"]
COPY ["Shared/PetFriend.SharedKernel/PetFriend.SharedKernel.csproj", "Shared/PetFriend.SharedKernel/"]
COPY ["Accounts/PetFriend.Accounts.Application/PetFriend.Accounts.Application.csproj", "Accounts/PetFriend.Accounts.Application/"]
COPY ["Shared/PetFriend.Core/PetFriend.Core.csproj", "Shared/PetFriend.Core/"]
COPY ["Shared/PetFriend.Framework/PetFriend.Framework.csproj", "Shared/PetFriend.Framework/"]
COPY ["Accounts/PetFriend.Accounts.Contracts/PetFriend.Accounts.Contracts.csproj", "Accounts/PetFriend.Accounts.Contracts/"]
COPY ["Accounts/PetFriend.Accounts.Infrastructure/PetFriend.Accounts.Infrastructure.csproj", "Accounts/PetFriend.Accounts.Infrastructure/"]
COPY ["Volunteers/PetFriend.Volunteers.Presentation/PetFriend.Volunteers.Presentation.csproj", "Volunteers/PetFriend.Volunteers.Presentation/"]
COPY ["Volunteers/PetFriend.Volunteers.Domain/PetFriend.Volunteers.Domain.csproj", "Volunteers/PetFriend.Volunteers.Domain/"]
COPY ["Volunteers/PetFriend.Volunteers.Application/PetFriend.Volunteers.Application.csproj", "Volunteers/PetFriend.Volunteers.Application/"]
COPY ["Volunteers/PetFriend.Volunteers.Infrastructure/PetFriend.Volunteers.Infrastructure.csproj", "Volunteers/PetFriend.Volunteers.Infrastructure/"]
COPY ["Accounts/PetFriend.Accounts.Presentation/PetFriend.Accounts.Presentation.csproj", "Accounts/PetFriend.Accounts.Presentation/"]
RUN dotnet restore "Web/Web.csproj"
COPY . .
WORKDIR "/src/Web"
RUN dotnet build "Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]

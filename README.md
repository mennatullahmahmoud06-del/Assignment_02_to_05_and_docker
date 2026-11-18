# Assignment - Quest Arena 

A small **turn-based RPG Web API**. A player registers, creates up to 3 heroes (Warrior / Archer / Mage),
sends them on quests, and the dice decide the result. Winning gives XP and gold, and heroes level up.

The game is only the excuse: **every session assignment is applied in this one project**.

## How the game works
- **Hero power** = `level x 10 + class bonus`
- **Chance of success** = `50 + power - difficulty x 15`, always between 5% and 95%
- A quest needs a minimum level and can be completed once per hero. Failing costs nothing, just try again.
- Next level needs `level x 100` XP (a big reward can level you up more than once).
- A player can have at most **3 heroes** and can only play with **their own** heroes.

## How this covers every assignment

| Assignment | What we did in the session | Where it is applied here |
|---|---|---|
| **Assignment 2** - EF Core, DTOs, async, **navigation properties**, full CRUD | Relationships between entities | `Entities/` + `Data/Configurations/`: **HeroClass 1-N Hero**, **Hero N-M Quest through HeroQuest** (join entity with extra data). CRUD for `HeroClasses`, `Heroes`, `Quests`. Join rows (`HeroQuest`) are created by playing (`GameController`). `GET /api/heroes/{id}` loads the navigation properties (class + quests). |
| **Session 02** - EF Core, migrations, seeding | `DbContext`, `IEntityTypeConfiguration`, auto migration, JSON seeding | `Data/ApplicationDbContext.cs` (`ApplyConfigurationsFromAssembly`), `Data/Configurations/*`, `Data/DatabaseInitializer.cs` + `Program.cs` (auto migrate), `Data/StoreContextSeeding.cs` + `Data/Seeding/*.json` |
| **Assignment 3** - Repository, Result, Pagination, Service layer | Generic repository, `Result` / `GenericResult`, `PaginatedList`, services, AutoMapper | `Repositories/` (`GenericRepository<T>`, `HeroRepository`, `HeroClassRepository`), `Results/`, `Services/`, `Helper/*Profile.cs`. Pagination + search: `GET /api/heroes?pageNumber=1&pageSize=10&searchTerm=...` |
| **Assignment 4** - Interceptors + Middlewares | `SaveChangesInterceptor`s, custom middlewares | `Data/Interceptors/` (**SoftDelete**, **CreatedAt**, **ModifiedAt**), `Middlewares/` (**Exception handling**, **Request timing**, **Request ID header**, **Rate limiting**). Order is set in `Program.cs`. |
| **Assignment 5** - Filters | Action / exception filters with `[ServiceFilter]` | `Filters/` (**ActionExecutionLogging**, **ActionTiming**, **ExceptionHandling**). The first two are on `ApiControllerBase` (all controllers), the exception filter is on `GameController`. |
| **Session 06** - Authentication / CORS | Identity, JWT, `[Authorize]`, CORS | `Data/AuthenticationDbContext.cs`, `Controllers/AccountController.cs` (register / login), `Services/JwtService.cs`, `Extensions/ApplicationServiceExtension.cs` (JWT + CORS), roles **Admin** and **Player** |
| **Docker session** | Dockerfile, SQL Server container | `Assignment/Dockerfile` (multi-stage) and `docker-compose.yml` (API + SQL Server with health check) |
| Tests | xUnit | `Assignment.Tests/` (Results, pagination, game rules) |

The folder layout (Controllers, Data, Dtos, Entities, Errors, Extensions, Filters, Helper, Middlewares, Repositories, Results, Services) is the same as the session project.

## Run with Docker (one command)
```bash
docker compose up --build
```
Open **http://localhost:8080/swagger**. Both databases (`QuestArenaDb`, `QuestArenaAuth`) are created and seeded automatically.

## Run locally (Visual Studio / CLI)
Needs SQL Server LocalDB (or change the connection strings in `appsettings.json`).
```bash
cd Assignment
dotnet run
```
Open the Swagger URL printed in the console (`/swagger`).

### Migrations (Session 02)
The app works without migrations (it creates the schema itself). To use real migrations like in the session,
**drop the two databases first**, then:
```bash
dotnet tool install --global dotnet-ef        # once
cd Assignment
dotnet ef migrations add Init --context ApplicationDbContext --output-dir Migrations
dotnet ef migrations add AddAuth --context AuthenticationDbContext --output-dir Migrations/AuthenticationDb
dotnet run                                    # applies them automatically
```

## Play in 6 steps (Swagger)
1. `POST /api/Account/register` - create your player (password needs upper, lower, digit and symbol, e.g. `Player#123`).
2. `POST /api/Account/login` - copy the token, click **Authorize** in Swagger and paste it.
3. `GET /api/HeroClasses` - pick a class id, then `POST /api/Heroes` with `{ "name": "Aria", "heroClassId": 2 }`.
4. `GET /api/Game/heroes/{heroId}/available-quests` - see what you can do.
5. `POST /api/Game/heroes/{heroId}/quests/{questId}/attempt` - roll the dice. Repeat until you win and level up.
6. `GET /api/Heroes/leaderboard` and `GET /api/Heroes/{id}` (hero + completed quests).

**Admin:** login with `admin@questarena.local` / `Admin#12345` (seeded from `appsettings.json`) to create, edit or delete hero classes and quests.
Change these values and the JWT key before using anything outside of development.


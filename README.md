# E-Commerce API

A RESTful e-commerce backend built with **ASP.NET Core 9** using **Onion (Clean) Architecture**. It covers the full shopping flow: product catalog, Redis-backed baskets, JWT authentication, orders with delivery methods, and an admin area for managing products, orders and users.

## Features

- **Product catalog** with pagination, search, sorting and filtering by brand and type
- **Shopping basket** stored in **Redis** for fast, temporary carts
- **Authentication** with ASP.NET Core Identity and **JWT** bearer tokens
- **Orders** with shipping address, delivery methods and order status tracking
- **Admin area** (role `Admin`): manage products, update order status, assign roles, lock out or delete users
- **Specification pattern**, **Generic Repository** and **Unit of Work** for data access
- **AutoMapper** profiles for entity ↔ DTO mapping
- **Global exception handler** returning `ProblemDetails`, with typed business error codes
- **Serilog** logging to console and rolling daily files
- **Health check** endpoint at `/health`
- **Swagger UI** with JWT support in development
- Automatic **migrations and data seeding** (brands, types, products, delivery methods, roles and an admin user) on startup

## Architecture

```
E-Commerce.sln
├── Core/
│   ├── Domain/                 # Entities, contracts (repositories, UoW, specifications), business exceptions
│   ├── Services/               # Business logic, AutoMapper profiles, specifications
│   └── ServicesAbstractions/   # Service interfaces
├── Infrastructure/
│   ├── Persistence/            # EF Core DbContext, configurations, repositories, seeding, Redis basket repository
│   └── Presentation/           # API controllers (Account, Products, Baskets, Orders)
├── Shared/                     # DTOs shared between layers
└── E-Commerce/                 # Web host: Program.cs, admin controllers, middleware, static product images
```

Dependencies point inward: `Presentation → ServicesAbstractions ← Services → Domain`, with `Persistence` implementing the domain contracts.

## Tech Stack

| Area | Technology |
|---|---|
| Framework | ASP.NET Core 9 Web API |
| Database | SQL Server + Entity Framework Core 9 |
| Cache / Basket | Redis (StackExchange.Redis) |
| Auth | ASP.NET Core Identity + JWT Bearer |
| Mapping | AutoMapper |
| Logging | Serilog |
| Docs | Swagger / OpenAPI |

## API Endpoints

### Account
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| POST | `/api/Account/register` | – | Register a new user |
| POST | `/api/Account/login` | – | Log in and receive a JWT |
| GET | `/api/Account/currentUser` | User | Get the logged-in user |

### Products
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| GET | `/api/Product` | – | List products (`PageIndex`, `PageSize`, `BrandId`, `TypeId`, `SortBy`, `Search`) |
| GET | `/api/Product/{id}` | – | Get a product by id |
| GET | `/api/Product/brands` | – | List brands |
| GET | `/api/Product/types` | – | List product types |

### Basket
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| GET | `/Baskets?id={id}` | – | Get a basket |
| POST | `/Baskets` | – | Create or update a basket |
| DELETE | `/Baskets/{id}` | – | Delete a basket |

### Orders
| Method | Endpoint | Auth | Description |
|---|---|---|---|
| GET | `/api/Orders` | User | List the current user's orders |
| GET | `/api/Orders/{id}` | User | Get an order by id |
| POST | `/api/Orders` | User | Create an order from a basket |
| GET | `/api/Orders/deliveryMethods` | User | List delivery methods |

### Admin (role `Admin`)
| Method | Endpoint | Description |
|---|---|---|
| GET / POST | `/api/admin/Products` | List or create products |
| PUT / DELETE | `/api/admin/Products/{id}` | Update or delete a product |
| GET | `/api/admin/Orders` | List all orders |
| PUT | `/api/admin/Orders/{id}/status` | Update an order's status |
| GET | `/api/admin/Users` | List users |
| GET / DELETE | `/api/admin/Users/{id}` | Get or delete a user |
| POST / DELETE | `/api/admin/Users/{id}/roles` | Assign or remove a role |
| POST | `/api/admin/Users/{id}/lockout` | Toggle user lockout |

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (LocalDB works)
- Redis running on `localhost:6379` (for example `docker run -d -p 6379:6379 redis`)

### Run

```bash
git clone https://github.com/AhmedHamada47/ECommerce-API.git
cd ECommerce-API
dotnet run --project E-Commerce
```

On first run the database is migrated and seeded automatically. Open Swagger at `/swagger` on the URL shown in the console (for example `https://localhost:7228/swagger`).

### Configuration

Settings live in `E-Commerce/appsettings.json`:

| Key | Purpose |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `ConnectionStrings:RedisConnection` | Redis host |
| `JWT:Issuer`, `JWT:Audience`, `JWT:SecretKey` | JWT token settings |
| `BaseUrl` | Base URL used to build product image links |

A default admin account is seeded on startup: `admin@ecommerce.com`. Change its password before using this anywhere other than local development.

## Author

**Ahmed Hamada** · [GitHub](https://github.com/AhmedHamada47) · [LinkedIn](https://linkedin.com/in/ahmed-hamada47)

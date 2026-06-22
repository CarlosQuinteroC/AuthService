# Auth Service

Auth Service is a .NET microservice responsible for user authentication, token issuing, refresh token management, and basic role-based authorization.

This project is part of a DevOps learning path focused on .NET, Docker, Linux, Kubernetes, monitoring, observability, and clean service architecture.

## Goals

- Build a reusable authentication service.
- Learn ASP.NET Core Web API with Controllers.
- Apply a lightweight Clean Architecture.
- Persist users, roles, and refresh tokens using PostgreSQL.
- Run the service locally with Docker Compose.
- Prepare the service for future Kubernetes deployment.
- Add health checks, logging, metrics, and tests progressively.

## Scope

### Version 1 includes

- User registration.
- Login with email and password.
- JWT access token generation.
- Refresh token generation and rotation.
- Logout by revoking refresh tokens.
- User profile management.
- Password change with current password validation.
- Basic role management.
- Admin-only user and role operations.

### Version 1 does not include

- OAuth login.
- Multi-factor authentication.
- Email confirmation.
- Password recovery.
- Multi-tenant support.
- Advanced rate limiting.
- Full audit trail.

## Architecture

The service uses a lightweight Clean Architecture approach.

```txt
AuthService.Api
  HTTP controllers, request/response models, middleware, Swagger.

AuthService.Application
  Use cases, application interfaces, validation flow.

AuthService.Domain
  Core entities and domain rules.

AuthService.Infrastructure
  EF Core, PostgreSQL, repositories, password hashing, JWT generation.
```

Dependency direction:

```txt
Api -> Application -> Domain
Infrastructure -> Application + Domain
Api -> Infrastructure
```

Domain and Application must not depend on EF Core, PostgreSQL, ASP.NET Core, or JWT implementation details.

## Main Entities

### User

Represents an application user.

Fields:

- Id
- Email
- PasswordHash
- FirstName
- LastName
- Phone
- IsActive
- CreatedAt
- UpdatedAt

### Role

Represents a permission group.

Fields:

- Id
- Name

### RefreshToken

Represents a long-lived credential used to issue new access tokens.

Fields:

- Id
- UserId
- TokenHash
- ExpiresAt
- RevokedAt
- CreatedAt

## Authentication Rules

- Users log in using email and password.
- Login must validate:
  - User exists.
  - Password is correct.
  - User is active.
- Public registration assigns the default `user` role.
- Only admins can assign or remove roles.
- JWT should include only necessary claims:
  - `sub`
  - `email`
  - `roles`
  - expiration

## Authorization Rules

### Public endpoints

- Register
- Login
- Refresh token

### Authenticated endpoints

- Logout
- Get own profile
- Update own profile
- Change own password

### Admin endpoints

- List users
- Get user by id
- Activate user
- Deactivate user
- Create role
- Assign role to user
- Remove role from user

## Planned API

```txt
POST   /api/auth/register
POST   /api/auth/login
POST   /api/auth/refresh
POST   /api/auth/logout

GET    /api/profile/me
PUT    /api/profile/me
PUT    /api/profile/me/password

GET    /api/users
GET    /api/users/{id}
PUT    /api/users/{id}/activate
PUT    /api/users/{id}/deactivate

POST   /api/roles
POST   /api/users/{id}/roles
DELETE /api/users/{id}/roles/{roleId}
```

## Technology Stack

- .NET
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Docker Compose
- JWT Bearer Authentication
- Swagger/OpenAPI
- xUnit or NUnit for tests

## Local Development Plan

Initial development will happen locally before Kubernetes.

First milestones:

1. Create .NET solution and projects.
2. Define domain entities.
3. Define application interfaces and use cases.
4. Configure EF Core and PostgreSQL.
5. Implement register and login.
6. Add JWT authentication.
7. Add refresh token and logout.
8. Add Docker Compose.
9. Add tests.
10. Add health checks and structured logging.

## DevOps Learning Roadmap

### Phase 1: Local service

- Run API locally.
- Run PostgreSQL with Docker Compose.
- Use Swagger for manual testing.

### Phase 2: Containerization

- Add Dockerfile.
- Run API and database with Docker Compose.
- Add environment-based configuration.

### Phase 3: Quality

- Add unit tests.
- Add integration tests.
- Add basic CI pipeline.

### Phase 4: Observability

- Add health checks.
- Add structured logs.
- Add metrics.
- Integrate Prometheus/Grafana later.

### Phase 5: Kubernetes

- Create Kubernetes manifests.
- Deploy locally using Kind or Minikube.
- Add ConfigMaps and Secrets.
- Add readiness and liveness probes.

## Security Notes

- Never store plain text passwords.
- Store only password hashes.
- Store refresh tokens as hashes.
- Do not expose internal errors to clients.
- Do not trust user id from request body for self-profile operations.
- Use authenticated user id from JWT claims.
- Keep secrets out of source control.

## Project Status

Planning phase. No production code has been created yet.

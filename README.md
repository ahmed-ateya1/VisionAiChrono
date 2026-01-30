# VisionAiChrono

## Overview

VisionAiChrono is a comprehensive AI-powered video processing and pipeline management platform built with .NET 9.0. The application enables users to create, manage, and execute AI model pipelines for video analysis, with support for user authentication, video management, and collaborative features.

## 🚀 Key Features

### Core Functionality

- **Video Management**: Upload, process, and organize video content
- **AI Model Integration**: Support for multiple AI models with custom endpoints
- **Pipeline Creation**: Build and configure AI processing pipelines
- **User Authentication**: Secure JWT-based authentication system
- **Favorites System**: Mark and organize favorite videos and pipelines
- **Tagging System**: Categorize and search content using tags
- **Public/Private Pipelines**: Share pipelines or keep them private
- **Pipeline Inheritance**: Create derived pipelines from existing ones

### Technical Features

- **RESTful API**: Comprehensive REST API with OpenAPI/Swagger documentation
- **Clean Architecture**: Domain-driven design with clear separation of concerns
- **CQRS Pattern**: Command Query Responsibility Segregation using MediatR
- **Entity Framework Core**: Database operations with migrations support
- **Docker Support**: Containerized deployment ready
- **Comprehensive Logging**: Built-in logging and exception handling
- **Health Checks**: Application health monitoring endpoints
- **CORS Support**: Cross-origin resource sharing configuration

## 🏗️ Architecture

The project follows Clean Architecture principles with the following layers:

```
VisionAiChrono/
├── src/
│   ├── VisionAiChrono.API/          # Web API Layer
│   ├── VisionAiChrono.Application/  # Application Layer
│   ├── VisionAiChrono.Domain/       # Domain Layer
│   └── VisionAiChrono.Infrastructure/ # Infrastructure Layer
```

### Layer Responsibilities

#### 🌐 API Layer (`VisionAiChrono.API`)

- **Controllers**: RESTful endpoints for all major entities
  - `AccountController`: User authentication and account management
  - `VideoController`: Video upload, management, and processing
  - `AiModelController`: AI model registration and management
  - `PipelineController`: Pipeline creation and execution
  - `TagController`: Tag management and assignment
  - `FavoriteController`: User favorites functionality
- **Middlewares**: Global exception handling and request processing
- **Configuration**: JWT settings, database connections, CORS policies

#### 💼 Application Layer (`VisionAiChrono.Application`)

- **Services**: Business logic implementation
  - Authentication services with email verification
  - File management and video processing
  - AI model integration services
  - Pipeline execution and management
- **DTOs**: Data transfer objects for API communication
- **CQRS Implementation**: Commands and queries using MediatR
- **Validation**: Input validation using FluentValidation
- **Mapping**: Object mapping with Mapster

#### 🏛️ Domain Layer (`VisionAiChrono.Domain`)

- **Entities**: Core business entities
  - `Video`: Video metadata and content management
  - `AiModel`: AI model definitions and configurations
  - `Pipeline`: Processing pipeline definitions
  - `Tag`: Content categorization system
  - `Favourite`: User preferences tracking
  - `ApplicationUser`: Extended identity user model
- **Repository Contracts**: Data access abstractions
- **Enums**: Domain-specific enumerations

#### 🔧 Infrastructure Layer (`VisionAiChrono.Infrastructure`)

- **Data Access**: Entity Framework Core implementation
- **Repository Pattern**: Concrete repository implementations
- **Database Migrations**: Schema version control
- **External Services**: Third-party integrations

## 🛠️ Technology Stack

### Backend Technologies

- **.NET 9.0**: Latest .NET framework
- **ASP.NET Core**: Web API framework
- **Entity Framework Core 9.0**: ORM for data access
- **MediatR**: CQRS and mediator pattern implementation
- **FluentValidation**: Input validation library
- **Mapster**: Object-to-object mapping
- **JWT Authentication**: Secure token-based authentication
- **Swagger/OpenAPI**: API documentation

### Database

- **SQL Server**: Primary database (configurable connection string)
- **Entity Framework Migrations**: Database schema management

### DevOps & Deployment

- **Docker**: Containerization support
- **Health Checks**: Application monitoring
- **Logging**: Built-in ASP.NET Core logging
- **CORS**: Cross-origin resource sharing

### Email Services

- **Brevo (Sendinblue)**: Email service integration for notifications

## 🚀 Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code
- Docker (optional, for containerized deployment)

### Installation

1. **Clone the Repository**

   ```bash
   git clone [repository-url]
   cd VisionAiChrono
   ```

2. **Restore Dependencies**

   ```bash
   dotnet restore
   ```

3. **Update Database Connection**

   Edit `src/VisionAiChrono.API/appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "visionAiConnection": "Server=YOUR_SERVER;Database=VisionAiChrono;TrustServerCertificate=True;Integrated Security=SSPI;"
     }
   }
   ```

4. **Run Database Migrations**

   ```bash
   cd src/VisionAiChrono.API
   dotnet ef database update
   ```

5. **Build and Run**

   ```bash
   dotnet build
   dotnet run --project src/VisionAiChrono.API
   ```

6. **Access the Application**
   - API: `https://localhost:7000` (HTTPS) or `http://localhost:5000` (HTTP)
   - Swagger UI: `https://localhost:7000/swagger`

### Docker Deployment

1. **Build Docker Image**

   ```bash
   docker build -f src/VisionAiChrono.API/Dockerfile -t visionaichrono .
   ```

2. **Run Container**
   ```bash
   docker run -p 8080:8080 -p 8081:8081 visionaichrono
   ```

## 📊 Database Schema

### Core Entities

- **Videos**: Store video metadata, file paths, and processing status
- **AiModels**: AI model configurations with endpoints and versions
- **Pipelines**: Processing workflows with JSON configuration
- **Tags**: Content categorization and searchability
- **Favourites**: User preference tracking
- **Users**: Extended Identity framework for user management
- **PipelineRuns**: Execution history and results
- **VideoTags**: Many-to-many relationship between videos and tags

### Relationships

- Users can create multiple Pipelines and AiModels
- Pipelines can be derived from other Pipelines (inheritance)
- Videos can have multiple Tags through VideoTags
- Users can favorite Videos and Pipelines through Favourites

## 🔐 Authentication & Security

- **JWT Token Authentication**: Secure token-based authentication
- **User Registration/Login**: Account management with email verification
- **Role-based Access**: User-specific content access
- **Password Security**: Secure password handling
- **Email Verification**: Account activation via email

## 📁 File Management

- **Video Upload**: Secure file upload to `wwwroot/Upload/` directory
- **File Validation**: Type and size validation for uploads
- **Organized Storage**: Structured file organization system

## 🔄 Pipeline System

### Pipeline Features

- **Visual Pipeline Builder**: JSON-based pipeline configuration
- **AI Model Integration**: Connect multiple AI models in workflows
- **Public/Private Sharing**: Control pipeline visibility
- **Pipeline Inheritance**: Create variations from existing pipelines
- **Execution Tracking**: Monitor pipeline runs and results

### Pipeline Workflow

1. Create pipeline with AI models
2. Configure processing steps
3. Execute on video content
4. Store and retrieve results
5. Share or derive new pipelines

## 📈 API Endpoints

### Authentication Endpoints

- `POST /api/Account/register` - User registration
- `POST /api/Account/login` - User authentication
- `POST /api/Account/verify-email` - Email verification

### Video Management

- `GET /api/Video/get-all` - Retrieve videos with pagination
- `POST /api/Video/add` - Upload new video
- `DELETE /api/Video/delete/{id}` - Remove video

### AI Model Management

- `GET /api/AiModel/get-all` - List AI models
- `POST /api/AiModel/add` - Register new AI model
- `PUT /api/AiModel/update/{id}` - Update model configuration

### Pipeline Operations

- `GET /api/Pipeline/get-all` - List pipelines
- `POST /api/Pipeline/add` - Create new pipeline
- `POST /api/Pipeline/execute` - Run pipeline on videos

### Content Management

- `GET /api/Tag/get-all` - Retrieve tags
- `POST /api/Favorite/add` - Add to favorites
- `DELETE /api/Favorite/remove` - Remove from favorites

## 🧪 Development

### Project Structure

```
VisionAiChrono/
├── VisionAiChrono.sln              # Solution file
├── README.md                       # This file
└── src/
    ├── VisionAiChrono.API/
    │   ├── Controllers/            # API endpoints
    │   ├── Middlewares/           # Custom middleware
    │   ├── Properties/            # Launch settings
    │   └── wwwroot/Upload/        # File upload directory
    ├── VisionAiChrono.Application/
    │   ├── Services/              # Business logic
    │   ├── Dtos/                  # Data transfer objects
    │   ├── Slices/                # CQRS commands/queries
    │   └── Helper/                # Utility classes
    ├── VisionAiChrono.Domain/
    │   ├── Models/                # Domain entities
    │   ├── RepositoryContract/    # Repository interfaces
    │   └── Enums/                 # Domain enumerations
    └── VisionAiChrono.Infrastructure/
        ├── Data/                  # EF Core DbContext
        ├── Repositories/          # Data access implementation
        └── Migrations/            # Database migrations
```

### Building and Testing

```bash
# Build solution
dotnet build

# Run tests (if test projects exist)
dotnet test

# Create migration
cd src/VisionAiChrono.API
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

## 📝 Configuration

### Key Configuration Files

- `appsettings.json`: Main application configuration
- `appsettings.Development.json`: Development-specific settings
- `launchSettings.json`: Development server settings

### Environment Variables

- Database connection strings
- JWT signing keys
- Email service configurations
- File upload paths

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

[Add your license information here]

## 📞 Support

[Add contact information or support channels]

---

**VisionAiChrono** - Empowering video analysis through AI-driven pipeline automation.

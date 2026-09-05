# 🏗️ Arquitectura del Proyecto

## Visión General

Esta aplicación sigue una **arquitectura Full-Stack de tres capas**:

```
┌─────────────────────────────────────────────┐
│         FRONTEND (React)                    │
│  - Components                               │
│  - State Management                         │
│  - HTTP Client                              │
└─────────────────────┬───────────────────────┘
                      │ HTTP/REST API
                      ▼
┌─────────────────────────────────────────────┐
│  BACKEND (.NET / C#)                        │
│  ┌────────────────────────────────────────┐ │
│  │ Controllers (API Endpoints)            │ │
│  └────────────────────────────────────────┘ │
│  ┌────────────────────────────────────────┐ │
│  │ Services (Lógica de Negocio)           │ │
│  └────────────────────────────────────────┘ │
│  ┌────────────────────────────────────────┐ │
│  │ Repositories (Acceso a Datos)          │ │
│  └────────────────────────────────────────┘ │
└─────────────────────┬───────────────────────┘
                      │ SQL
                      ▼
┌─────────────────────────────────────────────┐
│  DATABASE (SQL Server)                      │
│  - Tareas                                   │
│  - Estados                                  │
│  - Importancias                             │
└─────────────────────────────────────────────┘
```

---

## 🔑 Principios de Diseño

### 1. **Separación de Responsabilidades**

Cada capa tiene UN trabajo específico:

- **Frontend:** Presentar datos y capturar entrada del usuario
- **Backend:** Validar datos, aplicar reglas de negocio, persistir
- **BD:** Almacenar datos de forma segura y eficiente

### 2. **SOLID Principles**

- **S**ingle Responsibility: Una clase, una responsabilidad
- **O**pen/Closed: Abierto para extensión, cerrado para modificación
- **L**iskov Substitution: Interfaces intercambiables
- **I**nterface Segregation: Interfaces específicas
- **D**ependency Inversion: Depender de abstracciones, no implementaciones

### 3. **Clean Code**

- Nombres descriptivos
- Funciones pequeñas (máx 20 líneas)
- Comentarios para el "por qué", no el "qué"
- DRY (Don't Repeat Yourself)

---

## 📁 Estructura Backend (.NET)

```
backend/
├── TodoApp.API/
│   ├── Controllers/
│   │   └── TareasController.cs      # Endpoints HTTP
│   ├── Program.cs                   # Configuración de la app
│   └── appsettings.json             # Configuración por ambiente
│
├── TodoApp.Domain/
│   ├── Models/
│   │   ├── Tarea.cs                # Modelo de dominio
│   │   ├── Estado.cs
│   │   └── Importancia.cs
│   └── Enums/                       # Enums de negocio
│
├── TodoApp.Infrastructure/
│   ├── Data/
│   │   ├── AppDbContext.cs         # DbContext de EF Core
│   │   └── Migrations/             # Historial de cambios BD
│   ├── Repositories/
│   │   ├── IRepository.cs          # Interfaz genérica
│   │   └── TareaRepository.cs      # Implementación específica
│   └── Services/
│       └── TareaService.cs         # Lógica de negocio
│
└── TodoApp.Tests/
    ├── Unit/
    │   └── Services/
    │       └── TareaServiceTests.cs
    └── Integration/
        └── Controllers/
            └── TareasControllerTests.cs
```

### Explicación de capas:

**TodoApp.Domain** (Core de negocio)
- Modelos de datos
- Interfaces
- Lógica pura de negocio
- **SIN dependencias externas**

**TodoApp.Infrastructure** (Acceso a datos)
- Entity Framework DbContext
- Repositories (patrón Repository)
- Migraciones
- Acceso a recursos externos

**TodoApp.API** (Presentación)
- Controllers (endpoints HTTP)
- DTOs (Data Transfer Objects)
- Configuración de middleware
- Autenticación/Autorización

**TodoApp.Tests**
- Tests unitarios de servicios
- Tests de integración de endpoints
- Fixtures y test helpers

---

## 📦 Frontend (React)

```
frontend/
├── src/
│   ├── components/
│   │   ├── TareaForm.jsx           # Formulario de crear/editar
│   │   ├── TareaList.jsx           # Lista de tareas
│   │   ├── TareaItem.jsx           # Item individual
│   │   └── ErrorBoundary.jsx       # Manejo de errores
│   │
│   ├── pages/
│   │   ├── Home.jsx                # Página principal
│   │   └── NotFound.jsx            # Página 404
│   │
│   ├── services/
│   │   ├── api.js                  # Cliente HTTP (fetch/axios)
│   │   └── tareasService.js        # Lógica de llamadas a API
│   │
│   ├── hooks/
│   │   ├── useTareas.js            # Custom hook para tareas
│   │   └── useApi.js               # Custom hook para API calls
│   │
│   ├── context/
│   │   └── TareasContext.js        # Context API para estado global
│   │
│   ├── styles/
│   │   ├── App.css
│   │   └── components.css
│   │
│   ├── App.jsx                     # Componente principal
│   └── index.js                    # Entry point
│
└── package.json
```

---

## 🔄 Flujo de Datos

### Crear una tarea:

```
1. Usuario rellenar formulario en React
        ↓
2. onClick → validación en Frontend
        ↓
3. POST /api/tareas con datos JSON
        ↓
4. TareasController recibe request
        ↓
5. TareaService valida reglas de negocio
        ↓
6. TareaRepository guarda en BD
        ↓
7. Response 201 Created con tarea creada
        ↓
8. React actualiza estado con nueva tarea
        ↓
9. UI se renderiza con la nueva tarea
```

---

## 🔐 Seguridad

### Niveles de validación (Defense in Depth):

1. **Frontend (UX):**
   - Validar antes de enviar
   - Previene requests innecesarios

2. **API (Gateway):**
   - Validar formato y tipos
   - Autorizción
   - Rate limiting

3. **Servicios (Negocio):**
   - Validar reglas de negocio
   - Lógica de autorización

4. **Base de Datos:**
   - Constraints
   - Integridad referencial
   - Últimas defensas

---

## 🎯 Patrones Implementados

### 1. **Repository Pattern**

```csharp
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```

**Ventaja:** Abstrae el acceso a datos, fácil de testear

### 2. **Dependency Injection**

```csharp
// En Program.cs
builder.Services.AddScoped<ITareaService, TareaService>();
builder.Services.AddScoped<ITareaRepository, TareaRepository>();
```

**Ventaja:** Código desacoplado, fácil de mock en tests

### 3. **DTO Pattern**

```csharp
// Domain Model
public class Tarea { /* ... */ }

// DTO (para API)
public class TareaDto { /* ... */ }

// Mapper
var dtos = tareas.MapTo<List<TareaDto>>();
```

**Ventaja:** Controla qué datos expone la API

---

## 🚀 Deployment

### Desarrollo:
```bash
# Backend
dotnet run

# Frontend
npm start
```

### Producción:
- Backend → Azure / AWS / DigitalOcean (IIS o Docker)
- Frontend → Vercel / Netlify / GitHub Pages
- BD → Managed SQL Server (Azure SQL, AWS RDS)

---

## 📊 Consideraciones de Performance

1. **Lazy Loading:** No cargar datos hasta que se necesiten
2. **Pagination:** Dividir resultados en páginas
3. **Indexación:** Índices en BD para queries frecuentes
4. **Caching:** (Fase 2) Caché de tareas en memoria
5. **Compresión:** GZIP en respuestas HTTP

---

## 🔍 Evolución de la Arquitectura

**Fase 1 (Actual):**
- Arquitectura simple de 3 capas
- Estado local en React
- API síncrona

**Fase 2:**
- Añadir CQRS (Command Query Responsibility Segregation)
- Implementar patrón Mediator (MediatR)
- Caché distribuido (Redis)

**Fase 3:**
- Event Sourcing
- Microservicios
- Message queues (RabbitMQ)

---

## 📚 Referencias

- Clean Architecture: Robert C. Martin
- Domain-Driven Design: Eric Evans
- Design Patterns: Gang of Four

# 📋 Aplicación de Gestión de Tareas - TODO App

## 🎯 Descripción del Proyecto

Una aplicación **Full-Stack** de gestión de tareas que permite crear, organizar y dar seguimiento a actividades. Diseñada como proyecto de aprendizaje para desarrollar habilidades de programación senior.

**Stack Tecnológico:**
- **Backend:** C# / .NET (API REST)
- **Frontend:** React.js
- **Base de Datos:** SQL Server
- **Control de Versiones:** Git / GitHub

---

## 🚀 Características (MVP)

### ✅ Funcionalidades Implementadas
- [x] Crear tareas con validaciones
- [x] Listar todas las tareas
- [x] Editar tareas existentes
- [x] Eliminar tareas
- [x] Categorizar por nivel de importancia (Bajo, Medio, Alto)
- [x] Filtrar por estado (Pendiente, En Progreso, Completada)
- [x] Manejo de errores de red

### 🔄 Funcionalidades Futuras (Fase 2)
- [ ] Subtareas
- [ ] Tiempo estimado
- [ ] Comentarios
- [ ] Sistema de categorías personalizadas
- [ ] Recordatorios

---

## 📚 Objetivo Educativo

Este proyecto tiene como propósito aprender:

1. **Arquitectura Full-Stack:** Separación de responsabilidades frontend/backend
2. **Diseño de Base de Datos:** Modelado relacional, normalizaciones
3. **API REST:** Diseño de endpoints, validaciones, manejo de errores
4. **Frontend Moderno:** Componentes React, manejo de estado, consumo de APIs
5. **Buenas Prácticas:** Clean Code, SOLID, Testing, Git workflow
6. **DevOps Básico:** Estructura de proyecto, deployment

---

## 📋 Requisitos Previos

### Software Requerido
- Visual Studio 2022 o superior
- .NET 8.0 SDK
- SQL Server 2019 o superior
- Node.js 18+ y npm
- VS Code (opcional)
- Git

### Instalación

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/ING-Cristopher/App_lista_de_tareas.git
   cd App_lista_de_tareas
   ```

2. **Configurar Backend (.NET):**
   ```bash
   cd backend
   # Restaurar dependencias
   dotnet restore
   
   # Aplicar migraciones a BD
   dotnet ef database update
   
   # Ejecutar servidor
   dotnet run
   ```
   La API estará disponible en: `http://localhost:5000`

3. **Configurar Frontend (React):**
   ```bash
   cd frontend
   # Instalar dependencias
   npm install
   
   # Ejecutar servidor de desarrollo
   npm start
   ```
   La app estará disponible en: `http://localhost:3000`

---

## 📁 Estructura del Proyecto

```
App_lista_de_tareas/
├── backend/                 # Código C# / .NET
│   ├── TodoApp.API/         # Controladores y endpoints
│   ├── TodoApp.Domain/      # Modelos de negocio
│   ├── TodoApp.Infrastructure/  # Contexto BD, repositorios
│   └── TodoApp.Tests/       # Tests unitarios
├── frontend/                # Código React
│   ├── src/
│   │   ├── components/      # Componentes React reutilizables
│   │   ├── pages/           # Páginas principales
│   │   ├── services/        # Servicios HTTP (llamadas a API)
│   │   └── App.jsx
│   └── package.json
└── docs/                    # Documentación técnica
    ├── ARQUITECTURA.md
    ├── API.md
    └── DB_DESIGN.md
```

---

## 🔌 API Endpoints (Especificación)

### Tareas

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/tareas` | Obtener todas las tareas |
| `GET` | `/api/tareas/{id}` | Obtener tarea por ID |
| `POST` | `/api/tareas` | Crear nueva tarea |
| `PUT` | `/api/tareas/{id}` | Actualizar tarea |
| `DELETE` | `/api/tareas/{id}` | Eliminar tarea |

Ver especificación completa en: `docs/API.md`

---

## 🗄️ Diseño de Base de Datos

La aplicación usa **SQL Server** con las siguientes tablas:

- **Tareas:** Almacena información de cada tarea
- **Estados:** Enum de estados (Pendiente, En Progreso, Completada)
- **Niveles de Importancia:** Enum de prioridades

Ver diagrama ER completo en: `docs/DB_DESIGN.md`

---

## 🧪 Testing

```bash
# Ejecutar tests
cd backend
dotnet test
```

---

## 📖 Documentación

- **[ARQUITECTURA.md](docs/ARQUITECTURA.md)** - Explicación de decisiones arquitectónicas
- **[API.md](docs/API.md)** - Especificación detallada de endpoints
- **[DB_DESIGN.md](docs/DB_DESIGN.md)** - Diseño y modelos de datos

---

## 🤝 Contribuciones

Este es un proyecto de aprendizaje personal. Para sugerencias o mejoras, abre un **Issue** o **Pull Request**.

---

## 📝 Licencia

MIT License - Ver archivo LICENSE

---

## 👨‍💻 Autor

**ING-Cristopher**
- GitHub: [@ING-Cristopher](https://github.com/ING-Cristopher)
- Aprendiendo: Full-Stack Development con C# y React

---

## 📝 Notas de Desarrollo

### Cambios Recientes
- *Proyecto inicializado*

### Problemas Conocidos
- *Ninguno registrado aún*

### Próximos Pasos
1. Diseñar base de datos
2. Crear API REST en .NET
3. Implementar frontend en React
4. Integración y testing


# 📋 Diseño de Base de Datos

## Introducción

Este documento describe la estructura de la base de datos SQL Server para la aplicación de gestión de tareas.

**Principios aplicados:**
- Normalización hasta 3NF (Tercera Forma Normal)
- Integridad referencial
- Índices en campos frecuentemente consultados

---

## 📊 Diagrama Entidad-Relación (ER)

```
┌──────────────────────────────────────┐
│          Tareas                      │
├──────────────────────────────────────┤
│ PK: Id                               │
│ Titulo (VARCHAR 255)                 │
│ Descripcion (TEXT)                   │
│ FechaLimite (DATE)                   │
│ IdEstado (FK)                        │
│ IdImportancia (FK)                   │
│ FechaCreacion (DATE)                 │
│ FechaActualizacion                   │
└──────────────────┬────────────────────┘
                   │
        ┌──────────┴──────────┐
        │                     │
        ▼                     ▼
┌──────────────────────┐  ┌──────────────────────┐
│     Estados          │  │   Importancias      │
├──────────────────────┤  ├──────────────────────┤
│ PK: Id               │  │ PK: Id               │
│ Nombre (VARCHAR)     │  │ Nombre (VARCHAR)     │
└──────────────────────┘  └──────────────────────┘
```

---

## 📝 Tablas Principales

### 1. **Tareas**

Almacena las tareas principales de la aplicación.

```sql
CREATE TABLE Tareas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Titulo VARCHAR(255) NOT NULL,
    Descripcion TEXT NULL,
    FechaLimite DATETIME NOT NULL,
    IdEstado INT NOT NULL,
    IdImportancia INT NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaActualizacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IdEstado) REFERENCES Estados(Id),
    FOREIGN KEY (IdImportancia) REFERENCES Importancias(Id),
    CONSTRAINT CHK_TituloLength CHECK (LEN(Titulo) >= 5)
);

-- Índices para optimización de consultas
CREATE INDEX IX_Tareas_IdEstado ON Tareas(IdEstado);
CREATE INDEX IX_Tareas_IdImportancia ON Tareas(IdImportancia);
CREATE INDEX IX_Tareas_FechaLimite ON Tareas(FechaLimite);
```

**Campos:**
- `Id`: Identificador único (Primary Key)
- `Titulo`: Título de la tarea (mínimo 5 caracteres)
- `Descripcion`: Descripción detallada (opcional)
- `FechaLimite`: Fecha límite de entrega
- `IdEstado`: Referencia a la tabla Estados
- `IdImportancia`: Referencia a la tabla Importancias
- `FechaCreacion`: Timestamp de creación
- `FechaActualizacion`: Timestamp de última modificación

---

### 2. **Estados**

Enum de estados posibles de una tarea.

```sql
CREATE TABLE Estados (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL UNIQUE
);

-- Insertar datos iniciales
INSERT INTO Estados (Nombre) VALUES ('Pendiente');
INSERT INTO Estados (Nombre) VALUES ('En Progreso');
INSERT INTO Estados (Nombre) VALUES ('Completada');
```

**Valores:**
- 1: Pendiente
- 2: En Progreso
- 3: Completada

---

### 3. **Importancias**

Enum de niveles de importancia.

```sql
CREATE TABLE Importancias (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL UNIQUE
);

-- Insertar datos iniciales
INSERT INTO Importancias (Nombre) VALUES ('Bajo');
INSERT INTO Importancias (Nombre) VALUES ('Medio');
INSERT INTO Importancias (Nombre) VALUES ('Alto');
```

**Valores:**
- 1: Bajo
- 2: Medio
- 3: Alto

---

## 🔄 Migraciones (Entity Framework)

Usaremos **Entity Framework Core Migrations** para versionamiento de BD.

```bash
# Crear una nueva migración después de cambiar modelos
dotnet ef migrations add NombreMigracion -p TodoApp.Infrastructure -s TodoApp.API

# Aplicar migraciones a la BD
dotnet ef database update -p TodoApp.Infrastructure -s TodoApp.API

# Ver migraciones pendientes
dotnet ef migrations list
```

---

## 🌱 Seed Data

Datos iniciales para ambiente de desarrollo:

```sql
-- Estados
INSERT INTO Estados (Nombre) VALUES ('Pendiente');
INSERT INTO Estados (Nombre) VALUES ('En Progreso');
INSERT INTO Estados (Nombre) VALUES ('Completada');

-- Importancias
INSERT INTO Importancias (Nombre) VALUES ('Bajo');
INSERT INTO Importancias (Nombre) VALUES ('Medio');
INSERT INTO Importancias (Nombre) VALUES ('Alto');

-- Tareas de ejemplo
INSERT INTO Tareas (Titulo, Descripcion, FechaLimite, IdEstado, IdImportancia)
VALUES 
  ('Diseñar base de datos', 'Crear ER diagram y tablas', '2026-09-15', 2, 3),
  ('Implementar API', 'Endpoints CRUD en .NET', '2026-09-20', 1, 3),
  ('Desarrollar frontend', 'Componentes React', '2026-09-25', 1, 2);
```

---

## 📈 Decisiones de Diseño

### ¿Por qué estas tablas?

1. **Estados e Importancias como tablas separadas:**
   - **Ventaja:** Fácil de extender en el futuro
   - **Ventaja:** Mantiene integridad referencial
   - **Desventaja:** Más queries (pero mínimo impacto)

2. **Timestamps automáticos:**
   - Para auditoría y debugging
   - Permite ver cuándo se creó/modificó cada tarea

3. **Constraint en Titulo (mínimo 5 caracteres):**
   - Validación a nivel de BD (defense in depth)
   - Aunque también validaremos en API y frontend

4. **Índices en Foreign Keys:**
   - Las queries filtran por estado e importancia frecuentemente
   - Mejora performance en JOINs

---

## 🔐 Consideraciones de Seguridad

- ✅ Integridad referencial con FOREIGN KEY
- ✅ Constraints para validaciones a nivel BD
- ⚠️ TODO: Auditoría de cambios (Fase 2)
- ⚠️ TODO: Encriptación de datos sensibles (Fase 3)

---

## 📝 Próximas Fases

**Fase 2:**
- [ ] Tabla de Subtareas (1-a-muchos con Tareas)
- [ ] Tabla de Categorías personalizadas
- [ ] Tabla de Comentarios

**Fase 3:**
- [ ] Tabla de Usuarios (autenticación)
- [ ] Tabla de Auditoría
- [ ] Tabla de Recordatorios

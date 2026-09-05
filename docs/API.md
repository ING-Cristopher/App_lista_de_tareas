# 🔌 Especificación de API REST

## Base URL

```
Desarrollo:  http://localhost:5000/api
Producción: https://api.todoapp.com/api
```

---

## 📋 Tareas

### 1. Obtener todas las tareas

**Endpoint:** `GET /tareas`

**Query Parameters (Opcionales):**
```
GET /tareas?estado=1&importancia=3&limite=10&pagina=1
```

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `estado` | int | Filtrar por estado (1, 2, 3) |
| `importancia` | int | Filtrar por importancia (1, 2, 3) |
| `limite` | int | Tareas por página (default: 10) |
| `pagina` | int | Número de página (default: 1) |

**Response 200 OK:**
```json
{
  "data": [
    {
      "id": 1,
      "titulo": "Diseñar base de datos",
      "descripcion": "Crear ER diagram y tablas",
      "fechaLimite": "2026-09-15T00:00:00Z",
      "estado": {
        "id": 2,
        "nombre": "En Progreso"
      },
      "importancia": {
        "id": 3,
        "nombre": "Alto"
      },
      "fechaCreacion": "2026-09-05T06:30:00Z",
      "fechaActualizacion": "2026-09-05T06:30:00Z"
    }
  ],
  "total": 1,
  "pagina": 1,
  "totalPaginas": 1
}
```

**Response 500 Error:**
```json
{
  "error": "Error al obtener tareas",
  "detalles": "Conexión a BD fallida"
}
```

---

### 2. Obtener una tarea por ID

**Endpoint:** `GET /tareas/{id}`

**Parámetros:**
| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `id` | int | ID de la tarea |

**Response 200 OK:**
```json
{
  "id": 1,
  "titulo": "Diseñar base de datos",
  "descripcion": "Crear ER diagram y tablas",
  "fechaLimite": "2026-09-15T00:00:00Z",
  "estado": {
    "id": 2,
    "nombre": "En Progreso"
  },
  "importancia": {
    "id": 3,
    "nombre": "Alto"
  },
  "fechaCreacion": "2026-09-05T06:30:00Z",
  "fechaActualizacion": "2026-09-05T06:30:00Z"
}
```

**Response 404 Not Found:**
```json
{
  "error": "Tarea no encontrada",
  "id": 999
}
```

---

### 3. Crear una tarea

**Endpoint:** `POST /tareas`

**Body (JSON):**
```json
{
  "titulo": "Implementar autenticación",
  "descripcion": "Agregar JWT al API",
  "fechaLimite": "2026-09-20T00:00:00Z",
  "idEstado": 1,
  "idImportancia": 3
}
```

**Validaciones:**
- `titulo`: Requerido, mínimo 5 caracteres, máximo 255
- `descripcion`: Opcional, máximo 5000 caracteres
- `fechaLimite`: Requerida, debe ser en el futuro
- `idEstado`: Requerido, debe existir en BD
- `idImportancia`: Requerido, debe existir en BD

**Response 201 Created:**
```json
{
  "id": 3,
  "titulo": "Implementar autenticación",
  "descripcion": "Agregar JWT al API",
  "fechaLimite": "2026-09-20T00:00:00Z",
  "estado": {
    "id": 1,
    "nombre": "Pendiente"
  },
  "importancia": {
    "id": 3,
    "nombre": "Alto"
  },
  "fechaCreacion": "2026-09-05T06:35:00Z",
  "fechaActualizacion": "2026-09-05T06:35:00Z"
}
```

**Response 400 Bad Request:**
```json
{
  "error": "Validación fallida",
  "errores": [
    {
      "campo": "titulo",
      "mensaje": "Mínimo 5 caracteres requeridos"
    },
    {
      "campo": "fechaLimite",
      "mensaje": "La fecha debe ser en el futuro"
    }
  ]
}
```

---

### 4. Actualizar una tarea

**Endpoint:** `PUT /tareas/{id}`

**Parámetros:**
| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `id` | int | ID de la tarea |

**Body (JSON):** (Todos los campos opcionales)
```json
{
  "titulo": "Implementar autenticación JWT",
  "descripcion": "Agregar JWT al API con refresh tokens",
  "fechaLimite": "2026-09-21T00:00:00Z",
  "idEstado": 2,
  "idImportancia": 2
}
```

**Response 200 OK:**
```json
{
  "id": 3,
  "titulo": "Implementar autenticación JWT",
  "descripcion": "Agregar JWT al API con refresh tokens",
  "fechaLimite": "2026-09-21T00:00:00Z",
  "estado": {
    "id": 2,
    "nombre": "En Progreso"
  },
  "importancia": {
    "id": 2,
    "nombre": "Medio"
  },
  "fechaCreacion": "2026-09-05T06:35:00Z",
  "fechaActualizacion": "2026-09-05T06:40:00Z"
}
```

**Response 404 Not Found:**
```json
{
  "error": "Tarea no encontrada",
  "id": 999
}
```

---

### 5. Eliminar una tarea

**Endpoint:** `DELETE /tareas/{id}`

**Parámetros:**
| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `id` | int | ID de la tarea |

**Response 204 No Content:**
```
(Sin body)
```

**Response 404 Not Found:**
```json
{
  "error": "Tarea no encontrada",
  "id": 999
}
```

---

## 📊 Códigos de Estado HTTP

| Código | Significado | Cuándo |
|--------|-------------|--------|
| 200 | OK | GET exitoso, PUT exitoso |
| 201 | Created | POST exitoso |
| 204 | No Content | DELETE exitoso |
| 400 | Bad Request | Validación fallida |
| 404 | Not Found | Recurso no existe |
| 500 | Internal Server Error | Error del servidor |
| 503 | Service Unavailable | API no disponible |

---

## 🔄 Manejo de Errores

### Estructura de error estándar:

```json
{
  "error": "Descripción breve del error",
  "detalles": "Descripción más detallada (opcional)",
  "errores": [
    {
      "campo": "nombre_del_campo",
      "mensaje": "Qué está mal con este campo"
    }
  ],
  "timestamp": "2026-09-05T06:40:00Z",
  "traceId": "abc123xyz"
}
```

---

## 🔌 Headers

**Request:**
```
Content-Type: application/json
Accept: application/json
```

**Response:**
```
Content-Type: application/json
X-Total-Count: 1
X-Page: 1
X-Page-Size: 10
```

---

## 📝 Ejemplos con cURL

### Obtener todas las tareas:
```bash
curl -X GET http://localhost:5000/api/tareas
```

### Crear una tarea:
```bash
curl -X POST http://localhost:5000/api/tareas \
  -H "Content-Type: application/json" \
  -d '{
    "titulo": "Nueva tarea",
    "descripcion": "Descripción",
    "fechaLimite": "2026-09-20T00:00:00Z",
    "idEstado": 1,
    "idImportancia": 2
  }'
```

---

## 🚨 Errores Comunes

### "Titulo debe tener minimo 5 caracteres"
- Asegúrate de que el título tiene al menos 5 caracteres

### "IdEstado no existe"
- Valores válidos: 1 (Pendiente), 2 (En Progreso), 3 (Completada)

### "IdImportancia no existe"
- Valores válidos: 1 (Bajo), 2 (Medio), 3 (Alto)

### "Error de conexión"
- Verifica que la API está corriendo: `http://localhost:5000`

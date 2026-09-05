# 🚀 Guía de Configuración del Proyecto

## Requisitos Previos

### Software Requerido

- **Visual Studio 2022** (o superior)
  - Con workload: "ASP.NET and web development"
  - Link: https://visualstudio.microsoft.com/

- **.NET 8.0 SDK**
  - Link: https://dotnet.microsoft.com/download
  - Verificar: `dotnet --version`

- **SQL Server 2019** (o superior)
  - Express Edition es suficiente: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
  - O usar **LocalDB** (incluido con Visual Studio)

- **Node.js 18+** y **npm**
  - Link: https://nodejs.org/
  - Verificar: `node --version` y `npm --version`

- **Git**
  - Link: https://git-scm.com/
  - Verificar: `git --version`

- **VS Code** (Opcional, pero recomendado para frontend)
  - Link: https://code.visualstudio.com/

---

## 1️⃣ Clonar el Repositorio

```bash
git clone https://github.com/ING-Cristopher/App_lista_de_tareas.git
cd App_lista_de_tareas
```

---

## 2️⃣ Configurar Backend (.NET)

### Paso 1: Abrir en Visual Studio

1. Abre **Visual Studio 2022**
2. Click en "Open a project or solution"
3. Navega a `App_lista_de_tareas/backend`
4. Selecciona el archivo `.sln` (solución)

### Paso 2: Configurar la Cadena de Conexión

**Archivo:** `backend/TodoApp.API/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TodoApp;Trusted_Connection=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### Paso 3: Restaurar Dependencias

En la **Consola del Administrador de Paquetes** de Visual Studio:

```powershell
cd backend
dotnet restore
```

### Paso 4: Crear la Base de Datos

```powershell
# Aplicar migraciones
dotnet ef database update -p TodoApp.Infrastructure -s TodoApp.API
```

### Paso 5: Ejecutar el Backend

```powershell
dotnet run -p TodoApp.API
```

✅ Deberías ver algo como:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
```

Visita: http://localhost:5000/swagger (Swagger UI para probar API)

---

## 3️⃣ Configurar Frontend (React)

### Paso 1: Abrir Terminal

1. Abre **VS Code** o **Git Bash**
2. Navega a la carpeta del proyecto:
   ```bash
   cd App_lista_de_tareas/frontend
   ```

### Paso 2: Instalar Dependencias

```bash
npm install
```

⏳ Esto puede tomar 2-5 minutos la primera vez.

### Paso 3: Crear archivo de Configuración

**Archivo:** `frontend/.env.local`

```
REACT_APP_API_URL=http://localhost:5000/api
REACT_APP_ENV=development
```

### Paso 4: Ejecutar Frontend

```bash
npm start
```

✅ Deberías abrir automáticamente: http://localhost:3000

---

## ✅ Verificación de Configuración

### Checklist:

- [ ] Visual Studio abrió el proyecto `.sln`
- [ ] `appsettings.json` tiene conexión a BD correcta
- [ ] `dotnet ef database update` ejecutó sin errores
- [ ] Backend corre en http://localhost:5000
- [ ] Swagger UI es accesible en http://localhost:5000/swagger
- [ ] Frontend instaló dependencias con `npm install`
- [ ] `.env.local` apunta a http://localhost:5000/api
- [ ] Frontend corre en http://localhost:3000
- [ ] Puedes ver la interfaz de usuario en el navegador

---

## 🔧 Solución de Problemas

### Error: "Cannot connect to database"

**Problema:** La cadena de conexión es incorrecta o SQL Server no está corriendo.

**Solución:**
1. Verifica que SQL Server está corriendo
2. Abre SQL Server Management Studio
3. Intenta conectarte manualmente
4. Actualiza la cadena en `appsettings.json`

### Error: "Port 5000 already in use"

**Problema:** Otro proceso usa el puerto 5000.

**Solución:**
```bash
# Encuentra el proceso que usa el puerto
netstat -ano | findstr :5000

# O cambia el puerto en Program.cs
app.Run("http://localhost:5001");
```

### Error: "npm: command not found"

**Problema:** Node.js no está instalado o no está en PATH.

**Solución:**
1. Descarga Node.js desde https://nodejs.org/
2. Instálalo y reinicia la terminal
3. Verifica: `node --version`

### Error: "Module not found" en React

**Problema:** Las dependencias no se instalaron correctamente.

**Solución:**
```bash
cd frontend
rm -rf node_modules package-lock.json
npm install
```

---

## 📂 Estructura de Carpetas Después de Setup

```
App_lista_de_tareas/
├── backend/
│   ├── TodoApp.API/
│   ├── TodoApp.Domain/
│   ├── TodoApp.Infrastructure/
│   │   └── Migrations/
│   └── TodoApp.Tests/
│
├── frontend/
���   ├── src/
│   ├── public/
│   ├── node_modules/
│   ├── package.json
│   └── .env.local
│
└── docs/
    ├── DB_DESIGN.md
    ├── ARQUITECTURA.md
    ├── API.md
    └── SETUP_GUIDE.md
```

---

## 🚀 Workflow Típico de Desarrollo

### Terminal 1 - Backend
```bash
cd backend
dotnet run -p TodoApp.API
```

### Terminal 2 - Frontend
```bash
cd frontend
npm start
```

### Terminal 3 - Git (Opcional)
```bash
cd App_lista_de_tareas
# Aquí ejecutas comandos git
```

---

## 📚 Próximos Pasos

1. ✅ Setup completado
2. → Leer `docs/DB_DESIGN.md` (Entender base de datos)
3. → Leer `docs/ARQUITECTURA.md` (Entender estructura)
4. → Leer `docs/API.md` (Especificación de endpoints)
5. → Empezar a codificar (Backend primero)

---

## 💡 Tips Útiles

- **Live Reload:** React y .NET ambos tienen hot reload habilitado
- **API Testing:** Usa Swagger en http://localhost:5000/swagger
- **Browser DevTools:** F12 en Chrome para ver requests/responses
- **Git Commits:** Haz commits pequeños y frecuentes
- **Branch Strategy:** Usa feature branches: `git checkout -b feature/nombre`

---

## 🆘 Ayuda Adicional

- **Documentación .NET:** https://docs.microsoft.com/dotnet/
- **Documentación React:** https://react.dev/
- **Stack Overflow:** https://stackoverflow.com/
- **GitHub Issues:** Abre un issue en el repo

# RegistroEstudiantil

Aplicacion web para registro e inscripcion de estudiantes, desarrollada como prueba tecnica. El sistema permite registrar usuarios, crear su perfil de estudiante, gestionar inscripciones por periodo y consultar informacion relacionada con companeros y otros estudiantes.

## Stack Tecnologico

- `.NET 9`
- `ASP.NET Core Web API`
- `Entity Framework Core`
- `SQL Server`
- `Angular 19`
- `Angular Material`
- `JWT`
- `BCrypt`

## Arquitectura

La solucion esta organizada por capas:

- [RegistroEstudiantil.Domain](C:\Inter_Test_Ultimo\Inter_Test\RegistroEstudiantil.Domain): entidades del dominio
- [RegistroEstudiantil.Application](C:\Inter_Test_Ultimo\Inter_Test\RegistroEstudiantil.Application): DTOs, interfaces y servicios de aplicacion
- [RegistroEstudiantil.Infrastructure](C:\Inter_Test_Ultimo\Inter_Test\RegistroEstudiantil.Infrastructure): persistencia, repositorios, `UnitOfWork`, seguridad y `DbContext`
- [RegistroEstudiantil.Server](C:\Inter_Test_Ultimo\Inter_Test\RegistroEstudiantil.Server): API, configuracion, controladores y middleware
- [RegistroEstudiantil.Client](C:\Inter_Test_Ultimo\Inter_Test\RegistroEstudiantil.Client): frontend en Angular

## Funcionalidades Principales

- Registro en linea de usuario y estudiante en una sola operacion
- Inicio de sesion con JWT
- Consulta y actualizacion del perfil del estudiante
- Inscripcion de materias por periodo
- Validacion de reglas de negocio:
  - maximo 3 materias por periodo
  - no repetir profesor en el mismo periodo
  - no repetir materia en el mismo periodo
- Consulta de companeros por clase
- Consulta de registros de otros estudiantes

## Requisitos Previos

- `.NET SDK 9`
- `Node.js 20+`
- `SQL Server`
- `Visual Studio 2022` o `VS Code`

## Estructura Del Repositorio

```text
RegistroEstudiantil.sln
RegistroEstudiantil.Application/
RegistroEstudiantil.Client/
RegistroEstudiantil.Domain/
RegistroEstudiantil.Infrastructure/
RegistroEstudiantil.Server/
```

## Configuracion

Antes de ejecutar el proyecto, revisa la configuracion del backend en los archivos de entorno:

- [appsettings.json](C:\Inter_Test_Ultimo\Inter_Test\RegistroEstudiantil.Server\appsettings.json)
- [appsettings.Development.json](C:\Inter_Test_Ultimo\Inter_Test\RegistroEstudiantil.Server\appsettings.Development.json)

Debes definir al menos:

- cadena de conexion a SQL Server
- `JwtKey`
- `JwtIssuer`
- `JwtAudience`
- `JwtExpireMinutes`
- `origenesPermitidos`

## Ejecucion Del Proyecto

### 1. Restaurar dependencias del frontend

Desde [RegistroEstudiantil.Client](C:\Inter_Test_Ultimo\Inter_Test\RegistroEstudiantil.Client):

```powershell
npm install
```

### 2. Restaurar dependencias del backend

Desde la raiz del repositorio:

```powershell
dotnet restore RegistroEstudiantil.sln
```

### 3. Aplicar migraciones

```powershell
dotnet ef database update --project RegistroEstudiantil.Infrastructure --startup-project RegistroEstudiantil.Server
```

### 4. Ejecutar la solucion

Opcion 1, desde Visual Studio:

- abrir [RegistroEstudiantil.sln](C:\Inter_Test_Ultimo\Inter_Test\RegistroEstudiantil.sln)
- establecer varios proyectos de inicio si deseas levantar cliente y servidor

Opcion 2, por consola:

Backend:

```powershell
dotnet run --project RegistroEstudiantil.Server
```

Frontend:

```powershell
npm start
```

## Swagger

Cuando el backend este en ejecucion, Swagger queda disponible en la URL que exponga la API, normalmente bajo `/swagger`.

## Comandos Utiles

Crear una migracion:

```powershell
dotnet ef migrations add NombreMigracion --project RegistroEstudiantil.Infrastructure --startup-project RegistroEstudiantil.Server --output-dir Migrations
```

Actualizar base de datos:

```powershell
dotnet ef database update --project RegistroEstudiantil.Infrastructure --startup-project RegistroEstudiantil.Server
```

Validar TypeScript del cliente:

```powershell
npx tsc -p tsconfig.app.json --noEmit
```

## Estado Actual

El proyecto ya incluye:

- backend y frontend integrados
- validaciones de negocio en inscripciones
- mejoras de UI/UX en pantallas principales

## Documentacion

La documentacion tecnica detallada puede ir en un archivo separado, por ejemplo:



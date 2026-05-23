# Evently — Plataforma de Actividades de Ocio

> Aplicación web para centralizar y gestionar actividades de ocio en un único entorno accesible y fácil de usar.

---

## Equipo de desarrollo

| Nombre                 | Backend                                | Frontend                                      |
| ---------------------- | -------------------------------------- | --------------------------------------------- |
| Mykyta Vavulin         | Autenticación, Actividades, Categorías | Actividades, Detalle, Panel Admin             |
| Raquel Blázquez Corral | Carrito, Pedidos, Clientes, Estados    | Login, Registro, Perfil, Carrito, Mis Pedidos |

---

## Descripción del proyecto

**Evently** es una aplicación web que funciona como un escaparate digital donde los usuarios pueden:

- Consultar actividades de ocio organizadas por categorías
- Acceder a información detallada de cada actividad
- Gestionar un carrito de actividades (almacenado localmente en el navegador)
- Confirmar pedidos o reservas de actividades
- Gestionar sus datos personales

La aplicación distingue entre tres perfiles de usuario: **usuario anónimo**, **usuario registrado** y **administrador**.

---

## Estructura del repositorio

```
Evently/
├── backend/
│   └── Evently/
│       ├── Evently.sln
│       └── Evently.API/
│           ├── Controllers/   # Endpoints de la API
│           ├── Data/          # DbContext y seed de datos iniciales
│           ├── DTOs/          # Objetos de transferencia de datos
│           ├── Migrations/    # Historial de migraciones de EF Core
│           ├── Models/        # Modelos de la base de datos
│           └── Services/      # Lógica de negocio
└── frontend/
    └── Evently.Web/
        ├── Layout/        # Layout principal con navbar y drawer
        ├── Pages/         # Páginas de la aplicación
        │   ├── Home.razor
        │   ├── Actividades.razor
        │   ├── DetalleActividad.razor
        │   ├── Carrito.razor
        │   ├── Login.razor
        │   ├── Registro.razor
        │   ├── Perfil.razor
        │   ├── MisPedidos.razor
        │   ├── CompletarPerfil.razor
        │   ├── Contacto.razor
        │   ├── SobreNosotros.razor
        │   ├── Terminos.razor
        │   └── Admin/
        │       ├── AdminDashboard.razor
        │       ├── AdminActividades.razor
        │       ├── AdminCategorias.razor
        │       ├── AdminPedidos.razor
        │       ├── AdminEstados.razor
        │       ├── AdminClientes.razor
        │       └── AdminUsuarios.razor
        ├── Services/      # Servicios para llamadas a la API
        └── Models/        # Modelos del frontend
```

---

## Tecnologías utilizadas

### Backend

| Tecnología                    | Versión | ¿Por qué la usamos?                                                                                                                                           |
| ----------------------------- | ------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **ASP.NET Core Web API**      | .NET 9  | Framework robusto de Microsoft para crear APIs REST. Elegido por su rendimiento, integración nativa con C# y compatibilidad con Blazor en el frontend         |
| **Entity Framework Core**     | 9.x     | ORM que nos permite trabajar con la base de datos usando C# en lugar de SQL manual. Facilita las migraciones y el mantenimiento                               |
| **Npgsql EF Core PostgreSQL** | 9.x     | Proveedor de PostgreSQL para EF Core. Necesario para conectar nuestra API con la base de datos en Neon.tech                                                   |
| **JWT Bearer**                | 9.x     | Sistema de autenticación basado en tokens. Permite controlar el acceso según el rol del usuario (usuario / administrador) sin guardar sesiones en el servidor |
| **BCrypt.Net-Next**           | 4.x     | Librería para cifrar contraseñas. Nunca se guardan contraseñas en texto plano en la base de datos                                                             |
| **Swagger / OpenAPI**         | 9.x     | Documentación automática de la API. Permite probar todos los endpoints directamente desde el navegador sin herramientas externas                              |

### Base de datos

| Tecnología     | ¿Por qué la usamos?                                                                                                                                 |
| -------------- | --------------------------------------------------------------------------------------------------------------------------------------------------- |
| **PostgreSQL** | Base de datos relacional potente y gratuita. Compatible con todas las relaciones definidas en el modelo entidad-relación del proyecto               |
| **Neon.tech**  | Servicio de hosting gratuito para PostgreSQL en la nube. No requiere instalación local y permite que todo el equipo comparta la misma base de datos |

### Frontend

| Tecnología             | Versión | ¿Por qué la usamos?                                                                                                                                                                             |
| ---------------------- | ------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Blazor WebAssembly** | .NET 9  | Framework de Microsoft que permite crear interfaces web con C# en lugar de JavaScript. Elegido por su integración natural con el backend ASP.NET Core y porque todo el equipo ya trabaja con C# |
| **MudBlazor**          | 9.x     | Librería de componentes UI para Blazor. Proporciona componentes listos y con buen diseño visual como botones, tablas, formularios, navegación y mucho más, sin necesidad de CSS personalizado   |

### Herramientas

| Herramienta            | Uso                                                        |
| ---------------------- | ---------------------------------------------------------- |
| **Visual Studio 2022** | Entorno de desarrollo principal para el backend y frontend |
| **VS Code**            | Editor auxiliar y gestión de Git                           |
| **Git + GitHub**       | Control de versiones y colaboración en equipo              |

---

## Modelo de datos

El sistema cuenta con **10 tablas** principales:

| Tabla               | Descripción                                                           |
| ------------------- | --------------------------------------------------------------------- |
| `Usuarios`          | Cuentas de acceso al sistema (email, password, rol)                   |
| `Clientes`          | Datos personales para gestionar pedidos (nombre, dirección, teléfono) |
| `Categorias`        | Clasificación de las actividades                                      |
| `Actividades`       | Actividades de ocio disponibles en el escaparate                      |
| `ImagenesActividad` | Imágenes asociadas a cada actividad, con orden de visualización       |
| `Estados`           | Estados posibles de un pedido (Pendiente, Confirmado, Cancelado)      |
| `Pedidos`           | Reservas confirmadas por los usuarios                                 |
| `DetallesPedido`    | Líneas de actividades dentro de cada pedido (relación N:M)            |
| `Comentarios`       | Comentarios de usuarios sobre actividades                             |
| `Valoraciones`      | Puntuaciones (1–5) de usuarios sobre actividades (una por usuario)    |

---

## Perfiles de usuario

| Perfil            | Descripción               | Funciones principales                                                               |
| ----------------- | ------------------------- | ----------------------------------------------------------------------------------- |
| **Anónimo**       | Usuario sin cuenta        | Ver escaparate, filtrar categorías, ver detalle                                     |
| **Registrado**    | Usuario con cuenta activa | Todo lo anterior + carrito, confirmar pedidos, gestionar datos, comentar y valorar  |
| **Administrador** | Gestión interna           | CRUD de actividades, categorías, pedidos, estados, clientes, usuarios e imágenes    |

---

## Sistema de autenticación

Usamos **JWT (JSON Web Tokens)**:

```
1. Usuario hace login → API verifica credenciales con BCrypt
2. API devuelve un TOKEN
3. Usuario envía el TOKEN en cada petición
4. API verifica el TOKEN y permite o deniega el acceso según el ROL
```

Los tokens expiran en **8 horas**.

---

## Mapa de navegación

```
HOME
├── ACTIVIDADES
│   ├── Filtrar por categoría
│   └── DETALLE DE ACTIVIDAD → Añadir al carrito, comentar y valorar
├── CARRITO
│   └── CONFIRMAR PEDIDO
└── CUENTA
    ├── Login / Registro
    ├── PERFIL (mis datos)
    └── MIS PEDIDOS (historial)

ADMIN (solo administrador)
├── Dashboard
├── Gestión de Actividades (con imágenes)
├── Gestión de Categorías
├── Gestión de Pedidos
├── Gestión de Estados
├── Gestión de Clientes
└── Gestión de Usuarios
```

---

## Cómo ejecutar localmente

### Prerrequisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 con la carga de trabajo **ASP.NET y desarrollo web**

### 1. Clonar el repositorio

```bash
git clone https://github.com/nikita9992/Evently
cd Evently
```

### 2. Abrir la solución

Abrir el archivo `backend/Evently/Evently.sln` en Visual Studio 2022.

### 3. Configurar las credenciales secretas

El proyecto necesita dos valores que **no están incluidos en el repositorio**: la cadena de conexión a la base de datos y la clave JWT. Hay que proporcionarlos localmente usando los **Secretos de usuario** de .NET, que se almacenan fuera del repositorio y nunca se suben a Git.

**En Visual Studio:**

1. En el **Explorador de Soluciones**, click derecho sobre el proyecto `Evently.API`
2. Seleccionar **Administrar secretos de usuario**
3. Visual Studio abrirá un archivo `secrets.json` vinculado al proyecto
4. Añadir el contenido de `secret.txt`



| Clave                               | Descripción                                                                                        |
| ----------------------------------- | -------------------------------------------------------------------------------------------------- |
| `ConnectionStrings:ConexionEvently` | Cadena de conexión a PostgreSQL. Si usas Neon.tech, la obtienes desde el panel de tu proyecto      |
| `Jwt:Clave`                         | Clave secreta para firmar los tokens JWT — cualquier texto largo (mínimo 32 caracteres)            |

El resto de valores (`Jwt:Emisor`, `Jwt:Audiencia`, `Jwt:ExpiracionHoras`) ya están definidos en `appsettings.json` y no hace falta tocarlos.

### 4. Configurar proyectos de inicio

Para arrancar el backend y el frontend a la vez:

1. Click derecho sobre la solución → **Establecer proyectos de inicio...**
2. Seleccionar **Varios proyectos de inicio**
3. Marcar `Evently.API` y `Evently.Web` como **Iniciar**

### 5. Ejecutar

Pulsar **F5**. Las migraciones de base de datos se aplican automáticamente al arrancar la API — no es necesario ejecutar `Update-Database` manualmente. Si es la primera ejecución, también se insertan datos de prueba (categorías, actividades y estados iniciales).

La documentación Swagger estará disponible en `https://localhost:7174` mientras el entorno sea Development.

---

## Ramas:

- `main` → código estable y revisado
- `develop` → integración de ambas partes
- `Mykyta` → rama de trabajo de Mykyta
- `raquel` → rama de trabajo de Raquel

_Proyecto Final Evently — DAW-M | Mare Nostrum Alicante | 2026_

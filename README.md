# Evently — Plataforma de Actividades de Ocio

> Aplicación web para centralizar y gestionar actividades de ocio en un único entorno accesible y fácil de usar.

---

## Equipo de desarrollo

| Nombre                 | Rol principal                                    |
| ---------------------- | ------------------------------------------------ |
| Mykyta Vavulin         | Backend — Autenticación, Actividades, Categorías |
| Raquel Blázquez Corral | Backend — Carrito, Pedidos, Clientes, Estados    |

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
evently/
├── backend/          # API REST con ASP.NET Core
│   └── Evently.API/
│       ├── Controllers/   # Endpoints de la API
│       ├── Data/          # DbContext y configuración de BD
│       ├── DTOs/          # Objetos de transferencia de datos
│       ├── Models/        # Modelos de la base de datos
│       └── Services/      # Lógica de negocio
└── frontend/         # Interfaz de usuario con Blazor (próximamente)
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

| Tecnología             | ¿Por qué la usamos?                                                                                                                                   |
| ---------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Blazor WebAssembly** | Framework de Microsoft que permite crear interfaces web con C# en lugar de JavaScript. Elegido por su integración natural con el backend ASP.NET Core |
| **MudBlazor**          | Librería de componentes UI para Blazor. Proporciona componentes listos y con buen diseño visual                                                       |

### Herramientas

| Herramienta            | Uso                                             |
| ---------------------- | ----------------------------------------------- |
| **Visual Studio 2022** | Entorno de desarrollo principal para el backend |
| **VS Code**            | Editor auxiliar y gestión de Git                |
| **Git + GitHub**       | Control de versiones y colaboración en equipo   |

---

## Modelo de datos

El sistema cuenta con **7 tablas** principales:

| Tabla            | Descripción                                                           |
| ---------------- | --------------------------------------------------------------------- |
| `Usuarios`       | Cuentas de acceso al sistema (email, password, rol)                   |
| `Clientes`       | Datos personales para gestionar pedidos (nombre, dirección, teléfono) |
| `Categorias`     | Clasificación de las actividades                                      |
| `Actividades`    | Actividades de ocio disponibles en el escaparate                      |
| `Estados`        | Estados posibles de un pedido (Confirmado, Cancelado)                 |
| `Pedidos`        | Reservas confirmadas por los usuarios                                 |
| `DetallesPedido` | Líneas de actividades dentro de cada pedido (relación N:M)            |

> **Nota importante:** El carrito **no se guarda en la base de datos**. Se almacena en el `localStorage` del navegador. Solo se crea un `Pedido` en la base de datos cuando el usuario confirma la compra.

---

## Perfiles de usuario

| Perfil            | Descripción               | Funciones principales                                          |
| ----------------- | ------------------------- | -------------------------------------------------------------- |
| **Anónimo**       | Usuario sin cuenta        | Ver escaparate, filtrar categorías, ver detalle                |
| **Registrado**    | Usuario con cuenta activa | Todo lo anterior + carrito, confirmar pedidos, gestionar datos |
| **Administrador** | Gestión interna           | CRUD de actividades, categorías, pedidos, estados y clientes   |

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

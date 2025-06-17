# DentAssist - Sistema de Gestión Clínica Odontológica 🦷

DentAssist es una aplicación web desarrollada con ASP.NET Core MVC y Entity Framework Core para la gestión completa de una clínica dental. Permite registrar pacientes, odontólogos, asignar citas, llevar el control de tratamientos y administrar el acceso según el rol del usuario.

---

## ⚙️ Tecnologías utilizadas

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server LocalDB
- Bootstrap 5
- HTML + CSS + Razor
- C#

---

## 🛠️ Instalación y ejecución local

### Requisitos

- Visual Studio 2022
- .NET 8 SDK
- SQL Server

### Pasos para ejecutar el proyecto:

1. Clona el repositorio:

   ```bash
   git clone https://github.com/P1POX/DentAssist.git
   ```

2. Abre la solución en Visual Studio.

3. Ejecuta el siguiente comando en la Consola del Administrador de Paquetes:

   ```bash
   Update-Database
   ```

4. Presiona **F5** o haz clic en "Iniciar" para ejecutar el sistema.

---

## 👤 Roles y funcionalidades

### Administrador

- Registrar y editar odontólogos.
- Ver citas y tratamientos.
- Crear usuarios con distintos roles.

### Recepcionista

- Registrar pacientes.
- Asignar y editar citas.
- Consultar historial de pacientes.

### Odontólogo

- Ver sus propias citas semanales.
- Registrar tratamientos y observaciones clínicas.
- Visualizar pacientes asignados.

---

## 🔐 Usuarios de prueba

| Rol           | Usuario                | Contraseña     |
|---------------|------------------------|----------------|
| Administrador | admin@sonrisaplena.com | Pass123.       |

> Puedes cambiar estas credenciales o registrar nuevos usuarios desde la base de datos o el sistema si está habilitado.

---

## ✅ Funcionalidades principales

- Registro de pacientes, odontólogos y citas.
- Vista semanal de citas para odontólogos.
- Gestión de tratamientos por paciente.
- Validaciones en formularios.
- Control de acceso por roles.

---

## 📦 Estructura del proyecto

```
/Controllers       -> Controladores MVC
/Models            -> Entidades y ViewModels
/Views             -> Vistas Razor por entidad
/Data              -> Contexto de base de datos
/wwwroot           -> Recursos estáticos (CSS, JS)
/Migrations        -> Migraciones EF Core
```
----

# Manual de Usuario - Sistema DentAssist

Este manual describe cómo utilizar las funcionalidades principales del sistema DentAssist, desde el acceso al sistema hasta la gestión de citas y pacientes.

## Índice

1. [Iniciar sesión](#iniciar-sesión)
2. [Crear especialidad (Administrador)](#crear-especialidad-administrador)
3. [Crear odontólogo (Administrador)](#crear-odontólogo-administrador)
4. [Crear recepcionista (Administrador)](#crear-recepcionista-administrador)
5. [Crear paciente (Recepcionista)](#crear-paciente-recepcionista)
6. [Crear cita (Recepcionista)](#crear-cita-recepcionista)

---

## Iniciar sesión

1. Dirígete a la página de inicio de sesión.
2. Ingresa el correo y la contraseña proporcionados.
3. Haz clic en **Iniciar sesión**.

![img](https://i.imgur.com/MQR429K.png)

---

## Crear especialidad (Administrador)

1. Accede al panel de administrador.
2. Navega a la sección **Especialidades**.
3. Haz clic en **Crear nueva especialidad**.
4. Completa el formulario con el nombre de la especialidad.
5. Haz clic en **Crear**.

![img](https://i.imgur.com/h5cBui9.png)

---

## Crear odontólogo (Administrador)

1. Navega a la sección **Odontólogos**.
2. Haz clic en **Crear nuevo odontólogo**.
3. Rellena el formulario con nombre, apellido, matrícula, especialidad y correo electrónico.
4. Haz clic en **Crear Odontologo**.

![img](https://i.imgur.com/fkqrnos.png)

---

## Crear recepcionista (Administrador)

1. Dirígete a la sección **Usuarios** o **Recepcionistas**.
2. Haz clic en **Crear nuevo recepcionista**.
3. Ingresa los datos requeridos y asigna el rol de recepcionista.
4. Haz clic en **Crear Recepcionista**.

![img](https://i.imgur.com/5bHUbRF.png)

---

## Crear paciente (Recepcionista)

1. Entra a la sección **Pacientes**.
2. Haz clic en **Crear nuevo paciente**.
3. Completa los campos: nombre, apellido, RUT, dirección, correo electrónico y teléfono.
4. Haz clic en **Crear Paciente**.

![img](https://i.imgur.com/790qF6j.png)
---

## Crear cita (Recepcionista)

1. Ingresa a la sección **Citas**.
2. Haz clic en **Crear nueva cita**.
3. Selecciona la fecha, duración, paciente y odontólogo.
4. Haz clic en **Crear** para registrar la cita.

![img](https://i.imgur.com/7q1rINF.png)

---

**Fin del manual**

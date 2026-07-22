# El Paraíso del Sabor - Enterprise Management System
### Prototipo de Alta Fidelidad Integrado para Ingeniería de Software y Análisis de Sistemas

Este proyecto fue concebido, diseñado y desarrollado como un **prototipo funcional de alta fidelidad** con un enfoque estratégico multi-materia. En lugar de optar por un prototipo estático en diapositivas o maquetas interactivas tradicionales, se determinó que la construcción de un sistema de software real bajo el ecosistema .NET unificaba los criterios de evaluación de tres asignaturas simultáneas: **Programación**, **Bases de Datos** y **Análisis de Sistemas**, optimizando los tiempos de ciclo de desarrollo y entrega académica.

---

## 🏗️ 1. Justificación de la Arquitectura y Selección Tecnológica

La elección de **Blazor Server SPA** (Single Page Application) sobre .NET 10.0 responde directamente a los requisitos de diseño de sistemas modernos, modularidad y despliegue ágil:

*   **Modularidad Basada en Componentes:** La interfaz se descompone en componentes reutilizables (archivos `.razor`), permitiendo que el mantenimiento, los cambios estéticos o la lógica de negocio de la comanda y el catálogo se modifiquen en un único bloque aislado sin afectar la estabilidad general del sistema.
*   **Velocidad de Despliegue (Time-to-Market):** Al unificar todo el ecosistema en **C#**, se elimina la necesidad de desarrollar APIs REST intermedias complejas o escribir código JavaScript redundante. Gracias a la infraestructura nativa de **SignalR**, cualquier actualización en el servidor se propaga en tiempo real a la interfaz mediante WebSockets automáticos, acelerando el ciclo de retroalimentación.
*   **Portabilidad y Eficiencia Contenida:** La inclusión de **SQLite Relacional Embebido** suprime la sobrecarga de configurar y administrar servidores de bases de datos externos pesados durante la fase de prototipado, garantizando integridad referencial estricta y consistencia transaccional con una huella de memoria mínima.

---

## 🔄 2. Metodología de Desarrollo: Extreme Programming (XP) Adaptado

El ciclo de vida del proyecto se gestionó bajo los principios ágiles de la metodología **Extreme Programming (XP)**, adaptada para dinámicas de trabajo en equipos remotos distribuidos geográficamente:


```text
[Historias de Usuario] ──> [Iteraciones Cortas] ──> [Desarrollo Remoto] ──> [Testing en Tiempo Real] ──> [Despliegue VPS]
```

*   **Entregas Pequeñas (Small Releases):** El desarrollo avanzó mediante incrementos funcionales mínimos pero testeables (Módulo de usuarios ➡️ Catálogo reactivo ➡️ Comandas transaccionales ➡️ Background Workers cambiarios).
*   **Diseño Simple:** Se priorizó un diseño arquitectónico limpio y desacoplado, implementando estrictamente las reglas de negocio vigentes y evitando sobreingeniería que retrasara los entregables de las asignaturas.
*   **Pruebas del Cliente e Integración Continua (QA Remoto):** Para validar la calidad del software sin compartir espacio físico, el flujo operó de manera síncrona/asíncrona: mientras el ingeniero de software codificaba y desplegaba los cambios en el servidor **VPS (http://159.198.39.141:5000)**, el especialista en QA realizaba pruebas de caja negra y aceptación desde su propia ubicación en tiempo real, garantizando un lazo de retroalimentación inmediato.

---

## 📊 3. Modelo de Análisis de Sistemas y Reglas de Negocio

El sistema implementa un **Modelo Conceptual y Relacional de Datos** acoplado a un **Modelo de Arquitectura Basada en Eventos Reactivos**, respondiendo a las exigencias de auditoría interna requeridas en el análisis de sistemas de información:


```text
[Usuario] (1) ─── 🔑 UsuarioId ───> (N) [Pedido] (1) ─── 🔑 PedidoId ───> (N) [DetallePedido]│🔑 ProductoId│(1) [Producto]
```

---

### Mecanismos de Consistencia y Seguridad Sistémica
1.  **Preservación Histórica Transaccional:** Ante el riesgo analítico de la mutabilidad de datos (variación de costos en la tabla maestra de `Productos`), la entidad intermedia `DetallePedido` congela de forma atómica el `PrecioUnitario` exacto en el instante preciso de la compra, blindando las auditorías contables frente a modificaciones retroactivas.
2.  **Abstracción mediante Borrado Lógico (Soft Delete):** Ningún registro se destruye físicamente mediante sentencias `DELETE`. La desincorporación de inventario obedece a una bandera booleana (`Activo = false`), salvaguardando la integridad referencial de claves foráneas históricas asociadas a comandas antiguas.
3.  **Seguridad en Gestión de Concurrencia (Thread-Safety):** Para mitigar condiciones de carrera (*Race Conditions*) inherentes a los circuitos persistentes de Blazor Server, el backend implementa una fábrica abstracta (`IDbContextFactory<T>`), obligando a que cada consulta asíncrona instancie y destruya su propio bloque de conexión aislado.

---

## 🗣️ 4. Estructura Organizacional y Roles para la Defensa

Con el propósito de evaluar las competencias sistémicas de todos los integrantes del equipo, la defensa del proyecto ante el jurado se estructura dividiendo el sistema en cuatro dimensiones claras (Procesos, Negocio, Calidad y Tecnología), permitiendo que los miembros no programadores lideren áreas analíticas críticas:

*   **Rol 1: Analista de Sistemas / Project Manager (Gestión de Procesos):** Defensa de la problemática del restaurante, justificación de la metodología ágil **XP**, levantamiento de Historias de Usuario, gestión del tiempo y viabilidad del desarrollo remoto.
*   **Rol 2: Diseñador de Base de Datos / Analista Funcional (Lógica de Negocio):** Defensa del modelo entidad-relación, reglas de integridad referencial, el mecanismo de **Borrado Lógico** y la preservación del histórico contable en `DetallePedido`.
*   **Rol 3: Aseguramiento de Calidad / Tester de Software (Control de Calidad):** Defensa de la estrategia de **Testing Remoto**, pruebas de aceptación, usabilidad del prototipo de alta fidelidad, validación de flujos de usuario y control de despliegue en el entorno de producción (VPS).
*   **Rol 4: Ingeniero de Software Core / Desarrollador (Capa Tecnológica):** Defensa de la infraestructura técnica avanzada: inyección de dependencias con ciclos de vida controlados, manejo de concurrencia y subprocesos con `IDbContextFactory`, y automatización asíncrona en segundo plano (`DollarBackgroundService`).

---

## 🚀 Despliegue e Infraestructura (VPS Deployment)

Mientras el servidor VPS permanezca activo, la aplicación puede consultarse desde cualquier navegador web mediante la siguiente dirección:

*   [http://159.198.39.141:5000](http://159.198.39.141:5000)

La disponibilidad de esta URL está sujeta al estado del servicio en el VPS y se mantiene vigente durante un lapso de tiempo indefinido, condicionado únicamente por la administración del servidor.

---

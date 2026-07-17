# El Paraíso del Sabor - Enterprise Management System

Sistema modular de arquitectura desacoplada para la automatización de inventario, procesamiento de comandas transaccionales y auditoría de datos en tiempo real, desarrollado bajo el ecosistema de alto rendimiento de .NET y Blazor Server.

## 🏗️ Arquitectura del Sistema

La solución se rige bajo principios de diseño limpio, desacoplamiento estricto de responsabilidades y ejecución asíncrona no bloqueante (Non-blocking Asynchronous I/O). Toda la carga computacional pesada y el renderizado del DOM se centralizan en el servidor, utilizando canales de comunicación bidireccional mediante WebSockets a través de la infraestructura de **SignalR**.

### 💻 Stack Tecnológico
*   **Core Engine:** .NET 8.0 / C# 12
*   **Framework de Interfaz:** Blazor Server SPA (Single Page Application)
*   **Capa de Persistencia (ORM):** Entity Framework Core Code-First
*   **Motor de Base de Datos:** SQLite Relacional Embebido
*   **Entorno de Compilación Nativo:** Clang / Make en Linux Environment (Termux Isolated Environment)

---

## 📊 Arquitectura Relacional de Datos (Database Schema)

El diseño físico implementa una base de datos relacional de alta consistencia con restricciones explícitas de clave primaria, unicidad e integridad referencial en cascada controlada.

```text
  [Usuario] (1) ─── 🔑 UsuarioId ───> (N) [Pedido] (1) ─── 🔑 PedidoId ───> (N) [DetallePedido]
                                                                                      │
                                                                                🔑 ProductoId
                                                                                      │
                                                                           (1) [Producto]
```

### Mecanismos de Integridad Aplicados
1. **Preservación Histórica Transaccional:** La entidad intermedia `DetallePedido` captura y congela los valores de `PrecioUnitario` en el instante exacto de la confirmación de compra. Esto evita la alteración retroactiva de auditorías contables frente a modificaciones posteriores en la tabla maestra de `Productos`.
2. **Abstracción mediante Borrado Lógico (Soft Delete):** La remoción de stock obedece a un indicador booleano (`Activo = false`). Las filas físicas jamás se destruyen con sentencias `DELETE` directas, salvaguardando la integridad referencial de claves foráneas históricas asociadas a comandas antiguas.

---

## ⚙️ Capa de Servicios y Procesamiento Asíncrono

La aplicación implementa la inyección de dependencias (`Dependency Injection`) con ciclos de vida controlados (`Scoped`, `Singleton`, `Transient`) para mitigar colisiones en subprocesos concurrentes.

### Gestión Segura de Contextos (Thread-Safety)
Debido a la naturaleza persistente de los circuitos de Blazor Server, compartir instancias directas de contextos de base de datos puede inducir a condiciones de carrera (`Race Conditions`). Para resolver esto, el sistema inyecta una fábrica abstracta (`IDbContextFactory<T>`), asegurando que cada consulta asíncrona instancie y destruya su propio bloque de conexión a disco:

```csharp
public async Task<bool> CrearPedidoAsync(Pedido pedido)
{
    using var context = await _contextFactory.CreateDbContextAsync();
    context.Pedidos.Add(pedido);
    return await context.SaveChangesAsync() > 0;
}
```

---

## 🌐 Consumo Automatizado de Servicios Web (External API & Background Workers)

Para mitigar los efectos de la fluctuación de la moneda local, el backend ejecuta una tarea cronometrada persistente en segundo plano implementando la abstracción abstracta `BackgroundService`.

### Pipeline de Datos Cambiarios
1. El `DollarBackgroundService` despierta de forma cíclica (cada 4 horas) en un hilo secundario sin interferir con el hilo de ejecución de la interfaz gráfica.
2. Realiza una petición asíncrona `GET` y deserializa un payload estructurado desde el endpoint seguro de la API cambiaria externa.
3. Actualiza de manera atómica un servicio centralizado tipo `Singleton` (`DollarService`), propagando instantáneamente el nuevo factor de conversión a los componentes del catálogo y la comanda sin necesidad de llamadas directas a base de datos en cada renderizado.

#### Estructura de Intercambio (JSON DTO Mapping)
```json
{
  "compra": 36.45,
  "venta": 36.55,
  "promedio": 36.50,
  "fechaActualizacion": "2026-07-17T00:00:00Z"
}
```

---

## 🔒 Control de Estado y Autenticación Personalizada

El software descarta los intermediarios pesados de cookies nativas para implementar un motor ágil de gestión de sesiones a través del servicio desacoplado `AuthService`. 

El estado de la sesión activa de los clientes (`UsuarioActual`) se mantiene encapsulado y expone un patrón de eventos reactivos (`Action OnChange`). Cuando un cliente inicia o cierra sesión, el evento notifica al árbol de componentes de Blazor para forzar el refresco selectivo de la interfaz (`StateHasChanged`), bloqueando las rutas críticas del Dashboard Administrativo y redirigiendo el procesamiento del carrito de compras de forma totalmente dinámica y segura.

---

## 🚀 Despliegue e Infraestructura (VPS Deployment)

Mientras el servidor VPS permanezca activo, la aplicación puede consultarse desde cualquier navegador web mediante la siguiente dirección:

*   [http://159.198.39.141:5000](http://159.198.39.141:5000)

La disponibilidad de esta URL está sujeta al estado del servicio en el VPS y se mantiene vigente durante un lapso de tiempo indefinido, condicionado únicamente por la administración del servidor.

---

## Consideraciones finales

La adopción de Blazor en lugar de Windows Forms, el uso de Git y GitHub para la gestión del código y la ejecución del sistema en un VPS fueron decisiones clave que definieron la forma de trabajar en **El Paraíso del Sabor**.

Estas elecciones permitieron construir una solución modular, accesible de forma remota y alineada con tecnologías actuales de desarrollo en .NET, manteniendo al mismo tiempo el enfoque académico y la posibilidad de evaluar el proyecto tanto a nivel técnico como organizativo.

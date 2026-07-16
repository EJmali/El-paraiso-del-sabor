# El Paraíso del Sabor – Informe técnico

Este documento describe la solución técnica implementada para la aplicación web **El Paraíso del Sabor**, desarrollada en .NET 10 con Blazor, así como las decisiones principales que guiaron su construcción y organización.

## Descripción general de la solución

El sistema es una aplicación web de tipo e‑commerce orientada a la gestión de productos relacionados con el ámbito alimenticio y a la interacción con usuarios registrados mediante un mecanismo de autenticación propia.

La estructura del proyecto se apoya en una arquitectura modular con componentes Blazor, servicios de lógica de negocio y acceso a datos con SQLite y Entity Framework Core, manteniendo todo el código gestionado mediante Git y alojado en un repositorio en GitHub.

## Adopción de Blazor como interfaz principal

En el planteamiento inicial del proyecto, el uso de Windows Forms se estableció como opción obligatoria para la interfaz de usuario. Durante el desarrollo, se propuso y acordó con el profesor la adopción de Blazor como alternativa tecnológica, con base en las siguientes razones:

- Blazor ofrece una interfaz web moderna que se ejecuta en el navegador, sin necesidad de instalar clientes de escritorio en cada equipo.
- Su modelo basado en componentes facilita un desarrollo modular y organizado, permitiendo separar claramente vistas, lógica y manejo de datos.
- Al integrarse directamente en el ecosistema actual de .NET, Blazor refleja prácticas más cercanas a aplicaciones web reales, lo que enriquece el valor técnico del proyecto.

De este modo, se sustituyó Windows Forms por Blazor manteniendo los objetivos funcionales requeridos y aprovechando un marco de trabajo más adecuado para la colaboración, la modularidad y la evolución futura de la solución.

## Uso de Git y GitHub en el desarrollo

Para la gestión del código fuente se adoptó Git como sistema de control de versiones y GitHub como plataforma de alojamiento del repositorio del proyecto.

Esta combinación permitió:

- Registrar de forma continua los cambios realizados en el código y las decisiones técnicas tomadas.
- Coordinar el trabajo del equipo sobre una misma base de código, reduciendo problemas de versiones y archivos desactualizados.
- Contar con un punto central donde revisar el historial del proyecto, documentar el avance y presentar el resultado final.

Al apoyarse en GitHub, el repositorio de **El Paraíso del Sabor** se convierte también en el lugar natural para consultar este informe técnico y el resto de documentación asociada.

## Uso de VPS para acceso remoto al sistema

Además del entorno local de desarrollo, se habilitó un servidor VPS para ejecutar la aplicación de forma remota.

El uso del VPS aportó:

- La posibilidad de que cada integrante del equipo accediera a la aplicación desde su navegador, observando el avance del proyecto en tiempo real.
- Un punto centralizado para administrar el dashboard y comprobar el comportamiento del sistema en un entorno separado del desarrollo.
- Una experiencia más cercana a un entorno de uso real, manteniendo al mismo tiempo el carácter académico del proyecto.

El VPS, combinado con el repositorio en GitHub y la interfaz en Blazor, permitió que el proyecto se desarrollara y revisara de forma coordinada, haciendo visibles los cambios a todo el equipo sin depender de copias locales aisladas.

## Consideraciones finales

La adopción de Blazor en lugar de Windows Forms, el uso de Git y GitHub para la gestión del código y la ejecución del sistema en un VPS fueron decisiones clave que definieron la forma de trabajar en **El Paraíso del Sabor**.

Estas elecciones permitieron construir una solución modular, accesible de forma remota y alineada con tecnologías actuales de desarrollo en .NET, manteniendo al mismo tiempo el enfoque académico y la posibilidad de evaluar el proyecto tanto a nivel técnico como organizativo.
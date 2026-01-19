🌍 **Leer en otros idiomas:** [English](README.md)

# 🧩 ServiceHub 

ServiceHub es un concentrador de servicios modular diseñado para cargar, ejecutar y gestionar módulos independientes mediante reflexión, de forma desacoplada y extensible.

El objetivo principal es proporcionar una base ligera y reutilizable para aplicaciones que necesiten ejecutar tareas, servicios o procesos en segundo plano sin depender de frameworks específicos (WinForms, WPF, ASP.NET, etc.).


## 🎯 Objetivos del proyecto

- Proporcionar un núcleo único (Hub) capaz de:
  - Descubrir módulos dinámicamente
  - Cargar módulos desde DLLs externas
  - Gestionar su ciclo de vida

- Permitir el desarrollo de módulos desacoplados y reutilizables
- Mantener una arquitectura minimalista, clara y extensible
- Ser compatible con: 
    - .NET moderno (.NET 10)

## 🧠 Filosofía

#### Modularidad primero
Cada módulo es una unidad autónoma (plugin / pod) que expone su funcionalidad mediante contratos bien definidos.

#### Contratos antes que implementaciones
El núcleo solo conoce interfaces, nunca implementaciones concretas.

#### Sin dependencias innecesarias
El núcleo y los contratos no dependen de UI, servicios web ni librerías externas pesadas.

#### Evolutivo
Diseñado para crecer hacia:
 - Ejecución en segundo plano
 - Colas de tareas
 - Gestión de estados
 - Módulos especializados (scheduler, workers, etc.)

## 🧱 Arquitectura general

```
ServiceHub
│
├─ ServiceHub.Core        → Núcleo del sistema
├─ ServiceHub.Contracts   → Interfaces y contratos comunes
├─ ServiceHub.Modules     → Implementaciones de módulos (plugins)
│
└─ Host (App)
    └─ Carga y orquesta los módulos
```

## 🔌 Módulos

Un **módulo** es una DLL que:
- Implementa una interfaz común
- Se carga dinámicamente mediante reflexión
- Gestiona su propio estado interno
- Se comunica con el Hub a través de un contexto compartido

Los módulos pueden:
- Ejecutar lógica en background
- Exponer servicios a otros módulos
- Actuar como workers, schedulers o proveedores de recursos

## 🚀 Casos de uso
- Servicios en segundo plano
- Automatización de tareas
- Sistemas extensibles por plugins
- Herramientas headless
- Aplicaciones embebidas o de bajo consumo
- Sistemas modulares tipo micro-servicios locales


## 📦 Estado del proyecto

**🟡 En diseño inicial**

El proyecto se encuentra en fase de definición de arquitectura y contratos.

El código se irá construyendo de forma incremental, priorizando:

 - Contratos estables
 - Núcleo mínimo funcional
 - Extensibilidad sin romper compatibilidad

## 📜 License

[MIT](https://choosealicense.com/licenses/mit/)


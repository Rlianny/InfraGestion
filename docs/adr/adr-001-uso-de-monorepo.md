## Título: Decisión de usar un monorepositorio
**Estado**: Aceptado
**Contexto**: Queremos tomar la mejor decisión que nos ayude a avanzar ágilmente.
**Decisión**: Decidimos usar un **Monorepositorio** (un solo repositorio para frontend y backend) al ser un equipo de 4 personas sin intenciones de ampliarse para desarrollar un proyecto de principio a fin.

## Consecuencias
# Ventajas: 
- **Simplicidad inicial**: Es mucho más fácil configurar. Un único lugar para clonar, un único conjunto de issues, un único proyecto board.
- **Atomicidad en los commits**: Es mucho más fácil hacer un cambio que afecte tanto al backend (una nueva API) como al frontend (el consumo de esa API) en un solo commit y Pull Request. Esto mantiene el historial coherente y el proyecto sincronizado.
- **Mejor colaboración**: Con un equipo tan pequeño, todos tendrán visibilidad completa del proyecto. Un desarrollador de backend puede revisar fácilmente el código del frontend y viceversa.
- **Menos overhead administrativo**: Gestionas un solo repositorio, sus permisos, sus secrets de GitHub Actions, etc.
# Desventajas:
- **El código crece**: Con el tiempo, el repositorio puede volverse muy grande, lo que puede ralentizar las operaciones de Git (aunque esto suele ser un problema en proyectos mucho más grandes).
- **Despliegues separados**: Aunque el código esté junto, probablemente quieras desplegar frontend y backend por separado. Esto se puede manejar con una CI/CD bien configurada.

## Alternativas consideradas 
Se consideró la opción de un Multirepositorio (Multi-repo/ Polyrepo), constituido por dos repositorios separados (ej: InfraGestion-frontend e InfraGestion-backend).
Se descartó por las siguientes razones:
- **Complejidad aumentada**: Tienes que configurar y mantener el doble de repositorios, proyectos de GitHub, pipelines de CI/CD, etc.
- **Trabajo coordinado**: Hacer una feature que necesite cambios en ambos lados se vuelve más complicado. Requerirá dos PRs separados, que idealmente deberían ser probados, mergeados y desplegados de forma coordinada. Es fácil que se desincronicen.
- **Compartir código es difícil**: Para compartir tipos o utilidades, necesitarás crear un tercer repositorio como un paquete de librería privada, lo que añade una enorme complejidad inicial.

## Información adicional
Para un equipo de 4 personas que está empezando un proyecto desde cero, la opción más ágil, simple y productiva es sin duda el Monorepositorio.

La productividad ganada por la atomicidad de los cambios y la simplicidad de gestión en las primeras fases del proyecto supera con creces las posibles desventajas, que no se manifestarán hasta que el proyecto sea mucho más grande.

Se recomienda crear branches por funcionalidad, no por capa: No crees una rama `frontend` y otra `backend`. Es un anti-patrón. En su lugar, sigue la estrategia de branches por feature. Se recomienda además usar Labels (`frontend`, `backend`, `full-stack`) y Milestones para organizar las tareas en un único tablero de proyecto. Así puedes filtrar fácilmente.

# ¿Cuándo considerar separarlos en el futuro?
Si el proyecto escala masivamente, los equipos se vuelven muy grandes e independientes, y los ciclos de release de frontend y backend se desacoplan totalmente, entonces podrían plantearse migrar a un multi-repositorio. Pero no es una optimización que necesite hacerse ahora.
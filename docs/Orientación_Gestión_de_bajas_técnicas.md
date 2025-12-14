# Gestión de bajas técnicas

Bases de Datos II - Ingeniería de Software

Curso: 2025 - 2026

El presente documento describe una problemática para ser resuelta durante el semestre. Presenta además,

particulares a evaluar por cada asignatura de manera independiente y en conjunto.

## Características generales

El proyecto tributa a la evaluación de las asignaturas Bases de Dato II e Ingeniería de Software, donde

cada una controlará aspectos no necesariamente coincidentes.

El desarrollo de la solución será llevado a cabo por un equipo de estudiante de hasta 5 integrantes, aunque

la cantidad exacta de miembros se confirmará durante las primeras clases presenciales.

La nota de cada miembro del equipo será individual y no necesariamente la misma para ambas asignaturas,

teniendo en cuenta:

-  Labor de curso.
-  Trabajo del equipo.
-  Requisitos particulares de la asignatura de Bases de Datos (ver archivo adjunto).
-  Requisitos particulares de la asignatura de Ingeniería de Software (al final del documento).

## Descripción del problema

Se desea confeccionar una aplicación web que de como solución un sistema de la gestión de las bajas

técnicas para una empresa de infocomunicaciones. Este sistema debe manejar el inventario de equipos, los
procesos de baja y traslado de equipos, los mantenimientos, el personal técnico involucrado y las personas
responsables de recibir los equipos tras su baja o traslado. El objetivo es automatizar y optimizar la gestión
de las bajas técnicas, lo cual actualmente se realiza de manera manual así como el riesgo de pérdida de
información debido al gran volumen de datos.

El sistema debe de permitir la existencia de usuario capaz de tener acceso a la gestión completa del

inventario de los equipos, las bajas y los mantenimientos realizados, cuyo acceso le posibilitará la modificación
de los datos existentes o añadir nueva información.

Cada sección tiene asignado un responsable, el cual puede solicitar el traslado de los equipos y revisar

los inventarios de su área. Los técnicos, usuarios que trabajarán con el sistema diariamente, podrán registrar
las intervenciones de sus mantenimientos realizados y definir las bajas de los equipos atendidos por ellos.

El personal encargado de recibir los equipos ser revisados y/o reparados es responsable de registrar en el

sistema el recibimiento de los equipos, así como los traslados y las bajas propuestas por los técnicos; durante
el proceso de la recepción se define el departamento responsable a trabajar con el dispositivo. También, el
director del centro tiene control total sobre la información manejada sobre el sistema y puede generar los
reportes asociados el estado del inventario, las bajas técnicas y la efectividad del personal.

Sobre la información, cada equipo tendrá un identificador único, el nombre del equipo, su tipo (informáti
co, de comunicaciones, eléctrico, etc.), su estado (operativo, en mantenimiento, dado de baja), su ubicación
actual y la fecha de adquisición. Además, se debe almacenar el historial de los mantenimientos realizados,
con detalles como la fecha del mantenimiento, el tipo de mantenimiento, el costo asociado y el técnico
responsable.

Cuando un equipo es dado de baja, se registra la causa de la baja (fallo técnico irreparable, obsolescencia,

entre otras), la fecha de la baja, el destino final del equipo (almacén, desecho, traslado a otra unidad), así
como la persona que recibe el equipo en su destino. De este tipo de persona, conocida como “receptor del
equipo”, se registrarán su identificador, el nombre, el departamento al que pertenece, el departamento al que
enviará el equipo y el nombre de la persona que envía el equipo.

El sistema también gestionará los traslados de equipos entre diferentes secciones o unidades, registrando

la fecha del traslado, el origen, el destino, el personal responsable del envío y el receptor del equipo.

Se deberá llevar un registro detallado de los técnicos encargados del mantenimiento y baja de los equipos.

De cada técnico se almacenará su nombre, número de identificación, años de experiencia, especialidad y el

historial de intervenciones que ha realizado (mantenimientos o bajas). Además, se llevará un registro del
rendimiento de los técnicos, basado en las valoraciones de sus superiores, que pueden afectar su salario con
bonificaciones o penalizaciones.

## Funcionalidades

Tomando en cuenta la información almacenada en la base de datos, el sistema debe de proveer resultados
(tablas y gráficos) para cada una de las demandas descritas a continuación:

1. Obtener el listado de los equipos dados de baja en el último año, incluyendo la causa de la baja, el
destino final y el nombre de la persona que recibió el equipo.

2. Obtener el historial de los mantenimientos de un equipo específico, clasificando los mantenimientos por
tipo y fecha, junto con los técnicos que realizaron las intervenciones.

3. Listar los equipos que han sido trasladados entre diferentes secciones, indicando las fechas, el origen,
el destino, los nombres de la persona que envía y de la persona que recibe el equipo.

4. Identificar la correlación entre el rendimiento de los técnicos (basado en valoraciones) y la longevidad
de los equipos que mantienen, específicamente para los equipos que finalmente son dados de baja por
“fallo técnico irreparable”. La consulta debe calcular el costo total de mantenimiento por tipo de equipo
antes de ser dado de baja, y cruzar esta información para determinar qué técnicos tienen el mayor costo
de mantenimiento asociado a los equipos que, en última instancia, son declarados como irreparables.
Finalmente, la consulta debe generar un reporte que muestre los 5 técnicos con la peor correlación
(alto costo de mantenimiento y baja longevidad del equipo), su rendimiento promedio (valoración de
sus superiores) y el tipo de equipo en el que se especializan.

5. Obtener un informe de los equipos que han recibido más de tres mantenimientos en el último año y
que, por normativa, deben ser reemplazados.

6. Comparar el rendimiento de los técnicos, basándose en las valoraciones recibidas y la cantidad de
intervenciones realizadas, para determinar bonificaciones o penalizaciones en su salario.

7. Generar un reporte con los equipos que han sido enviados a un departamento específico, indicando
los nombres de la persona quien envía y quien recibe. Incluya el la empresa la cual envía los equipos
defectuosos.

Además, la posibilidad de exportar la información mostrada a ficheros con formato PDF tiene que ser
una funcionalidad provista por el sistema para todo tipo de usuario. Así como poder ordenar cada columna
de los resultados acorde a los intereses del usuario final.

## Requisitos particulares de la asignatura Ingeniería de Software

Además de la implementación de la aplicación web, las evaluaciones consistirán también de seminarios y
preguntas escritas, los cuales se orientarán y se explicarán en su momento durante el semestre.
Con respecto al desarrollo de la solución, los requerimientos utilizados para calificar el trabajo serán:

1. Trabajar con un control de versiones (github, tfs, etc.).
2. Realizar la planificación con alguna herramienta CASE (github, jira, gantt, etc.).
3. Sistema multiplataforma.
4. Cumplir con todos los requerimientos funcionales planteados en el problema.
5. Tener buenas prácticas de programación, incluido los comentarios en todo el código (doestring).
6. Implementar al menos dos patrones.
7. Implementar una arquitectura que permita a la aplicación ser desacoplada, extensible en funcionalidades y mantenible .
8. Implementar pruebas unitarias, tanto como para el _back-end_ como para el _front-end_.

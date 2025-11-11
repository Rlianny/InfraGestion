# InfraGestion - Technical Decommissioning Management System

![Project Status](https://img.shields.io/badge/Status-Active%20Development-brightgreen)
![Tech Stack](https://img.shields.io/badge/Stack-.NET%209%20|%20React-61DAFB?logo=react&logoColor=black&labelColor=512BD4&color=512BD4)

## Project Overview

InfraGestion is a web application designed to automate and optimize the management of **technical decommissionings, inventory, and Device maintenance** for InfraCom Conectividad, S.A.

This system replaces the current manual process based on spreadsheets and paper forms, significantly reducing the risk of information loss and greatly improving operational efficiency. It provides a centralized, reliable platform for asset lifecycle management.

## Technology Stack

This project is built using a modern, scalable, and maintainable technology stack:

| Component | Technology | Version | Purpose |
| :--- | :--- | :--- | :--- |
| **Frontend Framework** | React | 18+ | Building the user interface (UI) |
| **Backend Framework** | .NET | 9 (LTS) | Core backend development and business logic |
| **ORM** | Entity Framework Core | 9 | Object-Relational Mapping for database interaction |
| **API** | ASP.NET Core Web API | 9 | Building robust and scalable RESTful APIs |
| **Mediator Pattern** | MediatR | 12+ | Implementing the Command Query Responsibility Segregation (CQRS) pattern |
| **Frontend Language** | TypeScript | 4.x | Static typing for improved frontend reliability |
| **Backend Language** | C\# | 11 | Primary backend programming language |
| **Version Control** | Git + GitHub | N/A | Source code management and collaboration |
| **Database** | PostgreSQL | 15+ | Reliable and powerful relational data storage |

## Getting Started

Instructions on how to set up the project locally are pending and will be added here. This section will cover:

```bash
# Clone the repository
# Navigate to the backend directory and run:
# dotnet restore
# dotnet run
#
# Navigate to the frontend directory and run:
# npm install
# npm start
# ...
```

## Architectural Decision Records (ADR)

Architectural and strategic management decisions are critical to a project's long-term health. Documenting these decisions provides essential context to the development team, explaining *why* the code is structured in a particular way and preventing perpetual reassessment of established choices ("Wouldn't it be better to use X instead of Y?"). A well-documented decision (ADR) closes the debate and ensures time is not wasted revisiting already-analyzed topics.

### Documentation Location

The documentation for these types of decisions is maintained using **Architectural Decision Records (ADR)**.

  - **Location**: The records are stored in the repository's root folder under `docs/adr`.
  - **Format**: Each ADR is a Markdown file named following the convention `adr-001-description-of-the-decision.md`, capturing a single significant decision.
  - **Visibility**: A summary or link to the detailed ADR can be included in the main `README.md` for quick reference.

### ADR Template

The standard template for documenting an ADR is as follows:

  - **Title**
  - **Status**: Proposed / Accepted / Superseded
  - **Context**: What problem are we trying to solve?
  - **Decision**: A clear statement of the final decision.
  - **Consequences (Trade-offs)**: The pros and cons of the decision.
  - **Considered Alternatives** (and the reasons for their rejection).
  - **Additional Information**

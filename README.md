# Сollab Сode Editor

**Collab Code Editor** is a collaborative online code editor designed to support real-time code execution in multiple programming languages with the ability to build and run graphical applications.

![image](https://github.com/user-attachments/assets/ead617d9-3024-4cf0-ad78-f9c332ca2dd7)


## Features

- **Multi-Language Support**. Write and execute code in multiple programming languages within a single environment.
- **Real-Time Collaboration**. Simultaneously edit code with other users, with changes synchronized in real-time.
- **Graphic Application Execution**. Run and interact with graphical applications in an isolated environment using VNC and Docker.
- **Scalable Infrastructure**. Built on containerization and cloud technologies like Docker and Kubernetes, the platform ensures robust performance and high availability.
- **Project Management**. Create, manage, and collaborate on projects with ease through a user-friendly interface.
- **Authentication and Authorization**. Secure user management with integrated authentication and authorization mechanisms.

## Architecture

![Architecture](https://github.com/user-attachments/assets/cd3df1cc-e21e-4d9e-8f37-b618d5ab1d00)


The project utilizes a **microservices** architecture, ensuring modularity and scalability. Key components include.

- **Web Client**. Frontend providing an interactive interface for users to create, edit, and manage projects.
- **Project Management Service**. Handles project creation, updates, and access control.
- **Sync Service**. Ensures that code changes are instantly synchronized across all connected users.
- **File Service**. Manages file uploads, downloads, and storage using an S3-compatible solution.
- **Sandbox Service**. Provides isolated environments for code execution using Docker and VNC, supporting graphical outputs.
- **Authentication Service**. Manages user authentication and authorization, ensuring secure access to the platform.

## Technologies Used

- **Frontend**. Built with React, TypeScript and Monaco Editor.
- **Backend**. C# with ASP.NET Core, Golang.
- **Database**. PostgreSQL.
- **Synchronization**. WebSocket and y-websocket libraries.
- **Authentication**. Secure user authentication and authorization through OpenIddict integrated with ASP.NET Core.
- **File Management**. Minio S3-compatible storage.
- **Execution Environments**. Docker containers with integrated VNC for graphical application support.

## Demonstration



https://github.com/user-attachments/assets/64601168-7683-4175-ad2d-dd6a7926a248


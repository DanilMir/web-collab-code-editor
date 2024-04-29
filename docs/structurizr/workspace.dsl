workspace {

    model {
        
        properties {
            "structurizr.groupSeparator" "/"
        }


        user = person "Customer"
        
        softwareSystem = softwareSystem "Web Collab Code Editor" {
                webClient = container "Web client" "React frontend app" {
                    tags "frontend, react"
                    technology "React"
                }

                auth = container "Auth Service" {
                    description "User authentication and authorization service."
                    tags "backend, c-sharp"
                    technology "C# ASP.NET Core"                    
                }

                authDb = container "Auth Database" {
                    tags "db, postgres"
                    technology "PostgreSQL"
                }
                
                projectsManagement = container "Projects Management Service" {
                    description "Service for creating and managing projects."
                    tags "backend, c-sharp"
                    technology "C# ASP.NET Core"  
                }

                projectsDb = container "Projects Management Database" {
                    tags "db, postgres"
                    technology "PostgreSQL"
                }

                files = container "Files Service" {
                    description "Service for file uploading, downloading, processing"
                    tags "backend, c-sharp"
                    technology "C# ASP.NET Core"
                }

                s3 = container "S3 Service" {
                    tags "cloud, minio"
                    technology "Minio"
                }
                
                collab = container "Sync Service" {
                    description "Synchronization of code changes in real-time."
                    tags "backend, golang"
                    technology "Golang + y-websocket"        
                }
                                
                virtual = container "Sandbox" {
                    description "Isolated execution environments for code."
                    tags "backend, c-sharp"
                    technology "C# ASP.NET Core"        
                }
                
                apiGateway = container "API Gateway" {
                    description "Gateway for managing and exposing API endpoints."
                    technology "Nginx"
                    tags "API, Gateway, nginx"
                }


                files -> s3
                projectsManagement -> files "Manage project source code"
                files -> virtual "Send project code"
                auth -> authDb "Save credentials"
                projectsManagement -> projectsDb "Save projects info"
                collab -> files "Sync code with storage"
                
                apiGateway -> files
                apiGateway -> auth
                apiGateway -> collab
                apiGateway -> projectsManagement    
                apiGateway -> virtual    
        }

        webClient -> apiGateway
        user -> webClient "Uses"
    }

    views {

        systemContext softwareSystem "SystemContext"{
            include *
            autoLayout 
        }

        container softwareSystem "Containers"{
            include *
            # autoLayout lr
        }


        styles {
            element db {
                stroke "#3b48cc"
                color "#3b48cc"
                background "#ffffff"
                shape cylinder
                fontSize 34
            }

            element cloud {
                stroke "#3b48cc"
                color "#3b48cc"
                background "#ffffff"
                shape Folder
                fontSize 34
            }

            element backend {
                stroke "#3b48cc"
                color "#3b48cc"
                background "#ffffff"
                # shape hexagon

                stroke "#3b48cc"
                strokeWidth 3
                fontSize 34
            }

            element frontend {
                stroke "#3b48cc"
                color "#3b48cc"
                background "#ffffff"
                shape WebBrowser
                fontSize 34
            }


            element nginx {
                color "#000000"
                stroke "#32CD32"
                strokeWidth 10
                background "#ffffff"

                shape hexagon
                fontSize 34
            }


            relationship "Relationship" {
                # routing Orthogonal
                fontSize 34
            }
        }

       

        theme default

    }

}
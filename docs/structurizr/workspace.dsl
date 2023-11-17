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
                    technology "ASP.NET Core"                    
                }

                authDb = container "Auth Database" {
                    tags "db, postgres"
                    technology "PostgreSQL"
                }
                
                projectManagement = container "Project Management Service" {
                    description "Service for creating and managing code projects."
                    tags "backend, c-sharp"
                    technology "ASP.NET Core"  
                }

                files = container "Files Service" {
                    description "Service for file uploading, downloading, processing"
                    tags "backend, c-sharp"
                    technology "ASP.NET Core"
                }

                s3 = container "S3 Service" {
                    tags "cloud, minio"
                    technology "Minio"
                }
                
                collab = container "Collab Service" {
                    description "Synchronization of code changes in real-time."
                    tags "backend, c-sharp"
                    technology "ASP.NET Core"        
                }
                                
                virtual = container "Virtual Environments Service" {
                    description "Isolated execution environments for code."
                    tags "backend, c-sharp"
                    technology "ASP.NET Core"        
                }
                
                apiGateway = container "API Gateway" {
                    description "Gateway for managing and exposing API endpoints."
                    technology "API Gateway"
                    tags "API, Gateway"
                }


                files -> s3
                projectManagement -> files "Manage project source code"
                projectManagement -> virtual "Send project code"
                auth -> authDb "Save credentials"
                collab -> files "Sync code with storage"
                auth -> files "Upload user profile picture"
                
                apiGateway -> files
                apiGateway -> auth
                apiGateway -> collab
                apiGateway -> projectManagement    
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
            autoLayout lr
        }


        styles {
            element db {
                stroke "#3b48cc"
                color "#3b48cc"
                background "#ffffff"
                shape cylinder
            }

            element cloud {
                stroke "#3b48cc"
                color "#3b48cc"
                background "#ffffff"
                shape Folder
            }

            element backend {
                stroke "#3b48cc"
                color "#3b48cc"
                background "#ffffff"
                # shape hexagon

                stroke "#3b48cc"
                strokeWidth 3
            }

            element frontend {
                stroke "#3b48cc"
                color "#3b48cc"
                background "#ffffff"
                shape WebBrowser
            }

            element postgres {
                icon "icons/postgre.png"
            }

            element c-sharp {
                icon "icons/c-sharp.png"
            }

            element redis {
                icon "icons/redis.png"
            }

            element rabbitmq {
                icon "icons/RabbitMQ.png"
            }

            element react {
                icon "icons/react.png"
            }

            element nginx {
                color "#000000"
                stroke "#32CD32"
                strokeWidth 10
                background "#ffffff"
                icon "icons/nginx.png"
                shape hexagon
            }

            element "Message Bus" {
                width 1600
                Shape Pipe
            }

            relationship "Relationship" {
                # routing Orthogonal
            }
        }

       

        theme default

    }

}
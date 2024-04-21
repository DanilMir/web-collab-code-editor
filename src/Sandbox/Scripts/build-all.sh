docker build -f Dockerfile.Base -t sandbox-base .
docker build -f Dockerfile.Dotnet -t sandbox-dotnet .
docker build -f Dockerfile.Dotnet.GTK -t sandbox-dotnet-gtk .
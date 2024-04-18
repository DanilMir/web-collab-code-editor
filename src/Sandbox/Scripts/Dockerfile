# Pull base image.
FROM jlesage/baseimage-gui:alpine-3.19-v4.5.3

RUN install-glibc

# Install xterm.
RUN add-pkg xterm

# Copy the start script.
COPY startapp.sh /startapp.sh

# Set the name of the application.
RUN set-cont-env APP_NAME "Xterm"   
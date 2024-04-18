#!/bin/sh

# Запуск сервера X11VNC
x11vnc -display :0 -forever -shared -rfbauth ~/.vnc/passwd &

# Запуск NoVNC
/opt/noVNC/utils/launch.sh --vnc localhost:5900
#!/bin/sh
set -e # Exit immediately if a command exits with a non-zero status.

cleanup () {
    rc-service vncserver.1 stop 
    rm -rfv /tmp/.X*-lock /tmp/.X11-unix
    exit 0
}
trap cleanup SIGINT SIGTERM

openrc boot > openrc.log 2>&1
rc-service vncserver.1 start > vncserver.log 2>&1
websockify -D --web /usr/share/novnc/ 6901 localhost:5901 > websockify.log 2>&1  

DISPLAY=:1 exec "$@" > exec.log 2>&1

wait $!
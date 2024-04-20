#!/bin/sh
set -e # Exit immediately if a command exits with a non-zero status.

# rm -f ~/.Xauthority
# touch ~/.Xauthority
# xauth generate :1 . trusted
# xauth add ${HOST}:1 . $(xxd -l 16 -p /dev/urandom)

# /usr/bin/startxfce4 --replace
openrc boot > openrc.log 2>&1
rc-service vncserver.1 start > vncserver.log 2>&1
websockify -D --web /usr/share/novnc/ 6901 localhost:5901 

DISPLAY=:1 exec "$@"

tail -f *.log /var/log/vncserver.log
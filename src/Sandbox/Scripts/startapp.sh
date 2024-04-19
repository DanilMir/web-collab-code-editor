#!/bin/sh

set -e # Exit immediately if a command exits with a non-zero status.

openrc boot > openrc.log 2>&1
rc-service vncserver.1 start > vncserver.log 2>&1
websockify -D --web /usr/share/novnc/ 6901 localhost:5901 


tail -f *.log /var/log/vncserver.log
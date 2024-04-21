#!/bin/sh
set -e # Exit immediately if a command exits with a non-zero status.

cleanup () {
    kill -s SIGTERM $!
    exit 0
}
# trap cleanup SIGINT SIGTERM

# mkdir -p "$HOME/.vnc"
# PASSWD_PATH="$HOME/.vnc/passwd"
# rm -f $PASSWD_PATH
# echo "vncpassword" | vncpasswd -f >> $PASSWD_PATH
# chmod 600 $PASSWD_PATH

rm -rfv /tmp/.X*-lock /tmp/.X11-unix

# /usr/bin/startxfce4 --replace
# openrc boot > openrc.log 2>&1
# rc-service vncserver.1 start > vncserver.log 2>&1
# websockify -D --web /usr/share/novnc/ 6901 localhost:5901 

# DISPLAY=:1 exec "$@"
exec "$@"
# tail -f *.log /var/log/vncserver.log
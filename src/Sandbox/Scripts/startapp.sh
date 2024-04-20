#!/bin/sh

set -e # Exit immediately if a command exits with a non-zero status.

cleanup () {
    kill -s SIGTERM $!
    exit 0
}
trap cleanup SIGINT SIGTERM

rm -f "$HOME/.vnc/passwd"

rm -frv /tmp/.X11-unix/; 
rm -frv /tmp/.X1-lock; 
# rm -frv /home/root/.Xauthority; 
# rm -frv /home/root/.vnc/*.log;
# rm -frv /home/root/.vnc/*.pid


mkdir -p "$HOME/.vnc"

echo "vncpassword" | vncpasswd -f >> ~/.vnc/passwd

chmod 600 ~/.vnc/passwd

echo 'DISPLAYS="root:1"' >> /etc/conf.d/tigervnc
echo 'DISPLAYS="root:1"' >> /etc/conf.d/vncserver

echo '{:1=root}' > /etc/tigervnc/vncserver.users


cat <<EOF >> /etc/tigervnc/vncserver-config-defaults
session=xfce
securitytypes=vncauth,tlsvnc
geometry=2000x1200
localhost=no
alwaysshared
EOF

cat <<EOF >> /etc/tigervnc/vncserver-config-mandatory
session=xfce
securitytypes=none
geometry=1080x720
localhost=no
alwaysshared
EOF


mkdir -p /etc/X11/xorg.conf.d


cat <<EOF >> /etc/X11/xorg.conf.d/40-vnc.conf
Section "Module"
	Load "vnc"
EndSection
Section "Screen"
	Identifier "Default Screen"
	Option "PasswordFile" "/etc/X11/vncpasswd"
EndSection
EOF

printf "vncpassword\nvncpassword\n\n" | vncpasswd /etc/X11/vncpasswd

ln -s vncserver /etc/init.d/vncserver.1



openrc boot > openrc.log 2>&1
websockify -D --web /usr/share/novnc/ 6901 localhost:5901  > vncserver.log 2>&1
rc-service vncserver.1 start > vncserver.log 2>&1


echo "Executing command: '$@'"
DISPLAY=:1 $@

tail -f *.log /var/log/vncserver.log
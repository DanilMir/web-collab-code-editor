#!/bin/sh

set -e
set -u


mkdir -p "$HOME/.vnc"

echo "vncpassword" | vncpasswd -f >> ~/.vnc/passwd

chmod 600 ~/.vnc/passwd

echo 'DISPLAYS="root:1"' >> /etc/conf.d/tigervnc
echo 'DISPLAYS="root:1"' >> /etc/conf.d/vncserver

echo '{:1=root}' > /etc/tigervnc/vncserver.users


cat <<EOF >> /etc/tigervnc/vncserver-config-defaults
session=xfce
securitytypes=vncauth,tlsvnc
geometry=1920x1080
localhost=no
alwaysshared
EOF

cat <<EOF >> /etc/tigervnc/vncserver-config-mandatory
session=xfce
securitytypes=vncauth,tlsvnc
geometry=1920x1080
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
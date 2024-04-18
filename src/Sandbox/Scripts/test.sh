apk add tigervnc novnc
apk add --no-cache supervisor xfce4 xfce4-terminal xterm dbus-x11 dbus-glib openrc

apk add --no-cache vim wget net-tools libc-utils bzip2 procps python3 py3-numpy

apk add xorg-server xf86-input-libinput eudev mesa-dri-gallium

mkdir -p "$HOME/.vnc"

echo "vncpassword" | vncpasswd -f >> ~/.vnc/passwd

chmod 600 ~/.vnc/passwd


echo 'DISPLAYS="root:1"' > /etc/conf.d/tigervnc
echo 'DISPLAYS="root:1"' >> /etc/conf.d/vncserver

echo '{:1=root}' > /etc/tigervnc/vncserver.users

echo """session=xfce'
securitytypes=vncauth,tlsvnc
geometry=2000x1200
localhost
alwaysshared""" > /etc/tigervnc/vncserver-config-defaults

echo """session=xfce'
securitytypes=vncauth,tlsvnc
geometry=2000x1200
localhost
alwaysshared""" > /etc/tigervnc/vncserver-config-mandatory


mkdir -p /etc/X11/xorg.conf.d

# echo """Section "Module"
# 	Load "vnc"
# EndSection
# Section "Screen"
# 	Identifier "Default Screen"
# 	Option "PasswordFile" "/etc/X11/vncpasswd"
# EndSection
# """ > /etc/X11/xorg.conf.d/40-vnc.conf 

vim /etc/X11/xorg.conf.d/40-vnc.conf 
{
Section "Module"
	Load "vnc"
EndSection
Section "Screen"
	Identifier "Default Screen"
	Option "PasswordFile" "/etc/X11/vncpasswd"
EndSection
}

# vncpasswd /etc/X11/vncpasswd # нужно както-автоматизировать
printf "vncpassword\nvncpassword\n\n" | vncpasswd /etc/X11/vncpasswd

openrc boot

ln -s vncserver /etc/init.d/vncserver.1

rc-service vncserver.1 start





apk add alpine-conf
setup-xorg-base

apk update && apk add xorg-server xinit
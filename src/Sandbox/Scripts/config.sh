#!/bin/sh

set -e
set -u



echo 'DISPLAYS="root:1"' >> /etc/conf.d/tigervnc
echo 'DISPLAYS="root:1"' >> /etc/conf.d/vncserver

echo '{:1=root}' > /etc/tigervnc/vncserver.users


cat <<EOF >> /etc/tigervnc/vncserver-config-defaults
session=xfce
securitytypes=none
geometry=1080x720
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


cat <<EOF > /etc/xdg/xfce4/xfconf/xfce-perchannel-xml/xfce4-session.xml
<?xml version="1.0" encoding="UTF-8"?>

<channel name="xfce4-session" version="1.0">
  <property name="general" type="empty">
    <property name="FailsafeSessionName" type="string" value="Failsafe"/>
    <property name="LockCommand" type="bool" value="false"/>
  </property>
  <property name="sessions" type="empty">
    <property name="Failsafe" type="empty">
      <property name="IsFailsafe" type="bool" value="true"/>
      <property name="Count" type="int" value="5"/>
      <property name="Client0_Command" type="array">
        <value type="string" value="xfwm4"/>
        <value type="string" value="--replace"/>
      </property>
      <property name="Client0_Priority" type="int" value="15"/>
      <property name="Client0_PerScreen" type="bool" value="false"/>
      <property name="Client1_Command" type="array">
        <value type="string" value="xfsettingsd"/>
      </property>
      <property name="Client1_PerScreen" type="bool" value="false"/>
      <property name="Client2_PerScreen" type="bool" value="false"/>
      <property name="Client3_Command" type="array">
        <value type="string" value="Thunar"/>
        <value type="string" value="--daemon"/>
      </property>
      <property name="Client3_PerScreen" type="bool" value="false"/>
      <property name="Client4_Command" type="array">
        <value type="string" value="xfdesktop"/>
      </property>
      <property name="Client4_PerScreen" type="bool" value="false"/>
    </property>
  </property>
</channel>
EOF


printf "vncpassword\nvncpassword\n\n" | vncpasswd /etc/X11/vncpasswd

rm -rfv /tmp/.X*-lock /tmp/.X11-unix
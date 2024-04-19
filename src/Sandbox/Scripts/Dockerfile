# Используем базовый образ Alpine Linux
FROM alpine:3.19


RUN apk add --no-cache tigervnc novnc supervisor xfce4 xfce4-terminal xterm dbus-x11 dbus-glib openrc vim wget net-tools libc-utils bzip2 procps python3 py3-numpy
RUN apk add xorg-server xf86-input-libinput eudev mesa-dri-gallium

# RUN apk add alpine-conf
# RUN setup-xorg-base
COPY config.sh .

RUN /config.sh

RUN openrc boot
RUN  ln -s vncserver /etc/init.d/vncserver.1
RUN rc-service vncserver.1 start || true


EXPOSE 5901

# Запускаем supervisord для управления процессами
# CMD ["/usr/bin/supervisord", "-c", "/etc/supervisor/conf.d/supervisord.conf"]

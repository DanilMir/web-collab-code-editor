# Используем базовый образ Alpine Linux
FROM alpine:latest

# Обновляем индексы пакетов и устанавливаем необходимые пакеты
RUN apk update && \
    apk add --no-cache xfce4 xfce4-terminal tigervnc supervisor wget openbox \
    x11vnc xvfb dbus dbus-x11 novnc websockify bash

# Устанавливаем NoVNC
RUN wget -O noVNC.zip https://github.com/novnc/noVNC/archive/master.zip && \
    unzip noVNC.zip && \
    rm noVNC.zip && \
    mv noVNC-master /opt/noVNC

# Устанавливаем пароль для VNC
RUN mkdir ~/.vnc
RUN x11vnc -storepasswd 1234 ~/.vnc/passwd

# Копируем конфигурационные файлы
COPY supervisord.conf /etc/supervisor/conf.d/supervisord.conf
COPY xstartup /root/.vnc/xstartup

# Открываем порты для NoVNC и VNC
EXPOSE 8080
EXPOSE 5900

# Запускаем supervisord для управления процессами
CMD ["/usr/bin/supervisord", "-c", "/etc/supervisor/conf.d/supervisord.conf"]

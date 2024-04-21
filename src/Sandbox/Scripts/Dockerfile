FROM alpine:3.19

ENV DISPLAY=:1 \
    VNC_PORT=5901 \
    NO_VNC_PORT=6901
EXPOSE $VNC_PORT $NO_VNC_PORT

WORKDIR $HOME

RUN apk add --no-cache tigervnc novnc openrc xfce4 xfce4-terminal
RUN rm -rf /var/cache/apk/*
COPY --chmod=0755 config.sh .
COPY --chmod=0755 startapp.sh .

RUN ./config.sh

ENTRYPOINT [ "./startapp.sh" ]
CMD xfce4-terminal -e 'sh -c "ls /usr/bin && read"' -T "Run and ready" --zoom=2  --maximize --fullscreen --hide-menubar --hide-borders  --hide-toolbar

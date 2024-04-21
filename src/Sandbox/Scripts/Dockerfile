FROM alpine:3.19

ENV DISPLAY=:1 \
    VNC_PORT=5901 \
    NO_VNC_PORT=6901
EXPOSE $VNC_PORT $NO_VNC_PORT

WORKDIR $HOME

RUN apk add --no-cache tigervnc novnc openrc xfce4 xfce4-terminal
RUN apk add --no-cache dotnet7-sdk

COPY --chmod=0755 config.sh .
COPY --chmod=0755 startapp.sh .

RUN /config.sh

COPY ConsoleApp1.csproj /project/
COPY Program.cs /project/

RUN cd /project/ && dotnet build

ENTRYPOINT [ "./startapp.sh" ]
CMD xfce4-terminal -e 'sh -c "cd /project/ && dotnet run && read"' -T "Run and ready" --zoom=2  --maximize --fullscreen --hide-menubar --hide-borders  --hide-toolbar

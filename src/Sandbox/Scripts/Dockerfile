FROM alpine:3.19

#RUN apk add --no-cache add build-base curl make cmake git upx xterm

ENV DISPLAY=:1 \
    VNC_PORT=5901 \
    NO_VNC_PORT=6901
EXPOSE $VNC_PORT $NO_VNC_PORT

WORKDIR $HOME

RUN apk add --no-cache tigervnc novnc openrc xfce4 xterm xfce4-terminal
RUN apk add --no-cache dotnet7-sdk 

COPY --chmod=0755 config.sh .
COPY --chmod=0755 startapp.sh .

RUN /config.sh

COPY Program.cs /project/
COPY ConsoleApp1.csproj /project/

RUN cd /project/ && dotnet build

EXPOSE 6901

ENTRYPOINT [ "./startapp.sh" ]
CMD [ "/usr/bin/xterm", "-hold", "-e", "cd", "project", "&&", "dotnet", "run"]
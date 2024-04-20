FROM alpine:3.19

#RUN apk add --no-cache add build-base curl make cmake git upx xterm


RUN apk add --no-cache tigervnc novnc openrc xfce4 xterm xfce4-terminal
RUN apk add --no-cache dotnet7-sdk 
RUN apk add xhost xset cmatrix

# COPY --chmod=0755 config.sh .
COPY --chmod=0755 startapp.sh .

# RUN /config.sh

COPY Program.cs /project/
COPY ConsoleApp1.csproj /project/

RUN cd /project/ && dotnet build

EXPOSE 6901

ENTRYPOINT [ "./startapp.sh" ]
CMD [ "xterm", "-hold", "-e", "'cd project && dotnet run'"]
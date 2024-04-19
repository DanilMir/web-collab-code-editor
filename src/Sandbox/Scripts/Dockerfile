FROM alpine:3.19

RUN apk add --no-cache tigervnc novnc xfce4 openrc

COPY --chmod=0755 config.sh .
COPY --chmod=0755 startapp.sh .

RUN /config.sh


ENTRYPOINT [ "./startapp.sh" ]
CMD ["/bin/sh"]
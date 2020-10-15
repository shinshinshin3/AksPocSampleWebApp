FROM mcr.microsoft.com/mssql/server:2019-latest
COPY . /usr/src/docker

WORKDIR /usr/src/docker
USER root

RUN chown mssql ./db-init.sh
RUN chmod +x ./db-init.sh

CMD /bin/bash ./entrypoint.sh

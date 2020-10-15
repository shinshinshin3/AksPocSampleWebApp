#start SQL Server, start the script to create/setup the DB
# wait for the SQL Server to come up

sleep 60s

echo "running set up script"

#run the setup script to create the DB and the schema in the DB
/opt/mssql-tools/bin/sqlcmd -U sa -P P@ssw0rd -i ddl.sql


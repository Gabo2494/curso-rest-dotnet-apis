echo 'Creating DB'

RUN ( /opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'Aa123456' -i /usr/src/app/setup.sql \
    && pkill sqlservr 

echo 'Finish DB'
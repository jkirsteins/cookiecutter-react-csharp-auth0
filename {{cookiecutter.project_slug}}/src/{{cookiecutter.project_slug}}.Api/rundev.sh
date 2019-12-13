#!/bin/bash

VGF__RABBITMQ__CONNECTIONSTRING=none	\
VGF__RABBITMQ__QOS=10	\
VGF__MONGO__CONNECTIONSTRIN_sG=mongoconn	\
ASPNETCORE_URLS="http://0.0.0.0:5000;https://0.0.0.0:5001"    \
ASPNETCORE_ENVIRONMENT=Development  \
ASPNETCORE_FORWARDEDHEADERS_ENABLED=true    \
ASPNETCORE_Kestrel__Certificates__Default__Password=devcertpassword    \
ASPNETCORE_Kestrel__Certificates__Default__Path=/Users/janiskirsteins/.aspnet/https/aspnetapp.pfx  \
dotnet watch run

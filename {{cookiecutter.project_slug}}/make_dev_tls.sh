#!/bin/bash

set -e

if [ -e ".devcache/aspnet-https" ]; then
  echo "Please remove .devcache/aspnet-https"
  exit 1
fi

dotnet dev-certs https -ep ~/.aspnet/https/aspnetapp.pfx -p devcertpassword --trust
dotnet dev-certs https --check --trust

ln -svn ~/.aspnet/https .devcache/aspnet-https
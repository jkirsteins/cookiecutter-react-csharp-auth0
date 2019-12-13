#!/bin/bash

set -e

if [ ! -e ".devcache/aspnet-https" ]; then
  ./make_dev_tls.sh
fi

docker-compose up --abort-on-container-exit

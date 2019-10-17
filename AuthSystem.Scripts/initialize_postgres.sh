#!/bin/bash

docker run --name auth-postgres -e POSTGRES_PASSWORD=adminpass -e POSTGRES_DB=AuthSystem -d -p 5432:5432 postgres:12

sudo apt-get update -y

cd / || exit

# prequisite for evolve
sudo apt-get install libicu55 -y

sudo curl -L -o evolve.tar.gz https://github.com/lecaillon/Evolve/releases/download/2.3.0/evolve_2.3.0_Linux-64bit.tar.gz
sudo tar -zxvf evolve.tar.gz

cd "$OLDPWD" || exit # return to source root

sudo /evolve migrate postgresql -c "Server=127.0.0.1;Database=AuthSystem;User Id=postgres;Password=adminpass;" -l "AuthSystem.Scripts/migrations" -s public --metadata-table Evolve_Changelog

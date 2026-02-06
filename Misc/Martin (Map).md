# Martin (Map)

Map openstreetmap  pbf
https://download.geofabrik.de
https://planet.openstreetmap.org/

Openstreetmap to pgsql
https://osm2pgsql.org/examples/vector-tiles/
https://osm2pgsql.org/doc/manual.html#preparing-the-database

Lancer Pg avec postgis installé
docker run -d --name map -e POSTGRES_PASSWORD=test -e POSTGRES_DB=postgis -p 5432:5432 postgis/postgis

.\osm2pgsql.exe -c -U postgres -W  ..\ile-de-france-latest.osm.pbf

.\martin.exe postgresql://postgres:test@localhost/postgres

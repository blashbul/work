git clone https://github.com/systemed/tilemaker.git
cd tilemaker/
make
sudo make install
tilemaker --input /mnt/c/temp/new-york-latest.osm.pbf --output /mnt/c/temp/nyc.mbtiles --process ./resources/process-openmaptiles.lua --config ./resources/config-openmaptiles.json
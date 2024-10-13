#!/bin/bash

# Установка кодировки UTF-8
export LC_ALL=en_US.UTF-8

sleep 5
/usr/bin/screen -dmS ftorr /bin/bash -c 'cd /opt/AdTorrBot; ./FreeTorrBot'
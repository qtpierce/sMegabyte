#!/bin/sh
# File:  myweatherservice_wrapper.sh  is used to setup and launch the 
# actual weather service.
# Usage:  copy this file to /home/root/ directory and let the systemd
# system file called myweatherservice.service automatically launch it at boot up.

sleep 10  # This sleep is necessary.  I found that Edisons do horrible things 
# to Perl code when it is run immediately after boot up.  The sleep delay
# helps fix this horribleness. 

cd /home/root/
/home/root/myweatherservice.pl >> myweatherservice.log &


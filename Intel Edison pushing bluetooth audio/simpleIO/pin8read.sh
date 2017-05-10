#!/bin/sh
echo exporting files.
echo 49 > /sys/class/gpio/export
echo 256 > /sys/class/gpio/export
echo 224 > /sys/class/gpio/export
#echo 214 > /sys/class/gpio/export
echo writing directions
#echo low > /sys/class/gpio/gpio214/direction
echo low > /sys/class/gpio/gpio256/direction
echo high > /sys/class/gpio/gpio224/direction
echo in > /sys/class/gpio/gpio49/direction
#echo high > /sys/class/gpio/gpio214/direction
cat /sys/class/gpio/gpio49/value


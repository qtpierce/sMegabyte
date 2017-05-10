#!/bin/sh
echo exporting files.
echo 48 > /sys/class/gpio/export
echo 255 > /sys/class/gpio/export
echo 223 > /sys/class/gpio/export
echo 214 > /sys/class/gpio/export
echo writing directions
echo low > /sys/class/gpio/gpio214/direction
echo high > /sys/class/gpio/gpio255/direction
echo low > /sys/class/gpio/gpio223/direction
echo out > /sys/class/gpio/gpio48/direction
echo high > /sys/class/gpio/gpio214/direction
echo 0 > /sys/class/gpio/gpio48/value



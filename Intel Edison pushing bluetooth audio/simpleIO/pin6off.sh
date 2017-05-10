#!/bin/sh
echo exporting files.
echo 182 > /sys/class/gpio/export
echo 254 > /sys/class/gpio/export
echo 222 > /sys/class/gpio/export
#echo 214 > /sys/class/gpio/export
echo writing directions
#echo low > /sys/class/gpio/gpio214/direction
echo high > /sys/class/gpio/gpio254/direction
echo low > /sys/class/gpio/gpio222/direction
echo out > /sys/class/gpio/gpio182/direction
#echo high > /sys/class/gpio/gpio214/direction
echo 0 > /sys/class/gpio/gpio182/value


#!/bin/sh

# This script simply issues a sleep delay and then launches the mySync.pl script.

sleep 600

Script_Directory='/media/sdcard/my_scripts'
mySync_LOG='/media/sdcard/logs/mySync.log'

cd $Script_Directory
echo ------------------------------------------------------------------------------------ >> $mySync_LOG
date >> $mySync_LOG
$Script_Directory/mySync.pl >> $mySync_LOG 2>&1 &

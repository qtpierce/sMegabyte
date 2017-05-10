#!/bin/sh

# This script simply issues a sleep delay and then launches the Perl script.

# myMP3player.service was launched by systemd.  
# myMP3player.service launches this shell script.

# 11-22-2015:  I have discovered that systemd has a problem when it calls a Perl script.
# My work around is to have systemd call a shell script.  And the shell script calls my Perl script.

sleep 10  # Do I need this sleep?  Without it, the audio stops.

Script_Directory='/media/sdcard/my_scripts'
BTConnect_LOG='/media/sdcard/logs/BTConnect.log'
ControlsLauncher_LOG="/media/sdcard/logs/ControlsLauncher.log"

cd $Script_Directory
echo ------------------------------------------------------------------------------------ >> $BTConnect_LOG
date >> $BTConnect_LOG
perl $Script_Directory/BTConnect.pl >> $BTConnect_LOG 2>&1 &

echo ------------------------------------------------------------------------------------ >> $ControlsLauncher_LOG
date >> $ControlsLauncher_LOG
perl $Script_Directory/ControlsLauncher.pl >> $ControlsLauncher_LOG 2>&1 &

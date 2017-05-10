#!/usr/bin/perl

# This Perl script will setup the Bluetooth connection to the stereo.
# Then it announces the name of the Edison over the stereo.
# Finally it launches the Ogg Vorbis player Perl script.

# NOTE:  This script cannot be called directly by a systemd service.  I don't really know why.
# My workaround was to create a shell script that systemd calls.  That shell script calls this
# Perl script.

BEGIN
{
    use strict;
    $ENV{INC} = $ENV{INC} ."/media/sdcard/my_scripts/ ~";
}

use strict;
use warnings;
use Library qw (KillProcess LoadHash);

my $version = "20160930";

print "BTConnect.pl, version: $version\n";


use RCCar_Library;

# Load the configurations from hashes in the following files.
my $hashfile = "directories.pl";
Library::LoadHash ($hashfile);
$hashfile = "config.pl";
Library::LoadHash ($hashfile);





if (1)
{   # We must call rfkill as the first step of connecting to the radio over Bluetooth.
    print "Call rfkill to turn on (unblock) the bluetooth hardware.\n";
    my $rfkillTurnsOnBlueToothHW_cmd = "cd $Library::directories{Script_Directory}{DirectoryPath} ; rfkill unblock bluetooth; sleep 1";
    print "-D- cmd: $rfkillTurnsOnBlueToothHW_cmd\n";
    `$rfkillTurnsOnBlueToothHW_cmd`;  # Execute the rfkill command.
}



print "Use bluetoothctl to connect to the speaker.\n";
# This script assumes you, the user, have propery paired and trusted and tested the bluetooth connection 
# to the speaker.  It also assumes you've modified the RCCar_Library.pm file to contain the correct MAC
# address hard coded values too.
# NOTE:  Whitespaces in the bluetoothctl command  around the EOF tokens will break the command!

# I have two RC Cars and I need code that differentiates the two.
my $speakerMACAddress = RCCar_Library::Determine_Specific_BlueTooth_Speaker ();
my $bluetoothctlConnection_cmd = "cd $Library::directories{Script_Directory}{DirectoryPath} ; bluetoothctl << EOF\nconnect $speakerMACAddress\nEOF";
print "-D- cmd: $bluetoothctlConnection_cmd\n";
`$bluetoothctlConnection_cmd`;  # Execute the Bluetooth Connection command.



`sleep 5;`;  # Give the hardware time to resolve the previous commands.
print "Use pactl to set the audio sink to be the bluetooth speaker.\n";
# And pactl uses underscores for its punctuation, not colons.  So do a substitution.
my $underscored_speakerMACAddress = $speakerMACAddress;
$underscored_speakerMACAddress =~ s/:/_/g;
my $pactlAudioSink_cmd = "cd $Library::directories{Script_Directory}{DirectoryPath} ; pactl set-default-sink bluez_sink.${underscored_speakerMACAddress}";
print "-D- cmd: $pactlAudioSink_cmd\n";
`$pactlAudioSink_cmd`;  # Execute the Audio Sink command.



`sleep 1;`;  # Give the hardware time to resolve the previous commands.


if (1)
{   # For the fun of it, render the Edison's name as text-to-speech over the radio.
    RCCar_Library::Announce_Name ( $Library::directories{Script_Directory}{DirectoryPath} );
}


# By this step, the audio should be setup correctly.
print "Done with the bluetooth speaker.\n";
# So now launch some other stuff too.



# This is the real program that plays the Ogg Vorbis files.
print "\n\nLaunching myOggPlayer.pl\n\n";
`cd $Library::directories{Script_Directory}{DirectoryPath} ; ./myOggPlayer.pl >> $Library::directories{Log_Directory}{DirectoryPath}/myOggPlayer.log`;  # Execute the Ogg Vorbis playing Perl script.  
#  This should run continuously because myOggPlayer.pl only exits on an error.


print "\n\nDone playing music for some reason!\n";

exit 0;

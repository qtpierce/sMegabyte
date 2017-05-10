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

print "ControlLauncher.pl, version: $version\n";

use RCCar_Library;

# Load the configurations from hashes in the following files.
my $hashfile = "directories.pl";
Library::LoadHash ($hashfile);
$hashfile = "config.pl";
Library::LoadHash ($hashfile);



my $ethernetDevice = $Library::configs{EthernetDevice}{name};  # i.e.:  wlan0


if (1)  # If you want power-saving mode for Wifi, set this condition to 0.
{   # I turn off Wifi power-saving because I continually try to SSH to my system while in
    # use in order to program it.  If you are not rewriting code and not using the 
    # mySync.pl script to update files, then you can turn off this block of code.
    print "Edison's wifi uses a power saving mode that interferes with communication from an external computer.  So we turn it off.\n";
    my $turnOffPowerSavingsMode_cmd = "iwconfig $ethernetDevice power off; sleep 3";  # Use iwconfig to turn off the Edison's
    # power-saving mode.  If power-saving mode were on, then the Edison's Wifi would
    # power down and disrupt any possible wifi communication.
    print "-D- cmd: $turnOffPowerSavingsMode_cmd\n";
    `$turnOffPowerSavingsMode_cmd`;  # Execute iwconfig command.
}


sleep 10;  # This sleep is necessary to align launching myFakeControls.pl with the setup
# of the Bluetooth HW.


# Fake controls monitor the Bluetooth communmication and respond to button presses.
# This is how fast forward is handled.
print "\n\nLaunching myFakeControls.pl\n\n";  
`cd $Library::directories{Script_Directory}{DirectoryPath} ; ./myFakeControls.pl > $Library::directories{Log_Directory}{DirectoryPath}/myFakeControls.log &`;  # Execute and fork the fake controls script.


# CarStereo Shield Code controls the extra circuitry I put on the Arduino shield.
# This is how the power is held on after the key is removed.
print "\n\nLaunching myCarStereoShieldCode.pl\n\n";  
`cd $Library::directories{Script_Directory}{DirectoryPath} ; ./myCarStereoShieldCode.pl > $Library::directories{Log_Directory}{DirectoryPath}/myCarStereoShieldCode.log &`;  # Execute and fork the shield code script.




while (1)
{
    sleep 1;
}



exit 0;

#!/usr/bin/perl

BEGIN
{
    use strict;
    $ENV{INC} = $ENV{INC} ."/media/sdcard/my_scripts/ ~";
}

use strict;
use warnings;
use Library qw (KillProcess LoadHash);
use Getopt::Long;
use Data::Dumper;

my $opt_help;
my $opt_debug;
my $version = "20161017";

print "myCarStereoShieldCode.pl, version: $version\n";

GetOptions ("help|h" => \$opt_help,
            "debug|d" => \$opt_debug,
           );

$opt_help = defined ($opt_help) ? 1 : 0;
$opt_debug = defined ($opt_debug) ? 1 : 0;

if ($opt_help)
{
    print "help | h\n";
    exit 0;
}


# Load the configurations from hashes in the following files.
my $hashfile = "directories.pl";
Library::LoadHash ($hashfile);
$hashfile = "config.pl";
Library::LoadHash ($hashfile);

if ($opt_debug)
{
    print "\nHere's the directories\n";
    print Dumper (\%Library::directories);
    print "\nHere's the configs\n";
    print Dumper (\%Library::configs);
}


my $redirectSTDERR = " 2> /dev/null";
Initialization ();
WatchPinIgnition ();


exit 0;




sub Initialization
{
    if ($opt_debug) { print "-D- Entering: Initialization ()\n" };

    HoldPowerOn ();
    
    BlinkLEDNormal ();

    if ($opt_debug) { print "-D- Exiting: Initialization\n" };
}



sub WatchPinIgnition
{
    my $readPinIgnition_cmd = $Library::configs{Pin_Ignition_Signal}{read} . $redirectSTDERR;

    my $pinIgnitionState = -1;
    my $stateChangeOccurred = 0;

    while (1)
    {
        sleep (1);
	my $results = `$readPinIgnition_cmd`;

	my $tempPinState = undef;
	if ($results =~ /\n0/)
	{
            $tempPinState = 0;
	}
	if ($results =~ /\n1/)
	{
            $tempPinState = 1;
	}
	#print "-D- results: $results, tempPinState: $tempPinState, \n";

        if (defined $tempPinState)
	{
            $stateChangeOccurred = ($tempPinState != $pinIgnitionState) and ($pinIgnitionState != -1);
	    $pinIgnitionState = $tempPinState;
	}

	if ($stateChangeOccurred)
	{
            if ($opt_debug) { print "-D- pinIgnitionState changed to: $pinIgnitionState\n"; }
            $stateChangeOccurred = 0;
	    ProcessPinIgnitionState ($pinIgnitionState);
	}
    }
}



sub ProcessPinIgnitionState
{
    my ($pinIgnitionState) = @_;

    if ($pinIgnitionState)
    {
        BlinkLEDNormal ();
        CancelShutdown ();
	LaunchMusicProcesses ();
    }
    else
    {
        BlinkLEDSlow ();
        ScheduleShutdown ();
	KillMusicProcesses ();
	ScheduleMySync ();
    }
}



sub BlinkLEDNormal
{
    BlinkLEDWrapper ("-on 1 -off 1");
}




sub BlinkLEDSlow
{
    BlinkLEDWrapper ("-on 1 -off 3");
}




sub BlinkLEDWrapper
{
    my ($arguments) = @_;

    Library::KillProcess ("blinkLED");
    my $blinkLED_cmd = "./blinkLED.pl $arguments" . $redirectSTDERR;
    system ("$blinkLED_cmd &");
}




sub HoldPowerOn
{
    my $holdPowerOn_cmd = $Library::configs{Pin_Hold_Power_On}{on} . $redirectSTDERR;
    if ($opt_debug) { print "-D- cmd: $holdPowerOn_cmd\n" };
    system ("$holdPowerOn_cmd &");
}




sub ScheduleShutdown
{
    my $shutdown_cmd = "./sleepthenkill.pl -s 3600";
    print "Scheduling sleepthenkill shutdown.\n";
    if ($opt_debug) { print "-D- cmd: $shutdown_cmd\n" };
    system ("$shutdown_cmd &");
}



sub ScheduleMySync
{
#    my $sync_cmd = "cd $Library::directories{Script_Directory}{DirectoryPath} ; ./sleepthensync.sh &";
    my $sync_cmd = "./sleepthensync.sh";
    print "Scheduling MySync.\n";
    system ("$sync_cmd &");
}



sub CancelShutdown
{
    print "Cancelling shutdown.\n";
    Library::KillProcess ("sleepthenkill");
    Library::KillProcess ("sleepthensync");
}



sub KillMusicProcesses
{
    print "Killing the music processes.\n";
    Library::KillProcess ("myOggPlayer");
    Library::KillProcess ("gst-launch");
    Library::KillProcess ("BTConnect");
    Library::KillProcess ("myFakeControls");
}



sub LaunchMusicProcesses
{
    my $results = `ps`;
    if ($results !~ /BTConnect/)
    {
        print "Launching music processes.\n";
        #system ("sleep 10 ; cd $Library::directories{Script_Directory}{DirectoryPath} ; ./BTConnect.pl >> /media/sdcard/logs/BTConnect.log 2>&1 &");
        #system ("sleep 20 ; cd $Library::directories{Script_Directory}{DirectoryPath} ; ./myFakeControls.pl >> /media/sdcard/logs/myFakeControls.log 2>&1 &");
	`shutdown -r now`;
    }
}

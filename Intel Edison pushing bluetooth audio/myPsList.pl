#!/usr/bin/perl

# Uses ps to list all the expected processes.

BEGIN
{
    use strict;
    $ENV{INC} = $ENV{INC} ."/media/sdcard/my_scripts/ ~";
}

use strict;
use warnings;
use Library qw (LoadHash);
use Getopt::Long;
use Data::Dumper;

my $opt_help;
my $opt_debug;
my $version = "20161017";


GetOptions ("help|h" => \$opt_help,
            "debug|d" => \$opt_debug,
    );

$opt_help = defined ($opt_help) ? 1 : 0;
$opt_debug = defined ($opt_debug) ? 1 : 0;

if ($opt_help)
{
    print "myPsList.pl, version: $version\n";
    print "help | h\n";
    exit 0;
}


my @ExpectedProcesses = qw (BTConnect.pl myOggPlayer.pl gst-launch-1 ControlsLauncher.pl myFakeControls.pl myCarStereo blinkLED.pl sleepthenkill sleepthensync);

my $hashfile = "directories.pl";
Library::LoadHash ($hashfile);
$hashfile = "config.pl";
Library::LoadHash ($hashfile);

$Library::directories{PWD} = $ENV{PWD};
$Library::configs{foo} = 1;

if ($opt_debug)
{
    print "\nHere's the directories\n";
    print Dumper (\%Library::directories);
    print "\nHere's the configs\n";
    print Dumper (\%Library::configs);
}


Task ();




exit 0;



sub Task
{
    my $results = `ps`;
    foreach my $process (@ExpectedProcesses)
    {
        if ($results =~ /$process/)
	{
            print "-I- Found process: $process\n";
	}
	else
	{
            print "-E- Could NOT find process: $process\n";
	}
    }
}



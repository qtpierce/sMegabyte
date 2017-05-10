#!/usr/bin/perl

# Sleeps and then turns off the power-on pin.

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
my $version = "20170106";
my $opt_sleepTime;

print "sleephenkill.pl, version: $version\n";

GetOptions ("help|h" => \$opt_help,
            "debug|d" => \$opt_debug,
	    "sleepTime|s=i" => \$opt_sleepTime,
    );

$opt_help = defined ($opt_help) ? 1 : 0;
$opt_debug = defined ($opt_debug) ? 1 : 0;
$opt_sleepTime = defined ($opt_sleepTime) ? $opt_sleepTime : 1800;

if ($opt_help)
{
    print "help | h\n";
    print "debug | d\n";
    print "sleepTime | s <seconds>\n";
    exit 0;
}


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


Task ();




exit 0;



sub Task
{
    sleep ($opt_sleepTime);
    my $redirectSTDERR = " 2> /dev/null";
    #my $off_cmd = $Library::configs{Pin_Hold_Power_On}{off} . $redirectSTDERR;
    my $off_cmd = "shutdown -h now";  # a shutdown is good enough, it will deassert the GPIO pins.
    `$off_cmd`;
}



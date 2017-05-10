#!/usr/bin/perl

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
my $version = "20160930";
my $opt_onTime;
my $opt_offTime;
my $opt_rapid;

print "blinkLED.pl, version: $version\n";

GetOptions ("help|h" => \$opt_help,
            "debug|d" => \$opt_debug,
	    "onTime|on=f" => \$opt_onTime,
	    "offTime|off=f" => \$opt_offTime,
	    "rapid" => \$opt_rapid,
    );

$opt_help = defined ($opt_help) ? 1 : 0;
$opt_debug = defined ($opt_debug) ? 1 : 0;
$opt_onTime = defined ($opt_onTime) ? $opt_onTime : 1;
$opt_offTime = defined ($opt_offTime) ? $opt_offTime : 1;
$opt_rapid = defined ($opt_rapid) ? 1 : 0;

if ($opt_help)
{
    print "help | h\n";
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


Loop ();




exit 0;



sub Loop
{
    my $redirectSTDERR = " 2> /dev/null";
    my $on_cmd = $Library::configs{Pin_Activity_LED}{on} . $redirectSTDERR;
    my $off_cmd = $Library::configs{Pin_Activity_LED}{off} . $redirectSTDERR;
    my $onTime_cmd = "sleep ($opt_onTime)";
    my $offTime_cmd = "sleep ($opt_offTime)";

    if ($opt_rapid)
    {
        my $temp_onTime = $opt_onTime * 1000000;
        my $temp_offTime = $opt_offTime * 1000000;
	$onTime_cmd = "`usleep $temp_onTime`";
        $offTime_cmd = "`usleep $temp_offTime`";
    }

    while (1)
    {
        `$on_cmd`;
	eval ($onTime_cmd);
	`$off_cmd`;
	eval ($offTime_cmd);
    }
}



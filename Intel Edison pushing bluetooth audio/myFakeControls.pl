#!/usr/bin/perl

BEGIN
{
    use strict;
    $ENV{INC} = $ENV{INC} ."/media/sdcard/my_scripts/ ~";
}

use strict;
use warnings;
use Library qw (KillProcess);
use Getopt::Long;
use Data::Dumper;

my $opt_help;
my $opt_debug;
my $version = "20160912";

print "myFakeControls.pl, version: $version\n";

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

my %PlayListHash;
my $my_music_dir = "/media/sdcard/my_music";
my $hashfile = "/media/sdcard/my_scripts/PlayListHash.pm";
my $FileRegex = "\.ogg\$";


my $btmon_cmd = "hcidump -X";
open (my $BTMON, "-|", $btmon_cmd);
while (my $line = <$BTMON>)
{
    chomp $line;
    if ($opt_debug)
    {
        print "-D- line: $line\n";
    }
    my $goBackward = 0;
    my $goForward = 0;

    if ($line =~ / 00 48 7c /)
    {
        if ($line =~ / 00 48 7c 4b /)
        {   # Kenwood's forward button.
            $goForward = 1;
        }
        elsif ($line =~ / 00 48 7c 4c /)
        {   # Kenwood's backward button.
            $goBackward = 1;
        }
        elsif ($line =~ / 00 48 7c 44 /)
        {   # Pioneer's Play button.
            $goForward = 1;
        }
    }

    if ($goForward)
    {
        if (1 or $opt_debug)
        {
            print "-D- Received a Forward.\n";
        }
        # Kill gst-launch.
        Library::KillProcess ("gst-launch");
    }

    if ($goBackward)
    {
        if (1 or $opt_debug)
        {
            print "-D- Received a Backward.\n";
        }
	## LoadPlayListHash ($hashfile); 
        # Then modify the playlist hash.
        # Then write it back.
    }
#print Dumper (\%PlayListHash);

}

close $BTMON;

exit 0;


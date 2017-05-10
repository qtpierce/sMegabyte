#!/usr/bin/perl

BEGIN
{
    use strict;
    $ENV{INC} = $ENV{INC} ."/media/sdcard/my_scripts/ ~";
}

use strict;
use warnings;
use Library qw (GetSongInformation);
use Getopt::Long;
use Data::Dumper;

my $opt_help;
my $opt_debug;
my $opt_limit;

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


Library::GetSongInformation ();




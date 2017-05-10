#!/usr/bin/perl
use strict;
use warnings;

use Data::Dumper;
use Getopt::Long;

my $opt_debug;
my $opt_help;
my $opt_leftDir;
my $opt_rightDir;
my $opt_suppressCommon;
my $opt_suppressRight;
my $opt_quiet;

GetOptions ("debug|d" => \$opt_debug,
            "help|h" => \$opt_help,
	    "leftDir|l=s" => \$opt_leftDir,
	    "rightDir|r=s" => \$opt_rightDir,
	    "suppressCommon|sc" => \$opt_suppressCommon,
	    "suppressRight|sr" => \$opt_suppressRight,
	    "quiet|q" => \$opt_quiet,
           );

$opt_debug = defined ($opt_debug) ? 1 : 0;
$opt_help = defined ($opt_help) ? 1 : 0;
$opt_suppressCommon = defined ($opt_suppressCommon) ? 1 : 0;
$opt_suppressRight = defined ($opt_suppressRight) ? 1 : 0;
$opt_quiet = defined ($opt_quiet) ? 1 : 0;

if (!defined ($opt_leftDir) or !defined ($opt_rightDir))
{
    $opt_help = 1;
}

if ($opt_help)
{
    print "diffdir.pl will walk a left and a right directory and print the names of files\n";
    print "that exist in one and not the other.\n";
    print "\nUsage:\n";
    print "    debug | d     Print debug messages.\n";
    print "    help | h      Print this message and exit.\n";
    print "    leftDir | l   The left directory argument (the first).\n";
    print "    rightDir | r  The right directory argument (the second).\n";
    print "    suppressCommon | sc    Do not report common files.\n";
    print "    suppressRight | sr     Do not report for the right directory.\n";

    exit 0;
}


my %leftDirFiles;
my %rightDirFiles;


PackFilesIntoHash ($opt_leftDir, \%leftDirFiles);
PackFilesIntoHash ($opt_rightDir, \%rightDirFiles);
DiffHashes (\%leftDirFiles, \%rightDirFiles);

#print Dumper (\%leftDirFiles, \%rightDirFiles);

DisplayResults ($opt_leftDir, \%leftDirFiles);
if (!$opt_suppressRight)
{
    DisplayResults ($opt_rightDir, \%rightDirFiles);
}

exit 0;



sub PackFilesIntoHash
{
    my ($directory, $hash_ref) = @_;

    my $find_cmd = "find $directory";

    my $results = `$find_cmd`;

    my @fileList = split ("\n", $results);

    foreach my $file (@fileList)
    {
        if (FilesToIgnore ($file))
	{
            next;
	}

	my $length = length ($directory);
	#print "directory: $directory, length: $length\n";
	my $subString = substr $file, $length;
	$subString =~ s/^\///;

	if ($subString eq "")
	{
	    next;
	}
        $hash_ref->{$subString}{common} = 0;
	$hash_ref->{$subString}{path} = $file;
    }

    #print Dumper ($hash_ref);
}




sub DiffHashes
{
    my ($leftDirFiles, $rightDirFiles) = @_;

    foreach my $leftFile (keys %{$leftDirFiles})
    {
        if (exists $rightDirFiles->{$leftFile})
	{
            $leftDirFiles->{$leftFile}{common} = 1;
	    $rightDirFiles->{$leftFile}{common} = 1;
	}
    }
}



sub DisplayResults
{
    my ($directory, $hash_ref) = @_;

    if (!$opt_quiet)
    {
        print "\nFor directory: ". $directory .": \n";
    }
    
    foreach my $file (sort keys %{$hash_ref})
    {
        my $state = $hash_ref->{$file}{common} ? "common" : "unique";
	if ($opt_suppressCommon and $state eq "common")
	{
            next;
	}
	my $path = $hash_ref->{$file}{path};
	if (!$opt_quiet)
	{
	    print "file: $path, state: $state\n";
        }
	else
	{
            print "$path\n";
	}
    }
}



sub FilesToIgnore
{
    my ($file) = @_;

    my $shouldIgnore = 0;

    if ($file eq '.' or $file eq '..')
    {
        $shouldIgnore = 1;
    }

    return $shouldIgnore;
}

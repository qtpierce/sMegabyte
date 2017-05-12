#!/usr/bin/perl
use strict;
use warnings;
use threads;
use threads::shared;

use Data::Dumper;

use Getopt::Long;

my $opt_debug :shared;
my $opt_help;

GetOptions ("debug|d" => \$opt_debug,
            "help|h" => \$opt_help,
           );

$opt_debug = defined ($opt_debug) ? 1 : 0;
$opt_help = defined ($opt_help) ? 1 : 0;

if ($opt_help)
{
    print "ip_address_scanner will walk all possible ip addresses and report which are used.\n";
    print "\nUsage:\n";
    print "    debug | d     Print debug messages.\n";
    print "    help | h      Print this message and exit.\n\n";
    exit 0;
}


print "\nip_address_scanner\n";
my @emptyAddresses :shared;
my %usedAddresses :shared;
my $ipAddress;

my $OperatingSystem = DetermineOS( );

Scan( );

DisplayResults( );

exit 0;




sub DetermineOS
{
    my $OperatingSystem =  $^O;
    print "Operating System: $OperatingSystem\n";
    if ( $OperatingSystem =~ /Win/i )
    {
        return "Windows";
    }
    if ( $OperatingSystem =~ /linux/i )
    {
        return "Linux";
    }
}




sub Scan 
{
    for ($ipAddress = 0; $ipAddress < 256; $ipAddress++)
    {
        if ($ipAddress == 0 or $ipAddress == 255)
        {   # Don't test these addresses.
            next;
        }

        if ($opt_debug)
        {
            print "-D- Testing address: $ipAddress\n";
        }
        async (\&Ping)->detach;
    }

    Wait ();
}



sub Wait
{
    my $countScannedAddresses = scalar (@emptyAddresses) + scalar (keys %usedAddresses);
    if ($countScannedAddresses < 253)
    {   # The threads code runs 253 parallel threads and then exits that loop.
        # There must be a method for delaying until the threads are all done.
        sleep 1; 
    }
}




sub Ping
{
    my $cmd = "ping -c 2 -w 100 192.168.4.$ipAddress";
    
    if ($OperatingSystem eq "Windows")
    {
        $cmd = "ping -n 2 -w 100 192.168.4.$ipAddress";
    }

    my $result = `$cmd`;

    if ($result =~ /0 received/ or $result =~ /Received = 0/)
    {
        push (@emptyAddresses, "192.168.4.$ipAddress");
    }
    else
    {
        if ($opt_debug)
        {
            print "-D- cmd: $cmd\n";
            print "-D- result: $result\n";
        }
        $usedAddresses{"1921684$ipAddress"} = "192.168.4.$ipAddress";
    }
}



sub DisplayResults
{
    my $countUsedAddresses = scalar (keys %usedAddresses);
    print "\n\nUsed Addresses: $countUsedAddresses\n";

    foreach my $ipAddress (sort {$a <=> $b} keys (%usedAddresses))
    {
        print "  $usedAddresses{$ipAddress}\n";
    }
}


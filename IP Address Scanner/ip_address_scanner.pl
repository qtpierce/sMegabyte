#!/usr/bin/perl
use strict;
use warnings;
use threads;
use threads::shared;
use Data::Dumper;
use Getopt::Long;


my $opt_debug :shared;
my $opt_help;
my $version = "20170528";

print "\nip_address_scanner, version: $version\n";

GetOptions( "debug|d" => \$opt_debug,
            "help|h" => \$opt_help,
          );

$opt_debug = defined( $opt_debug ) ? 1 : 0;
$opt_help = defined( $opt_help ) ? 1 : 0;


my @emptyAddresses :shared;
my %usedAddresses :shared;
my $ipAddressLastOrdinal;
my $rootNetAddress;
my $operatingSystem;


MainBody( );
exit 0;




sub MainBody
{
    if( $opt_help )
    {
        PrintHelpAndExit( );
    }

    $operatingSystem = DetermineOS( );
    print "\n-I- Operating System: $operatingSystem\n";

    DetermineLocalHostIPAddress( );
    print "-I- Root Net Address: $rootNetAddress\n";

    ScanLoop( );

    DisplayResults( );
}




sub PrintHelpAndExit
{
    print "\nip_address_scanner will walk all possible ip addresses and report which are in use.\n";
    print "\nUsage:  ./ip_address_scanner.pl\n";
    print "    -debug | -d     Print debug messages.\n";
    print "    -help | -h      Print this message and exit.\n\n";
    exit 0;
}






sub DetermineOS
{   # This script is meant to be run on both Linux and Windows.  I developed
    # it on Windows 7 and I had installed Strawberry Perl as well.
    my $operatingSystemTemp =  $^O;

    if( $operatingSystemTemp =~ /Win/i )
    {
        return "Windows";
    }
    if( $operatingSystemTemp =~ /linux/i )
    {
        return "Linux";
    }
}




sub ScanLoop 
{
    # Assume we are skipping addresses 0 and 255 because those are reserved.
    for( $ipAddressLastOrdinal = 1; $ipAddressLastOrdinal < 255; $ipAddressLastOrdinal++ )
    {
        if( $opt_debug )
        {
            print "-D- Testing address: $ipAddressLastOrdinal\n";
        }
        async( \&IssuePing )->detach;
    }
    Wait( );
}






sub Wait
{   # The threads code runs 253 parallel threads and then exits that loop.
    # There must be a method for delaying until the threads are all done.
    my $countScannedAddresses = scalar( @emptyAddresses ) + scalar( keys %usedAddresses );
    if( $countScannedAddresses <= 253 )
    {   
        sleep 1; 
    }
}




sub IssuePing
{
    my $addressToTest = $rootNetAddress .".". $ipAddressLastOrdinal;
    my $packetCount = "-c 2";
    my $timeout = "-W 2";  # Time unit is seconds of timeout.
    
    if( $operatingSystem eq "Windows" )
    {
        $packetCount = "-n 2";
        $timeout = "-w 2000";  # Time unit is milliseconds of timeout.
    }

    my $cmd = "ping ". $packetCount ." ". $timeout ." ". $addressToTest;

    if( $opt_debug ) 
    {
        print "-I- ping cmd: $cmd\n";
    }

    my $result = `$cmd`;
    
    $result =~ /(\d) received/;
    my $receivedPackets = $1;
    if( $operatingSystem eq "Windows" )
    {
        $result =~ /Received = (\d)/;
        $receivedPackets = $1;
    }

    if( $result =~ /Request timed out/)
    {   # Windows time out case.
        push( @emptyAddresses, $addressToTest );
    }
    elsif( $receivedPackets == 1 or $receivedPackets == 2 )
    {   # Received a correct ping from the address.
        $usedAddresses{ $ipAddressLastOrdinal } = $addressToTest;
    }
    else
    {   # Received no ping from the address.
        push( @emptyAddresses, $addressToTest );
        if( $opt_debug )
        {
            sleep 1;
            open( my $log, '>>', "received_no_ping.log" );
            print $log "-D- Received no ping: $result\n";
            close $log;
        }
    }
}




sub DisplayResults
{
    my $countUsedAddresses = scalar( keys %usedAddresses );
    print "\n-I- Used Addresses: $countUsedAddresses\n";

    foreach my $ipAddress( sort {$a <=> $b} keys( %usedAddresses ) )
    {
        print "  $usedAddresses{$ipAddress}\n";
    }
    print "\n\nDone scanning IP Addresses.\n";


    if( $operatingSystem eq "Windows" )
    {
        print "Looping infinitely.\n";
        while (1)
        {
            sleep 1;
        }
    }
}




sub DetermineLocalHostIPAddress
{
    my $ipconfig_cmd = "ifconfig";
    if( $operatingSystem eq "Windows" )
    {
        $ipconfig_cmd = "ipconfig";
    }

    my $results = `$ipconfig_cmd`;
    my $lineWithIPAddress = CullLineWithIPAddress( $results );
    $rootNetAddress = ParseLineWithIPAddress( $lineWithIPAddress );
}




sub CullLineWithIPAddress
{
    my( $results ) = @_;
    my @lines = split( "\n", $results );
    foreach my $line( @lines )
    {   
        if( $line =~ /addr/i and $line =~ /192\./ )
        {   # Yes, it stops at the first line if finds.  I assume
            # that is either the local host address or the gateway
            # address.
            return $line;
        }
    }
}




sub ParseLineWithIPAddress
{
    my( $lineWithIPAddress ) = @_;
    chomp $lineWithIPAddress;
    $lineWithIPAddress =~ s/Bcast.*$//;  # For Linux, strip suffix.
    $lineWithIPAddress =~ s/^.*addr://;  # For Linux, strip prefix.
    $lineWithIPAddress =~ s/^.*\. : //;  # For Windows, strip prefix.

    # Strip the last ordinal of the IPAddress.
    my @pieces = split( '\.', $lineWithIPAddress );
    pop( @pieces );
    my $rootAddress = join( '.', @pieces );
   
    return $rootAddress;
}




# Notes.
# I'd originally developed this Perl script on Linux.  On a whim, I tried it on 
# Windows and Strawberry Perl and found it almost worked correctly.  I've since
# updated it to run on both Linux and on Windows 7 + Strawberry Perl.  

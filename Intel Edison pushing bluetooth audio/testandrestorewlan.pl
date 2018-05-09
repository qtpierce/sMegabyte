#!/usr/bin/perl
use strict;
use warnings;

# This program will try to keep the Wifi HW alive.
# It tries to connect to the server.  And failing that, tries to reset the connection.

my $hostIPAddress = "192.168.8.1";

while (1)
{   # Loop and test the connection.
    if (Test () == 0)
    {
        # PASSING.
    }
    else
    {
        # Else must reset wifi.
        Reset ();
    }
    sleep 60;
}


sub Test
{   # Use ping to test the connection.
    my $test_cmd = "ping $hostIPAddress -c 4 -q";
    print "-D- test_cmd: $test_cmd\n";
    my $results = `$test_cmd`;
    print "-D- results: $results\n";

    if ($results =~ /0 packets received/)
    {
        return 1;  # return 1 means a failing connection.
    }
    return 0;  # return 0 means a PASSING connection.
}


sub Reset
{   # Use ifconfig down and up to reset the connection.
    my $reset_cmd = "ifconfig wlan0 down; sleep 3; ifconfig wlan0 up";
    print "-D- reset_cmd: $reset_cmd\n";
    my $results = `$reset_cmd`;
    print "-D- results: $results\n";
}

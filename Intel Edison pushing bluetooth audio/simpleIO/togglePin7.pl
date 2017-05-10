#!/usr/bin/perl
use strict;
use warnings;

my $exportPins_cmd = "~/simpleIO/export_pins.sh";
`$exportPins_cmd`;

while (1)
{
    my $delay = 5;
    my $pin7_cmd = "~/simpleIO/pin7off.sh";
    `$pin7_cmd`;
    sleep ($delay);
    $pin7_cmd = "~/simpleIO/pin7on.sh";
    `$pin7_cmd`;
    sleep ($delay);
}

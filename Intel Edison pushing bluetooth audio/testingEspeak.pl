#!/usr/bin/perl

use strict;
use warnings;

my $message = "hey.
<break/> knock, knock.
<voice name=\"welsh\">
Who's there?
</voice>
Canoe.
Canoe who?
Canoe help me <break/> with my homework?
";

my $espeak_cmd = "espeak -w speech.wav -s 130 -a 200 -m  \"$message\"";
print "-D- espeak command:  $espeak_cmd\n";
`$espeak_cmd`;

sleep 1;

my $mplayer_cmd = "gst-launch-1.0 filesrc location=speech.wav ! wavparse ! audioconvert ! audioresample ! pulsesink";
print "-D- mplayer command:  $mplayer_cmd\n";
`$mplayer_cmd`;

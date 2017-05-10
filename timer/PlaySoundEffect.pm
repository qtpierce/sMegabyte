#! c:/utils/perl/bin/ 

use strict;
use Win32::Sound;

my $num_args = $#ARGV + 1;
if ($num_args == 1) {
    print "argument: $ARGV[0]\n";
    PlaySoundEffect ($ARGV[0]);
} else {
    print "Using the default wave file.\n";
    PlaySoundEffect ("1khz_tone.wav");
}



sub PlaySoundEffect {
    my ($audioFile) = @_;
    print "  Playing audio file: $audioFile.\n";
    Win32::Sound::Volume('100%');
    Win32::Sound::Play($audioFile);
    Win32::Sound::Stop();
    print "  Done playing the audio file.\n";
}



1;

#! c:/utils/perl/bin/ 

# timer.pl
# This is a timer.  It counts down time in seconds.
# Quentin Pierce
# 05-08-2017

our %skins;

use strict;  # strict forces me to scope variables correctly.
$| = 1;  # Turn off buffering.
use Carp qw ( cluck carp croak confess );  # Carp provides exception handling.
use Tk;
use handleargs;
use handleappearances;
use Time::Local;
use Cwd 'abs_path';


my @argscopy;
@argscopy = &findhelparg (@ARGV);

my $difference = 0;
my $Tminus1 = 0;

my $SECONDS_PER_MINUTE = 60;
my $SECONDS_PER_HOUR = $SECONDS_PER_MINUTE * 60;
my $SECONDS_PER_DAY = $SECONDS_PER_HOUR * 24;

my $timeStamp = time();
my $currentTime_seconds = time();
my $currentTime_formatd = formatTimeStrings ($currentTime_seconds);
my $currentTimeFontSize;
my $enableCountDown = 0;
my $desiredDelay = 0;
my $timeDifference_seconds = 0;
my $timeDifference_formatd = seconds2time ($timeDifference_seconds);
my $enablePlaySoundEffect = 0;
my $timeAlarmPoint_seconds = 0;
my $timeAlarmPoint_formatd = seconds2time ($timeAlarmPoint_seconds);
my $fontSizeScalar = 3.53;

my $mw = MainWindow->new;
$mw->title("Timer");
my $appearance = 0;
my $b1 = $mw->Button(-text => "Call timer",
            -command => sub { &toggleCountDown }, );
my $b2 = $mw->Button(-text => "Exit",
            -command => sub { exit 0 }, );
my $b3 = $mw->Button(-text => "Update",
            -command => sub { &updateEntry1FontSize}, );
my $b4 = $mw->Button(-text => "Appearance",
            -command => sub { &incrementAppearance}, );
my $b5 = $mw->Button(-text => "Stop",
            -command => sub { &stopTimer},
            -background => "red", );
my $chk1 = $mw->Checkbutton (-text => "Play a Soundfile.",
	    -variable => \$enablePlaySoundEffect );
$chk1 -> deselect ();


my $entry2 = $mw->Entry (-textvariable => \$skins{COMMON}->{fontSize},
                         -font => "{$skins{COMMON}->{fontName}} 14", -width => 7, );


my $timeAlarmPoint_label = $mw->Label (-text => "    Alarm Point:",
    );


my $entry1 = $mw->Entry (-textvariable => \$timeDifference_formatd, 
                 -font => "{$skins{COMMON}->{fontName}} $skins{COMMON}->{fontSize} bold", 
		 -width => 0, # 0 == automatic width.
                 -justify => 'center',
	 );

my $entry4 = $mw->Entry (-textvariable => \$timeAlarmPoint_formatd,
                         -font => "{$skins{COMMON}->{fontName}} 14", -width => 7, );

$currentTimeFontSize = $skins{COMMON}->{fontSize} / $fontSizeScalar;	 
my $entry3 = $mw->Entry (-textvariable => \$currentTime_formatd, 
                 -font => "{$skins{COMMON}->{fontName}} $currentTimeFontSize bold", 
		 -width => 0, # 0 == automatic width.
                 -justify => 'center',
	 );



$b1->grid($entry2, $b3, $b4, $b2, $b5);
$entry1->grid(-columnspan => 300);
$entry3->grid(-columnspan => 300);
$chk1->grid($timeAlarmPoint_label, $entry4);

if ($argscopy[0]) {
    $timeDifference_formatd = &convertToSeconds ($argscopy[0]);
    toggleCountDown ();
}


myLoop_1 ();
MainLoop;  # Their loop for the Perl TK GUI library's calling of their MainLoop.



sub myLoop_1 {
    my $loopTime = 15;  # This temporary assignment solves that problem with
    # the printing of message concerning TclObj_get int 900.
    Tk::after ($loopTime, \&myLoop_1);    # How to create a concurrent loop.
    # By using Tk::after ();

    my $difference;
    $currentTime_seconds = time();
    $difference = $currentTime_seconds - $timeStamp;
    if ($currentTime_seconds > $Tminus1) {
        $currentTime_formatd = formatTimeStrings ($currentTime_seconds);
        $Tminus1 = $currentTime_seconds;
    	if ($enableCountDown) {
            updateTimeVariables ();
	}
    }
    $mw->idletasks;
    $mw->update;
}



sub PlaySoundTone
{
    if ($enablePlaySoundEffect)
    {
        my $executable_directory = abs_path($0);
        $executable_directory =~ s/timer\.pl//;
        system 'start /b perl '. $executable_directory .'\PlaySoundEffect.pm 1khz_tone.wav';
    }
}



sub updateTimeVariables {
    my $currentTime_seconds = time();
    $timeDifference_seconds = $currentTime_seconds - $timeStamp;

    my $countDifference;
    $countDifference = $desiredDelay - $timeDifference_seconds;
    $timeDifference_formatd = seconds2time ($countDifference);
    $timeAlarmPoint_formatd = seconds2time ($timeAlarmPoint_seconds);


    if ($timeDifference_seconds == $desiredDelay - $timeAlarmPoint_seconds ) {
        if ($timeAlarmPoint_seconds > 0) {
            PlaySoundTone();
        }
    }

    if ($timeDifference_seconds == $desiredDelay -1) {
        PlaySoundTone();
    }

    if ($timeDifference_seconds >= $desiredDelay) {
        $enableCountDown = 0;
    }
}



sub stopTimer {
    $enableCountDown = 0;
    $b1->configure (-background => "gray95",);
}



sub SecondsRemainder 
{
    my ($totalSeconds) = @_;
    my $secondsRemainder = $totalSeconds % $SECONDS_PER_MINUTE;
}



sub MinutesElapsed
{
    my ($totalSeconds) = @_;
    my $minutesElapsed = int (( $totalSeconds % $SECONDS_PER_HOUR ) / $SECONDS_PER_MINUTE);
    return $minutesElapsed;
}



sub HoursElapsed
{
    my ($totalSeconds) = @_;
    my $hoursElapsed = int (( $totalSeconds % $SECONDS_PER_DAY ) / $SECONDS_PER_HOUR );
    return $hoursElapsed;
}



sub DaysElapsed
{
    my ($totalSeconds) = @_;
    my $daysElapsed = int ( $totalSeconds / $SECONDS_PER_DAY );
    return $daysElapsed;
}



sub seconds2time {
    use integer;	
    my ($totalSeconds) = @_;
    my $time = "";
    my $secondsRemainder = SecondsRemainder( $totalSeconds );
    # It is much too easy to accidentally allow time to be double counted here.
    # The use of modulo helps solve the double counting of time.
    my $minutesElapsed = MinutesElapsed( $totalSeconds );
    my $hoursElapsed = HoursElapsed( $totalSeconds );
    my $daysElapsed = DaysElapsed( $totalSeconds );
    if ($daysElapsed > 0) {
        $time = $daysElapsed ." days, ";
        $time = sprintf "%s %02d:%02d:%02d", $time, $hoursElapsed, $minutesElapsed, $secondsRemainder;
    } else {
        $time = sprintf "%02d:%02d:%02d", $hoursElapsed, $minutesElapsed, $secondsRemainder;
    }

    return $time;
}



sub time2seconds {
    use integer;
    my ($desiredTime) = @_;

    my $days = 0;
    my $hours = 0;
    my $minutes = 0;
    my $seconds = $desiredTime;
    if ($desiredTime =~ /:/) {
        my @split = split (/:/, $desiredTime);
	my $countsplit = @split;

	if ($countsplit == 4) {
            $days = $split[0];
            $hours = $split[1];
            $minutes = $split[2];
            $seconds = $split[3];
	}
	if ($countsplit == 3) {
	    $hours = $split[0];
            $minutes = $split[1];
            $seconds = $split[2];
        } elsif ($countsplit == 2) {
            $minutes = $split[0];
            $seconds = $split[1];            
	} elsif ($countsplit == 1 or $countsplit == 0) {
            $seconds = $split[0];
	}
    }
    # $desiredTime's units are seconds.
    $desiredTime = DaysToSeconds( $days ) + HoursToSeconds( $hours ) + MinutesToSeconds( $minutes ) + $seconds;
    return ($desiredTime);
}



sub MinutesToSeconds
{
    my ($minutes) = @_;
    return $minutes * $SECONDS_PER_MINUTE;
}



sub HoursToSeconds
{
    my ($hours) = @_;
    return $hours * $SECONDS_PER_HOUR;
}



sub DaysToSeconds
{
    my ($days) = @_;
    return $days * $SECONDS_PER_DAY;
}



sub convertToSeconds {
    my (@args) = @_;
    my $argument1 = $args[0];
    my $returnarg = 0;
    if ($argument1 =~ /([0-9]+)/) {
        $returnarg = $1;
        print ("The argument: $argument1, returnarg: $returnarg.\n");
    } else {
        print ("The argument: $argument1 was a bad argument.\n");
        exit;
    }
    return ($returnarg);
}



sub updateEntry1FontSize {
    $entry1->configure(-textvariable => \$timeDifference_formatd, 
                         -font => "{$skins{COMMON}->{fontName}} $skins{COMMON}->{fontSize} bold", );
    $currentTimeFontSize = $skins{COMMON}->{fontSize} / $fontSizeScalar;		 
    $entry3->configure(-textvariable => \$currentTime_formatd, 
                         -font => "{$skins{COMMON}->{fontName}} $currentTimeFontSize bold", );
}



sub incrementAppearance {
    if ($appearance >= ($skins{COMMON}->{numberOfSkins} - 1)) {
        $appearance = 0;
    } else {
        $appearance++;
    }
    &setAppearance();
}



sub setAppearance {
    $mw->configure(-background => $skins{$appearance}->{mw_background}, );
    $entry1->configure(-textvariable => \$timeDifference_formatd, 
            -font => "{$skins{COMMON}->{fontName}} $skins{COMMON}->{fontSize} bold", 
            -background => $skins{$appearance}->{entry1_background},
	    -foreground => $skins{$appearance}->{entry1_foreground}, 
            -insertbackground => $skins{$appearance}->{entry1_insertbackground}, 
            -relief => $skins{$appearance}->{entry1_relief}, );
    $currentTimeFontSize = $skins{COMMON}->{fontSize} / $fontSizeScalar;
    $entry3->configure(-textvariable => \$currentTime_formatd, 
            -font => "{$skins{COMMON}->{fontName}} $currentTimeFontSize bold", 
            -background => $skins{$appearance}->{entry1_background},
	    -foreground => $skins{$appearance}->{entry1_foreground}, 
            -insertbackground => $skins{$appearance}->{entry1_insertbackground}, 
            -relief => $skins{$appearance}->{entry1_relief}, );
    $chk1->configure(-background => $skins{$appearance}->{mw_background},
	    -foreground => $skins{$appearance}->{chk1_foreground}, 
            -selectcolor => $skins{$appearance}->{mw_background}, ); 
    $timeAlarmPoint_label->configure(-background => $skins{$appearance}->{mw_background},
	    -foreground => $skins{$appearance}->{chk1_foreground}, 
            ); 
}



sub toggleCountDown {
    $enableCountDown = !$enableCountDown;
    if ($enableCountDown) {
        $b1->configure (-background => "green",);
        $desiredDelay = time2seconds ($timeDifference_formatd);
        $timeAlarmPoint_seconds = time2seconds ($timeAlarmPoint_formatd);
	
        $timeStamp = time();
        if ($desiredDelay == 0) {
            $enableCountDown = 0;
            $b1->configure (-background => "gray95",);
        }
    } else {
        $b1->configure (-background => "gray95",);
    }
}



sub formatTimeStrings {
    my ($time) = @_;
    my $formatLocalTime = localtime ($time);

    $formatLocalTime =~ /(\w+) (\w+) \s*(\d+) (\d+):(\d+):(\d+) (\d\d\d\d)/;
    my $formatd = "$1, $2 $3 @ $4:$5:$6, $7";
    return $formatd;
}



exit 0;  # success.

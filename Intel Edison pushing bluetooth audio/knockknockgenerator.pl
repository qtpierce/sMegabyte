#!/usr/bin/perl

           
use strict;  
use warnings;
use Library qw (FindFiles WritePlayListHash);
use Getopt::Long;
use Data::Dumper;

my $CWD = "/media/sdcard/my_scripts";

my $opt_help; 
my $opt_debug;
my $opt_oneJoke;
my $opt_phrase;
my $opt_gender;
                                   
GetOptions ("help|h" => \$opt_help,  
            "debug|d" => \$opt_debug,
            "oneJoke|o" => \$opt_oneJoke,
            "phrase|p=s" => \$opt_phrase,
            "gender|g=s" => \$opt_gender,
           );
                                        
$opt_help = defined ($opt_help) ? 1 : 0;  
$opt_debug = defined ($opt_debug) ? 1 : 0;
$opt_oneJoke = defined ($opt_oneJoke) ? 1 : 0;
$opt_phrase = defined ($opt_phrase) ? $opt_phrase : "";
$opt_gender = defined ($opt_gender) ? $opt_gender : "m1";


if ($opt_help)
{                      
    print "knockknockgenerator.pl can text-to-speech knock knock jokes and phrases too.
  help | h           This help text.
  debug | d          Turn on debug printing.
  oneJoke | o        Tell 1 joke and then exit.
  phrase | p=s       Tell one phrase where the phrase is a string.
  gender | g=s       Set the gender of the voice per espeak's arguments. i.e.: f1 m1 ...
The default behavior of knockknockgenerator.pl is to infinitely loop through all the jokes in the jokes directory.  It will use both male (m1) and female (f1) voices to tell each joke.
";
    exit 0;
}


# An alternate mode in which a phrase is just generated and then exit.
if ($opt_phrase ne "")
{
    if ($opt_gender eq "")
    {
        die "When using -phrase, you must also use -gender too.\n";
    }
    Synthesize ($opt_phrase, $opt_gender);
    exit 0;
}


my %Jokes;
my $JokeDirectory = "$CWD/jokes";
my $JokeListHash = "$CWD/JokeListHash.pm";
my $FileRegex = "\.txt\$";

LoadPlayListHash ($JokeListHash);
print Dumper (\%Jokes);
Library::FindFiles ($JokeDirectory, \%Jokes, $FileRegex); 
LoadJokes ();

my $minimumTimesPlayed = Library::FindMinimumTimesPlayed (\%Jokes, $opt_debug);
my $numberOfJokes = 0;

foreach my $jokeFilePath (keys %{$Jokes{FileList}})
{
    my $numberOfTimesThisJokesBeenTold = $Jokes{FileList}{$jokeFilePath}{minimumTimesPlayed};
print "-D- minimumTimesPlayed: $minimumTimesPlayed, numberOfTimesThisJokesBeenTold: $numberOfTimesThisJokesBeenTold\n";    
    if ($numberOfTimesThisJokesBeenTold gt $minimumTimesPlayed)
    {
        next;
    }
    my @order = ("knock", "who", "name", "query", "joke");
    my @gender = ("m1", "f1", "m1", "f1", "m1");
    my $i = 0;
    foreach my $stage (@order)
    {
        PreWork ();
        my $line = $Jokes{FileList}{$jokeFilePath}{$stage};
        Synthesize ($line, $gender[$i]);
        $i++;
    }

    UpdatePlayList ($jokeFilePath);

    # This is old code that lit up an APPLAUSE sign I'd made as a joke.
    if ($opt_oneJoke)
    {
    #    my $activateApplause_cmd = "$CWD/simpleIO/pin7on.sh; sleep 5; $CWD/simpleIO/pin7off.sh";
    #     `$activateApplause_cmd`;
        exit 0;
    }
}

exit 0;



sub LoadPlayListHash                                                                               
{                                                                                                  
    my ($hashfile) = @_;                                                        
print "-D- hashfile: $hashfile\n";                                                                 
                                                                                                   
    if (-r $hashfile)                                                                      
    {                                                                                      
        $Data::Dumper::Purity = 1;                                                                 
        open FILE, $hashfile;                                                                      
        undef $/;                                                                                  
        eval <FILE>;                                                                       
        close FILE;                                                                                
    }                                                                                              
    else                                                                                           
    {                                                                                              
print "-D- did NOT find hashfile\n";
        # Else do nothing, a new file will be written soon enough.                                 
    }                                                                                              
}       




sub Synthesize
{
    my ($line, $gender) = @_;
    GenerateEspeakWav ($line, $gender);
    Callmplayer ();
}


sub LoadJokes
{
    foreach my $filename (keys %{$Jokes{FileList}})
    {
        wanted ($filename);
    }

    print Dumper (\%Jokes);
}



sub wanted
{
    my ($filename) = @_;  # is the current filename within that directory
    
    if ($filename eq "\.") { return; }
    if ($filename =~ /\.swp$/) { return; }
    AddJoke ($filename);
}


sub AddJoke
{
    my ($pathname) = @_;
    if (exists ($Jokes{FileList}{$pathname}) and exists ($Jokes{FileList}{$pathname}{knock}))
    {
        return;
    }

    open (FILE, "$pathname");
    my $i = 0;
    while (my $line = <FILE>)
    {
        chomp $line;
        print "-D- line: $line\n";
        if ($i == 0)
        {
            $Jokes{FileList}{$pathname}{knock} = $line;
        }
        elsif ($i == 1)
        {
            $Jokes{FileList}{$pathname}{who} = $line;
        }
        elsif ($i == 2)
        {
            $Jokes{FileList}{$pathname}{name} = $line;
        }
        elsif ($i == 3)
        {
            $Jokes{FileList}{$pathname}{query} = $line;
        }
        elsif ($i == 4)
        {
            $Jokes{FileList}{$pathname}{joke} = $line;
        }
        $i++;
    }
}


sub PreWork
{
    my $delete_cmd = "rm speech.wav";
    `$delete_cmd`;
}


sub GenerateEspeakWav
{
    my ($message, $gender) = @_;

    my $espeak_cmd = "espeak -w speech.wav -s 170 -a 200 -ven-us+${gender} -m  \"$message\"";
    print "-D- espeak command:  $espeak_cmd\n";
    `$espeak_cmd`;
}        


sub Callmplayer
{                                       
    #my $mplayer_cmd = "/usr/bin/mplayer speech.wav";
    my $mplayer_cmd = "gst-launch-1.0 filesrc location=$CWD/speech.wav ! wavparse ! audioconvert ! audioresample ! pulsesink & ";

    print "-D- mplayer command:  $mplayer_cmd\n";
    `$mplayer_cmd`;   
}



sub UpdatePlayList
{
    my ($jokeFilePath) = @_;
    $Jokes{FileList}{$jokeFilePath}{minimumTimesPlayed} += 1;
    Library::WritePlayListHash ($JokeListHash ,\%Jokes, "Jokes");  
}



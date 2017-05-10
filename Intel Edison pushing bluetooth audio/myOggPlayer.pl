#!/usr/bin/perl

BEGIN
{
    use strict;
    $ENV{INC} = $ENV{INC} ."/media/sdcard/my_scripts/ ~";
}

use strict;
use warnings;
use Library qw (FindFiles WritePlayListHash);
use Getopt::Long;
use Data::Dumper;

my $opt_help;
my $opt_debug;
my $opt_limit;

GetOptions ("help|h" => \$opt_help,
            "debug|d" => \$opt_debug,
            "limit|l=s" => \$opt_limit,
           );

$opt_help = defined ($opt_help) ? 1 : 0;
$opt_debug = defined ($opt_debug) ? 1 : 0;
$opt_limit = defined ($opt_limit) ? $opt_limit : 0;


my $version = "20160930";

print "myOggPlayer.pl, version: $version\n";


if ($opt_help)
{
    print "help | h\n";
    exit 0;
}

my %PlayListHash;                                                          
my %TemporaryHash;                                                         
my $my_music_dir = "/media/sdcard/my_music";                                         
my $hashfile = "/media/sdcard/my_scripts/PlayListHash.pm";                                                                    
my $FileRegex = "\.ogg\$";
my $songCounter = 0;

while (1)
{
    LoadPlayListHash ($hashfile);  
#print Dumper (\%PlayListHash);

    Library::FindFiles ($my_music_dir, \%PlayListHash, $FileRegex);
    PlayLoop ();
    print "DONE!  Played through once.\n";
    sleep 3;
}
exit 0;



sub LoadPlayListHash                                                                               
{   # This function must be in the main body (not in a library) because the simple way I load the 
    # playlist hash expects a global:  %PlayListHash                                                                                                  
    my ($hashfile) = @_;                                                        
                                                                                                  
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
        # Else do nothing, a new file will be written soon enough.                                 
    }                                                                                              
}       






sub PlayLoop
{
    my $minimumTimesPlayed = 0 + Library::FindMinimumTimesPlayed (\%PlayListHash, $opt_debug);
    foreach my $filename (keys (%{$PlayListHash{FileList}}))
    {
        my $timesPlayed = $PlayListHash{FileList}{$filename}{minimumTimesPlayed};
        if ($timesPlayed le $minimumTimesPlayed)
        {
            if ($opt_debug)                                                                                       
            {                                                                                                     
                print "-D- PLAYING song: $filename, minimumTimesPlayed: $minimumTimesPlayed, timesPlayed: $timesPlayed\n";
            }             

            # Test the music filename for existence.  The -e test uses a raw
            # filename, not a sanitized filename.
            my $testfilename = $filename;

            if (-e $testfilename)
            {            
                my $CMDfilename = SanitizeFileName ($filename);     
                $PlayListHash{FileList}{$filename}{minimumTimesPlayed} += 1;
                $PlayListHash{PlayOrder}{$filename} = $songCounter;
                Library::WritePlayListHash ($hashfile, \%PlayListHash, "PlayListHash");
                #my $PlayCMD = "/usr/bin/mplayer ". $CMDfilename ." </dev/null >/dev/null 2>\&1 ;";
		my $PlayCMD = "gst-launch-1.0 filesrc location=". $CMDfilename ." ! oggdemux ! vorbisdec ! audioconvert ! audioresample ! pulsesink ;";
                print "-I- Playing song: $filename\n";

                $songCounter++;
                if ($opt_debug)
                {   # Debug mode, just sleep and then move to next song.
                    print "-D- CMD: $PlayCMD\n";
                    sleep 5;
                }
                else
                {   # Normal mode, so play song.
                    `$PlayCMD`;
                }
            }
            else
            {
                $PlayListHash{FileList}{$filename}{errors} .= ", Could not read song.";
                print "-D- Could not read song: $testfilename\n";
            }
        }
        else
        {
            # Already played this once, skip it.
            if ($opt_debug)                                                                                       
            {                                                                                                     
                print "-D- SKIPPING song: $filename, minimumTimesPlayed: $minimumTimesPlayed, timesPlayed: $timesPlayed\n";
            }             
        }

        if ($opt_limit > 0 and $songCounter >= $opt_limit)
        {
            exit 0;
        }

        if ($songCounter > 0 and ($songCounter % 9) == 0)
        {
            $songCounter = 0;
            my $knockknock_cmd = "/media/sdcard/my_scripts/knockknockgenerator.pl -o";
            `$knockknock_cmd`;
        }
    }
}




sub SanitizeFileName
{
    my ($filename) = @_;
    $filename =~ s/\\/\\\\/g;
    $filename =~ s/'/\\'/g;
    $filename =~ s/ /\\ /g;
    $filename =~ s/\(/\\\(/g;
    $filename =~ s/\)/\\\)/g;
    return $filename;
}



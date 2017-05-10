package Library;

use strict;
use warnings;
use Data::Dumper;

use Exporter;
use vars qw (@EXPORT);

@EXPORT = qw(FindFiles LoadPlayListHash WritePlayListHash FindMinimumTimesPlayed KillProcess LoadHash GetInformation);

my %directories;
my %configs;



sub FindFiles
{
    my ($FileDirectory, $hash_ref, $FileRegex) = @_;

    my @tempFiles;
    opendir (DIR, $FileDirectory) or die "cannot open dir $FileDirectory: $!";

    @tempFiles = readdir DIR;

    closedir DIR;

    foreach my $filename (@tempFiles)
    {
        if ($filename eq "\." or $filename eq "\.\.")
        {
            next;
        }

        my $fullpath = $FileDirectory ."/". $filename;
        if ($filename =~ /$FileRegex/)
        {
            if (exists ($hash_ref->{FileList}{$fullpath}))
            {
            }
            else
            {
print "-D- hashlookup did not exist: $fullpath\n";
                $hash_ref->{FileList}{$fullpath}{minimumTimesPlayed} = 0;
                $hash_ref->{FileList}{$fullpath}{errors} = "";
            }
        }
        elsif (-d $fullpath)
        {
            # It's a directory, recurse.
            FindFiles ($FileDirectory ."/". $filename, $hash_ref, $FileRegex);
        }
    }
}



sub LoadPlayListHash
{   # This function must be in the main body (not in a library) because the simple way I load the
    # playlist hash expects a global:  %PlayListHash
    my ($hashfile) = @_;

    my %PlayListHash;

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

    return \%PlayListHash;
}




sub WritePlayListHash
{
    my ($hashfile, $hash_ref, $hashName) = @_;

    $Data::Dumper::Purity = 1;
    open FILE, ">$hashfile";
    print FILE Data::Dumper->Dump ([$hash_ref], ['*'. $hashName]);
    close FILE;
    `sync`;
}




sub FindMinimumTimesPlayed
{
    my ($hash_ref, $opt_debug) = @_;
    my $minimumTimesPlayed = 214748364;
    foreach my $filename (keys (%{$hash_ref->{FileList}}))
    {   # Loop through hash and look for the minimum.
        if ($hash_ref->{FileList}{$filename}{minimumTimesPlayed} < $minimumTimesPlayed)
        {
            if ($hash_ref->{FileList}{$filename}{errors} eq "")
            {
                $minimumTimesPlayed = $hash_ref->{FileList}{$filename}{minimumTimesPlayed};

                if ($opt_debug)
                {
                    print "-D- Resetting minimumTimesPlayed to: $minimumTimesPlayed\n";
                }
            }
        }
    }

    foreach my $filename (keys (%{$hash_ref->{FileList}}))
    {   # Now loop through the hash again and set all songs that were played alot to values just
        # higher than the minimum.  This effectively promotes newer songs to the front of the list
        # for the next playthrough and then they will have the same minimum as older songs after
        # new songs have been played once.
        if ($hash_ref->{FileList}{$filename}{minimumTimesPlayed} > $minimumTimesPlayed)
        {
            if ($hash_ref->{FileList}{$filename}{errors} eq "")
            {
                $hash_ref->{FileList}{$filename}{minimumTimesPlayed} = $minimumTimesPlayed + 1;
            }
        }
    }

    return $minimumTimesPlayed;
}



sub KillProcess
{
    my ($processName) = @_;

    my $ps_cmd = "ps | grep -i \"$processName\" | grep -v 'grep'" ;

    my $results = `$ps_cmd`;
    chomp $results;
    my @pieces = split ("\n", $results);

    foreach my $process (@pieces)
    {
        print "-D- KillProcess:: process: $process\n";
        if ($process =~ /^\s*(\d+) root/)
        {
            my $PID = $1;
            my $kill_cmd = "kill $PID";
            `$kill_cmd`;
        }
    }
}




sub LoadHash
{
    my ($hashfile) = @_;                                                        
    if (-r $hashfile)
    {
        $Data::Dumper::Purity = 1;
        open FILE, $hashfile;
        undef $/;
        my $results = eval <FILE>;
        close FILE;
    }
    else
    {
        # Else throw an error because we must have the config file.
    }
}




sub GetSongInformation
{
    my $processName = "gst-launch";
    my $ps_cmd = "ps | grep -i \"$processName\" | grep -v 'grep'" ;

    my $results = `$ps_cmd`;
    chomp $results;

    #print "-D- Library::GetSongInformation:: results: $results\n";

    my @pieces = split ("my_music/", $results);
    my $song = $pieces[-1];
    @pieces = split (" ! ", $song);
    $song = $pieces[0]; 
    print "-I- song: $song\n";
}



1;

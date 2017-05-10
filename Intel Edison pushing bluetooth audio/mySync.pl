#!/usr/bin/perl
use strict;
use warnings; 
#use File::Find;  # Edison does not have this library.

my $version = "20170505";


# This script will try to copy source files for the Edison Bluetooth car audio using SCP.

# Connection information to the file server.
# The $password and $username are the login credentials to the file server.
my $password = `cat ~/EdisonCarAudio/password`;  # Yes, the password is in clear text.
my $username = `cat ~/EdisonCarAudio/username`;
my $IPAddress = EscapePunctuation ( `cat ~/EdisonCarAudio/ipaddress` );  # IP address of the fileserver. 
my $sourceDirectory = EscapePunctuation (`cat ~/EdisonCarAudio/sourceDirectory` );    # The absolute path on the fileserver that contains the project source code. 

chomp $password;
chomp $username;
chomp $IPAddress;
chomp $sourceDirectory;


# Some directories we'll use on the Edison.
my $tempDirectory = "\/home\/root\/car_stereo_mount";
my $installationDirectory = "\/media\/sdcard";
my $my_music_dir = "/media/sdcard/my_music";
my $my_script_dir = "/media/sdcard/my_scripts";



print "mySync.pl, version: $version\n";

use Getopt::Long;
use Data::Dumper;

my $opt_help;
my $opt_debug;
my $opt_dontCopy;

GetOptions (
	    "help|h" => \$opt_help,
            "debug|d" => \$opt_debug,
	    "dontCopy|dc" => \$opt_dontCopy,
           );

$opt_help = defined ($opt_help) ? 1 : 0;
$opt_debug = defined ($opt_debug) ? 1 : 0;
$opt_dontCopy = defined ($opt_dontCopy) ? 1 : 0;


if ($opt_help)
{
    print "help | h\n";
    print "debug | d\n";
    print "dontCopy | dc\n";
    exit 0;
}



TestDirectoryStructure ();
if (PingServer())
{
    SCPFiles ();
    HandleDirectives ();
    DiffMyScripts ();
    DiffMyMusic ();
}
else
{
    exit 0;
}



sub SCPFiles
{   # Connect to the fileserver and copy down the project source code.
    my $rmdir_cmd = "rm -rf $tempDirectory";  # Delete the temp directory.
    my $mkdir_cmd = "$rmdir_cmd ; mkdir -p $tempDirectory";  # Make the temp directory.
    print "-I- mkdir_cmd: $mkdir_cmd\n";
    my $results = `$mkdir_cmd`;  # Execute the temp directory commands.

    if ($opt_debug)
    {
        chomp $results;
        print "-D- SCPFiles::  results: $results\n";
    }


    # Use sshpass to wrap around scp.  sshpass lets us work around a password requirements.
    # scp is how we copy down the project source code from the fileserver.
    my $scp_cmd = "sshpass -p ${password} scp -pr ". ${username} .'@'. ${IPAddress} .":${sourceDirectory}/* $tempDirectory";
    print "-I- scp_cmd: $scp_cmd\n";
    $results = `$scp_cmd`;  # Execute the scp command.
    chomp $results;

    if ($opt_debug)
    {
        print "-D- SCPFiles::  results: $results\n";
    }
}



sub HandleDirectives
{
    # find(\&wanted, $tempDirectory);
    my $ls_cmd = "ls -R $tempDirectory/my_music";
    my $results = `$ls_cmd`;

    if ($results =~ /deleteme/i)
    {
        HandleDirectiveDeleteme();
    }
}



sub HandleDirectiveDeleteme
{
    print "-W- Found a deleteme directive and therefore deleting the my_music content.\n";

    # Erase all of the /media/sdcard/my_music/* content.
    my $rm_cmd = "rm -rf $my_music_dir/* ; rm -f $tempDirectory/my_music/deleteme*";
    `$rm_cmd`;

    $rm_cmd = "rm -f $my_script_dir/PlayListHash*";
    `$rm_cmd`;
}



sub DiffMyScripts 
{   # Diff the downloaded scripts VS the local copy.  Differences will be copied
    # over.  Diff and ignore blank spaces, quietly list only filenames,
    my $targetDirectory = "$installationDirectory/my_scripts";
    my $diff_cmd = "diff -bqrsN $tempDirectory/my_scripts $targetDirectory";
    Diff_Function ($diff_cmd, $targetDirectory);
}



sub DiffMyMusic
{
    my $targetDirectory = "$installationDirectory/my_music";
    my $diff_cmd = "./dirdiff.pl -l $tempDirectory/my_music -r $targetDirectory -sr -sc";
    Diff_Function ($diff_cmd, $targetDirectory);
}



sub Diff_Function
{
    my ($diff_cmd, $targetDirectory) = @_;
    
    if ($opt_debug)
    {
        print "\n-D- diff_cmd: $diff_cmd\n\n";
    }
    
    my $results = `$diff_cmd`;  # Execute the diff command.
    chomp $results;

    # Turn the diff results into an array of files that are different.
    my @results = split ("\n", $results);

    foreach my $line (@results)
    {   # Each $line is a diff result containing 2 filenames. 
        if ($opt_debug)
        {
            if ($line !~ /identical/)
	    {
                print "-D- difference: $line\n";
            }
	    else
	    {
                print "-D- indentical line: $line\n";
	    }
        }

        # Remove content from the diff line to turn it into a pair of filenames.
        if ($line =~ /differ$/)
        {   # Differences means the new file will be copied over.
            my $copyFiles = $line;
            $copyFiles =~ s# and \/# \/#;
            $copyFiles =~ s# differ$##;
            $copyFiles =~ s#^Files ##;

            # $copyFiles were reduced to a pair of filenames.
            if (!FilesToIgnore ($copyFiles))
            {   # Some files were supposed to be ignored. i.e.: logs and play lists.
                CopyCommand ($copyFiles, 0);
            }
        }
        elsif ($line =~ /Only in/ and $line !~ m#$installationDirectory#)
        {   # 'Only in' means the new file must be copied over.
            my $copyFiles = $line;
            $copyFiles =~ m#Only in (.+?): (.+?)$#;
            my $path = $1;
            my $filename = $2;

            $copyFiles = "$path\/$filename $targetDirectory/$filename ";

            if (!FilesToIgnore ($filename))
            {
                CopyCommand ($copyFiles, 0);
            }
        }
	elsif ($line =~ /unique$/)
	{
            #print "dirdiff found: $line\n";
            my $copyFiles = $line;
	    $copyFiles =~ m#file: (.+?), state:.*#;
	    my $filepath = SanitizeFileName ($1);
	    my $destination = $filepath;
	    $destination =~ s/$tempDirectory/$installationDirectory/;

	    my $exists = IsFile ($destination);
	    my $isDirectory = IsDirectory ($filepath);
	    print "-D- processing: $filepath and $destination, isDirectory: $isDirectory, exists: $exists\n";
	    if ($destination =~ /\.ogg/ and $exists)
	    {
                print "-I- File exists: $destination\n"; 
                next;
	    }
	    
	    if ($isDirectory and !$exists)
	    {
                my $mkdir_cmd = "mkdir -p $destination";
		print "-D- mkdir: $mkdir_cmd\n";
		`$mkdir_cmd`;
	    }
            elsif (!FilesToIgnore ($filepath))
            {
                CopyCommand ("$filepath $destination", 0);
            }
        }
    }
}




sub CopyCommand
{
    my ($copyFiles, $recursive) = @_;

    $recursive = ($recursive) ? "-r" : "";
    
    my $cp_cmd = "cp $recursive $copyFiles";
    print "-I- cp_cmd: $cp_cmd\n";

    if (!$opt_dontCopy)
    {
        `$cp_cmd`;  # Execute the copy command.
    }
}




sub FilesToIgnore
{   # Given a filename, return whether it is in the list of files to ignore or not.
    my ($filename) = @_;

    my @ignoreRegexs = ("PlayListHash", "JokeListHash", "\.wav", "\.mp3");

    if ($filename =~ /^\./ or $filename =~ /\/\./)
    {   # Also ignore hidden files like vim backup files.
        return 1;
    }

    if ($filename =~ /~$/)
    {   # Also ignore vim *~ files too.
        return 1;
    }
    
    foreach my $ignoreRegex (@ignoreRegexs)
    {
        if ($filename =~ /$ignoreRegex/)
        {
            return 1;  # return 1, ignore this file.
        }
    }

    return 0;  # return 0, this file is important.
}



sub SanitizeFileName
{   # Add escapes to the filenames.
    my ($filename) = @_;
    $filename =~ s/\\/\\\\/g;
    $filename =~ s/'/\\'/g;
    $filename =~ s/ /\\ /g;
    $filename =~ s/\(/\\\(/g;
    $filename =~ s/\)/\\\)/g;
    $filename =~ s/&/\\&/g;
    return $filename;
}




sub IsDirectory
{
    my ($filepath) = @_;
    my $results = FileStat ($filepath);

    my $isDirectory = 0;
    if ($results =~ /directory/)
    {
        $isDirectory = 1;
    }
    return $isDirectory;
}



sub FileStat
{
    my ($filepath) = @_;
    my $stat_cmd = "stat $filepath 2>&1";

    my $results = `$stat_cmd`;
    return $results;
}


sub IsFile
{
    my ($filepath) = @_;
    my $results = FileStat ($filepath);

    my $isFile = 0;
    if ($results =~ /regular file/)
    {
        $isFile = 1;
    }
    return $isFile;
}




sub TestDirectoryStructure
{
    my @directoryStructure = ("$installationDirectory", "$installationDirectory/logs", "$installationDirectory/my_music", "$installationDirectory/my_scripts", "$installationDirectory/my_scripts/jokes", "$installationDirectory/my_scripts/simpleIO");
    
    foreach my $directoryPath (@directoryStructure)
    {
        if (!DirectoryExists ($directoryPath))
	    {
            my $mkdir_cmd = "mkdir $directoryPath";
	        `$mkdir_cmd`;
	    }
    }
}




sub DirectoryExists
{
    my ($directoryPath) = @_;

    my $exists = 0;

    if (-e $directoryPath)
    {
        $exists = 1;
    }

    return $exists;
}



sub PingServer
{
    my $PingCmd = "ping -c 4 $IPAddress";
    my $results = `$PingCmd`;
    #print "-D- results: $results\n";
    
    if ($results =~ /(\d) packets received/ and $1 > 0)
    {
        return 1;
    }
    else
    {
        print "\n-W- mySync.pl:  The host server could not be found by pinging, so mySync.pl is exiting without performing a sync.\n\n";
        return 0;
    }
}




sub EscapePunctuation
{
    my ($string) = @_;

    $string =~ s/\./\\\./g;
    $string =~ s/\//\\\//g;

    return $string;
}

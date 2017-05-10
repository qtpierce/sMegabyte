#! c:/utils/strawberry/perl/bin/

# Lists all the fonts found on the computer and listed in the Registry.

# Usage:  
#   1.  Copy this script to a computer's c:/temp/ directory.
#   2.  Open up cmd.exe and cd to the location.
#   3.  Run the script locally.


use strict;
use Win32::TieRegistry;

$|=1;

$Registry->Delimiter("\\");  # Set delimiter to "/".

my $fontskey = $Registry->{
        "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Fonts"
    } || die $^E;

for my $val ($fontskey->ValueNames)
{
    print "$val\n";
}

print "\n\nUse Ctrl+C to exit the final loop.\n";

while (1)
{
    ;
}

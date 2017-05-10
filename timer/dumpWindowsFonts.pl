#! c:/utils/strawberry/perl/bin/
#
use strict;
use Win32::TieRegistry;
$Registry->Delimiter("\\"); # Set delimiter to "/".
$|=1;
my $fontskey = $Registry->{"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Fonts"} || die $^E;
for my $val ($fontskey->ValueNames)
{
    print "$val\n";
}
exit;

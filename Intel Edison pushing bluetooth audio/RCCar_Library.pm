package RCCar_Library;

use strict;
use warnings;
use Data::Dumper;

use Exporter;
use vars qw (@EXPORT);

@EXPORT = qw(Determine_Gender_of_Voice Determine_Specific_BlueTooth_Speaker Announce_Name);


# I have two RC Cars and I need code that differentiates the two.
sub Determine_Gender_of_Voice
{   # The voice we render for text-to-speech generation is based on the name of the
    # car.  The name of the car is the hostname.  There are blue and purple cars.
    my $hostname = `hostname`;

    my $gender = "m1";
    if ($hostname =~ /Purple/)
    {
       $gender = "f1";
    }
    return $gender;
}



# I have two RC Cars and I need code that differentiates the two.
sub Determine_Specific_BlueTooth_Speaker
{   # The two cars each have specific Bluetooth speakers with specific MAC Addresses.
    # Here I use the hostname to determine the car the code is running on and assign
    # the appropriate MAC Address.
    my $hostname = `hostname`;

    my $speakerMACAddress = "30:22:22:05:D7:F4";  # EnerPlex speaker on RCCarBlue.
    if ($hostname =~ /Purple/)
    {
       $speakerMACAddress = "30:22:00:2F:A5:A3";  # EnerPlex speaker on RCCarPurple.
    }
    elsif ($hostname =~ /EdisonTruck/)
    {
       $speakerMACAddress = "74:5E:1C:C5:77:D5";  # Truck radio
    }
    elsif ($hostname =~ /EdisonCar/)                                            
    {                                                                           
        $speakerMACAddress = "74:5E:1C:C5:9B:F4";  # MR2 radio                  
    }                                                                           
    elsif ($hostname =~ /EdisonBench/)
    {
        $speakerMACAddress = "00:1D:86:5A:64:2C";  # Kenwood Bench radio
    }

    return $speakerMACAddress;
}



sub Announce_Name
{   # Given a directory to the knockknockgenerator.pl, determine the Edison's hostname and call the
    # text-to-speech generation on that name.
    my ($CWD) = @_;
    my $hostname = `hostname`;
    chomp $hostname;
    my $gender = RCCar_Library::Determine_Gender_of_Voice ();
    my $announceName_cmd = "cd $CWD ; ./knockknockgenerator.pl -phrase \"$hostname\" -gender $gender";
    print "-D- cmd: $announceName_cmd\n";
    my $results = `$announceName_cmd`;
    print "-D- results: $results\n";
}


1;

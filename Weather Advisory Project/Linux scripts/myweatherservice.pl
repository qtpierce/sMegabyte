#!/usr/bin/perl
# File:  myweatherservice.pl   is the business logic of the weather service.
# Usage:  copy it to /home/root/ directory and allow the systemd system and
# and the myweatherservice_wrapper.sh to call it.

# This program will run in a continuous loop on the Linux host side.  
# Every 2 minutes, it will query the internet for information on the
# weather.  It will toggle the LCD display between the temperature and
# the general conditions.  

use strict;
use warnings;
use Data::Dumper;

print "\nmyweatherservice.pl\n";

my %WeatherInfoHash;  # This is a hash and a global variable as well.

my $counter = 0;
my $station = "";

while (1)
{   # This is the infinite loop.

    CallWGet ();  # Call the function that fetches the weather information
    # from the internet.

    HashWebPage ();  # Call the function that packs all the weather 
    # information into a hash structure.
    
    for (my $i = 0; $i < 3; $i++)
    {   # Loop 3 times, updating the shared data file.
        UpdateSharedFile ();  # Call the function that updates the
        # shared data file.
    }

    $counter++;
}


sub CallWGet 
{
    # KHIO is the callsign for the Hillsboro Airport.
    # PANC is the callsign for the Anchorage International Airport.
    my $URL;
    if (($counter % 2) == 0) 
    {
        $URL = "http://w1.weather.gov/xml/current_obs/KHIO.xml";
        $station = "KHIO";
    }
    else
    {
        $URL = "http://w1.weather.gov/xml/current_obs/PANC.xml";
        $station = "PANC";
    }

    # Wget will fetch a webpage from the internet and write it out
    # as a text file. 
    my $wgetCmd = "wget $URL --output-document weather.xml ";
    my $webResults = `$wgetCmd`;  # webResults is not really being used.
}


sub HashWebPage
{   # Pack the weather information into the global hash.
    open (FILE, "weather.xml");  # Open up the shared data file.
    while (my $line = <FILE>) {  # Read through the whole shared data file.
        chomp($line);
        if ($line =~ /<(.+?)>(.+)<(.+?)>/)  # Parse every line.
        {
            my $tagName = $1;  # First, parse out the name of the XML tag.
            my $value = $2;  # Second, parse out the value.
            my $tagClosing = $3;  # Third, parse the closing tag.

            $WeatherInfoHash {$tagName} = $value;  # Write the tag name and value
            # to the hash.  Now this 1 piece of weather information is
            # stored in the global hash for use later.
        }
    }
    close (FILE);  # Close the shared data file because we are done with it.
}


sub UpdateSharedFile 
{   # Update the shared data file.

    # Below is an example of the weather information data.
    # <temperature_string>43.0 F (6.1 C)</temperature_string>
    # <temp_f>43.0</temp_f>
    # <temp_c>6.1</temp_c>
    # <relative_humidity>93</relative_humidity>
    # <wind_string>Calm</wind_string>
    #print Dumper (\%WeatherInfoHash);

    my $temperature = $WeatherInfoHash {temp_f};
    my $weather = $WeatherInfoHash {weather};

    # Print the weather information to the screen.
    print "station:  $station\n";
    print "temp:  $temperature F\n";
    print "weather:  $weather\n";

    # Open the file, write out the temperature.
    open (SHAREDFILE, ">/tmp/sharedfile");
    print SHAREDFILE "$station $temperature F               \n";
    close SHAREDFILE;
    sleep 10;

    # Open the file, write out the weather conditions.
    open (SHAREDFILE, ">/tmp/sharedfile");
    print SHAREDFILE "$station $weather                     \n";
    close SHAREDFILE;
    sleep 10;
}

sub findhelparg {
    my (@args) = @_;
    my @returnargs;
    my $numberofargs;
    my $helpwasprinted = 0;

    foreach my $element (@args) {
        if ($element =~ /-h|-help/) {
            &printhelp ();
            $helpwasprinted = 1;
        } else {
            push (@returnargs, $element);
        }
    }
    $numberofargs = @returnargs;

    return (@returnargs);
}


sub printhelp {
    print "timer.pl\n";
    print "\nusage:   timer {<desired countdown time> <-h>}\n";
    print "\n    To exit timer, please use the exit button.\n";
    print "\n    -h, --help       Prints the help.\n";
    print "\nTimer will count time down from an initial value to 0.\n"; 
    print "\n";
}


1;

$skins{COMMON} = {
    fontSize => 60,
    #fontName => "Nimubus Mono L",
    fontName => "Times New Roman",
    #fontName => "System",
    currentTimeFontSize => ($skins{COMMON}->{fontSize} / 2),

    numberOfSkins => 7,
};
# The font Utopia 122 bold looks good.
# The font {Bitstream Vera Serif} 122 bold looks good.
# The font {Luxi Mono} 122 bold looks good.
# The font {SUSE Serif} 122 bold looks good.
# How to put whitespace in the argument:  -font => "{Luxi Serif} 122 bold"


# website documenting color names:   http://www.rtapo.com/notes/named_colors.html
$skins{0} = {
    name => "Default",	
    mw_background => 'grey95',
    entry1_background => 'white',
    entry1_foreground => 'black',
    entry1_insertbackground => 'black',
    entry1_relief => 'sunken',
    chk1_foreground => 'black',
};

$skins{1} = {
    name => "Black",
    mw_background => 'black',
    entry1_background => 'black',
    entry1_foreground => 'green',
    entry1_insertbackground => 'gray',
    entry1_relief => 'flat',
    chk1_foreground => 'green',    
};

$skins{2} = {
    name => "Red",
    mw_background => 'red',
    entry1_background => 'red',
    entry1_foreground => 'white',
    entry1_insertbackground => 'black',
    entry1_relief => 'flat',
    chk1_foreground => 'black',    
};

$skins{3} = {
    name => "Blue",
    mw_background => 'blue',
    entry1_background => 'blue',
    entry1_foreground => 'yellow',
    entry1_insertbackground => 'gray',
    entry1_relief => 'flat',
    chk1_foreground => 'black',    
};

$skins{4} = {
    name => "Maroon",
    mw_background => 'maroon',
    entry1_background => 'maroon',
    entry1_foreground => 'pink',
    entry1_insertbackground => 'white',
    entry1_relief => 'flat',
    chk1_foreground => 'black',
};

$skins{5} = {
    name => "Gold",
    mw_background => 'Gold',
    entry1_background => 'Gold',
    entry1_foreground => 'DarkViolet',
    entry1_insertbackground => 'white',
    entry1_relief => 'flat',
    chk1_foreground => 'black',
};

$skins{6} = {
    name => "SteelBlue",
    mw_background => 'SteelBlue',
    entry1_background => 'SteelBlue',
    entry1_foreground => 'Navy',
    entry1_insertbackground => 'LightSteelBlue',
    entry1_relief => 'flat',
    chk1_foreground => 'black',
};



1;

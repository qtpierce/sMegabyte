our %configs = (
          'Pin_Activity_LED' => {
                                  'Pin' => 6,
                                  'Description' => "Active Low connection to an LED on the circuit board.  Write a 0 to this to indicate something is happening.",
				  'on' => $Library::directories{simpleIO_Directory}{DirectoryPath}.'/pin6off.sh',
				  'off' => $Library::directories{simpleIO_Directory}{DirectoryPath}.'/pin6on.sh',
                                },
          'Pin_Hold_Power_On' => {
                                   'Pin' => 7,
                                   'Description' => "Active High connection to a transistor on the circuit board.  Write a 1 to this to keep power on even when the key is removed.",
				   'on' => $Library::directories{simpleIO_Directory}{DirectoryPath}.'/pin7on.sh',
				   'off' => $Library::directories{simpleIO_Directory}{DirectoryPath}.'/pin7off.sh',
                                 },
          'Pin_Ignition_Signal' => {
                                     'Pin' => 8,
                                     'Description' => "Active High connection to the key-in-ignition signal on the circuit board.  Read this to determine whether the key is in or not.",
				     'read' => $Library::directories{simpleIO_Directory}{DirectoryPath}.'/pin8read.sh',
                                   },
          'EthernetDevice' => {
                                     'name' => "wlan0",
                                     'Description' => "The name of the wifi ethernet device.  This is used to turn off power saving features which cause connection problems.",
                                   },

	   );


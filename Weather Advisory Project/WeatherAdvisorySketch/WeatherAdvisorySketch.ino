// File:  WeatherAdvisorySketch.ino   is the Arduino sketch that ought to be loaded
// onto the Arduino side of the Intel Edison.  It queries a text file for a string and
// writes the string to the LCD.
// Usage:  Use the Arduino IDE to install this sketch on the Edison.

#include <Wire.h>
#include "rgb_lcd.h"

rgb_lcd lcd;

// This is a white color.
const int colorR = 200;
const int colorG = 200;
const int colorB = 200;
   
char abBuf[256];  
char catCmd [] = "cat /tmp/sharedfile";
int catCmdLength = strlen (catCmd);


int speakerPin = 3;   
int mydelay=100;  
char mylastletter='0';



void setup() {  
    // set up the LCD's number of columns and rows:
    lcd.begin(16, 2);
    
    lcd.setRGB(colorR, colorG, colorB);
    
    // Print a message to the LCD.
    lcd.print("hola!");

    Serial.begin(115200);  
    delay(2000);  
    Serial.println("Serial Terminal");  
    Serial.flush();  

pinMode(speakerPin, OUTPUT);
}  
  

    
void loop() {
    delay (1000);  

    // put your main code here, to run repeatedly:  

    int cbRead = 0; 
    int i;

    for (i = 0; i < catCmdLength; i++) {
        // Copy the catCmd into the string buffer abBuf.  
        abBuf[i] = catCmd[i];  
    }  
    abBuf[i] = 0;  // NULL terminate the string buffer abBuf.

    // DEBUG CODE.
    //Serial.print("cb Read: ");  
    //Serial.println(cbRead, DEC);  
    //Serial.print("Cmd: ");  
    //Serial.println(abBuf);  
    /////////////

    // Issue the catCmd down to the Linux side.
    // popen:    http://linux.die.net/man/3/popen      
    FILE *fpipe;  
    if ( !(fpipe = (FILE*)popen(abBuf,"r")) ) {  
        Serial.println("Problems with pipe");  
        return;  
    }  

    // fgets asks Linux "what is happing with the pipe fpipe?"
    // And writes it to buffer abBuf.
    // fgets:    http://www.cplusplus.com/reference/cstdio/fgets/    
    while (fgets( abBuf, sizeof(abBuf), fpipe)) {  
        Serial.print(abBuf);  // Send a copy of the character up the
        // serial port for debug purposes.
    }  
    pclose(fpipe);  // Close the pipe to /tmp/sharedFile, we are done with it.
    Serial.println ("");  // Send a newline up the serial port.

    // Set the cursor to column 0, line 1.
    // (note: line 1 is the second row, since counting begins with 0):
    lcd.setCursor(0, 1);

    // Print the string abBuf, which ought to contain the contents of
    // /tmp/sharedFile
    lcd.print(abBuf);



 

  
// heres where we beep we added this 
 if (abBuf[0] != mylastletter){
 digitalWrite(speakerPin, HIGH);
        delay(mydelay);
        digitalWrite(speakerPin, LOW); 
}        
 // heres where we change color  we added this       
if (abBuf[0] == 'P')
    {
    lcd.setRGB(0, colorG, 0);
    }
if (abBuf[0] == 'K')
    {

    lcd.setRGB(255,52,179);
    
    }
 mylastletter=abBuf[0];   
}
// KHIO is hillsboro
// PANC is anchorage

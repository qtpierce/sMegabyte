/*
   Copyright (c) 2015 Intel Corporation.  All rights reserved.

   This library is free software; you can redistribute it and/or
   modify it under the terms of the GNU Lesser General Public
   License as published by the Free Software Foundation; either
   version 2.1 of the License, or (at your option) any later version.

   This library is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
   Lesser General Public License for more details.

   You should have received a copy of the GNU Lesser General Public
   License along with this library; if not, write to the Free Software
   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/


// The LED is a bare red LED inserted into pins 4 and 5.  The anode (positive, long pin) is supposed to be inserted in pin 5.  The cathode (negative, flat edge) is supposed to be inserted in pin 4.
// The phone app is supposed to be the Nordic Semiconductor nRF Toolbox setup as a 9-button remote issuing UART chars.

#include <CurieBle.h>


int pwm_a = 3;  //PWM control for motor outputs 1 and 2 is on digital pin 3
int pwm_b = 9;  //PWM control for motor outputs 3 and 4 is on digital pin 9
int dir_a = 12;  //direction control for motor outputs 1 and 2 is on digital pin 12
int dir_b = 13;  //direction control for motor outputs 3 and 4 is on digital pin 13
int val = 0;     //value for fade

int LEDanode = 5;  // Meant for a bare LED with no series resistance.
int LEDcathode = 4;  // Meant for a bare LED with no series resistance.


BLEPeripheral blePeripheral;  // BLE Peripheral Device (the board you're programming)
// Nordic's UART Characteristics:    https://devzone.nordicsemi.com/documentation/nrf51/6.0.0/s110/html/a00066.html
BLEService UARTService ("6E400001-B5A3-F393-E0A9-E50E24DCCA9E"); // Nordic UART Service

// BLE UART Switch Characteristic - custom 128-bit UUID, read and writable by central
BLEUnsignedCharCharacteristic RXCharacteristic ("6E400002-B5A3-F393-E0A9-E50E24DCCA9E", BLEWrite);  // The Nordic Semiconductor UART App sends chars over this RXCharacteristic.
BLEUnsignedCharCharacteristic TXCharacteristic ("6E400003-B5A3-F393-E0A9-E50E24DCCA9E", BLERead);


// const int ledPin = 13; // pin to use for the LED  BUT!  We can't use the onboard LED because the Ardumoto shield
// uses pin 13 to indicate forward direction is being enabled.


void setup () {
  Serial.begin (9600);

  // set advertised local name and service UUID:
  blePeripheral.setLocalName ("UART");
  blePeripheral.setAdvertisedServiceUuid (UARTService.uuid());

  // add service and characteristic:
  blePeripheral.addAttribute (UARTService);
  blePeripheral.addAttribute (RXCharacteristic);
  blePeripheral.addAttribute (TXCharacteristic);
    
  // set the initial value for the characeristic:
  RXCharacteristic.setValue (0);

  // begin advertising BLE service:
  blePeripheral.begin ();

  Serial.println ("BLE UART Peripheral");

  myardumoto_setup ();
}



void loop () {
  // listen for BLE peripherals to connect:
  BLECentral central = blePeripheral.central ();

  // if a central is connected to peripheral:
  if (central) {
    Serial.print ("Connected to central: ");
    // print the central's MAC address:
    Serial.println (central.address ());

    // while the central is still connected to peripheral:
    while (central.connected ()) {
      // if the remote device wrote to the characteristic,
      if (RXCharacteristic.written()) {
        Serial.print ("Received: ");
        Serial.print (RXCharacteristic.value ());  // RX Characteristic
        Serial.print (", ");
        Serial.println (TXCharacteristic.value ());  // TX Characteristic
      
        ParseRXValue (RXCharacteristic.value ());
      }
      
      // https://github.com/sandeepmistry/arduino-BLEPeripheral/blob/master/BLETypedCharacteristic.h
      RXCharacteristic.setValue (0);
      
      delay (1000);
    }

    // when the central disconnects, print it out:
    Serial.print ("Disconnected from central: ");
    Serial.println(central.address ());
  }
}



void myardumoto_setup ()
{
  pinMode (pwm_a, OUTPUT);  //Set control pins to be outputs
  pinMode (pwm_b, OUTPUT);
  pinMode (dir_a, OUTPUT);
  pinMode (dir_b, OUTPUT);
  pinMode (11, INPUT);
  
  analogWrite (pwm_a, 0);  // set both motors to 0
  analogWrite (pwm_b, 0);

  pinMode (LEDanode, OUTPUT);
  pinMode (LEDcathode, OUTPUT);
  LEDOff ();
}




void ParseRXValue (int RXValue)
{
  if (RXValue == 49)
  {   // ASCII '1'
    LEDOn ();
  }
  if (RXValue == 48)
  {  // a 0 value
    LEDOff ();
  }
  if (RXValue == 119)
  {   //
    forward ();
  }
  if (RXValue == 115)
  {  // 
    backward ();
  }
  if (RXValue == 32)
  {  // 
    stopped ();
  }
}

void LEDOn ()
{
  digitalWrite (LEDanode, HIGH);
  digitalWrite (LEDcathode, LOW);
  Serial.println ("LED on");
}

void LEDOff ()
{
  digitalWrite (LEDanode, LOW);
  digitalWrite (LEDcathode, LOW);
  Serial.println ("LED off");
}


void forward () // forward
{ 
  digitalWrite (dir_a, HIGH);  //Reverse motor direction, 1 high, 2 low
  digitalWrite (dir_b, HIGH);  //Reverse motor direction, 3 low, 4 high  
  analogWrite (pwm_a, 100);    //set both motors to run at (100/255 = 39)% duty cycle
  analogWrite (pwm_b, 100);
  Serial.println ("forward");
}

void backward () // backward
{
  digitalWrite (dir_a, LOW);  //Set motor direction, 1 low, 2 high
  digitalWrite (dir_b, LOW);  //Set motor direction, 3 high, 4 low
  analogWrite (pwm_a, 100);   //set both motors to run at 100% duty cycle (fast)
  analogWrite (pwm_b, 100);
  Serial.println ("backward");
}

void stopped () // stop
{ 
  digitalWrite (dir_a, LOW); //Set motor direction, 1 low, 2 high
  digitalWrite (dir_b, LOW); //Set motor direction, 3 high, 4 low
  analogWrite (pwm_a, 0);    
  analogWrite (pwm_b, 0); 
  Serial.println ("STOP");
}

// space is 32
// ^ is 119
// > is 100
// v is 115
// < is 97
// 0 is 48
// 1 is 49




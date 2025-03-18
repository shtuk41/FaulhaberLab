# FaulhaberLab

This C# library was developed to communicate with the Faulhaber MC 5004 P motion controller via a UPS/RS232 interface. It allows a PC to control a Faulhaber brushless DC servo motor, which in our application drives a custom camera aperture ring. 

From the document: the object dictionary contains parameters, set-points and actual values of the drive.  The object dicionary is the link between the application (drive functions) and the communication services.  The master communicatates with the object dictionary via the interface (USB/RS232) and using the communication services that are based on CAN-open device system.

SDO (Service Data Objects) are used to read and write information to the object dictionary.  The way it works, the software builds telegrams to read or write data from/to an object dictionary, then sends them to the interface and receives responses.

For more information see https://www.faulhaber.com/fileadmin/Import/Media/EN_7000_05052.pdf

Faulahber.Core  - communication library.
FaulhaberPlayer - GUI application to operate the motor, test motion & homing and monitor status register.
FaulhaberMonitorService - monitors motor position.

![alt text](Readme/image.png)
Note that Faulhaber provides a library called MomanLib for communicating with their controllers using C++ and C#. https://faulhaber.com.cn/fileadmin/Import/Media/AN176_EN.pdf.


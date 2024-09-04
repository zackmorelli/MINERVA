# MINERVA

MINERVA is a small program for experimenting with the Health Layer Seven (HL7) communication protocol used in the healthcare industry.
The healthcare industry has a large problem with disparate software applications being unable to communicate with each other, or not communicate as well as they should be.
This was a proof-of-concept I made to investigate using HL7 to improve communication between the Radiation Oncology department's ARIA system and the hospital's EPIC EHR during my time working at Lahey Hospital.

The great thing about HL7 is it is an open-source, standarized communication protocol for healthcare that anyone can use.
MINERVA uses the NHapi library, which is an open source implementation of HL7 for .NET.

MINERVA specifically uses NHapi to send a simple "ADT01" (patient admission) message in HL7 version 2.5.1 format, which uses pipes as delimiters.

There is a lot of documentation online about how HL7 works and the different versions (including FHIR) and history behind it, as well as documentation for the NHapi library, so I won't go into a lot of detail about that here.

However, here is a brief implementation description. MINERVA uses a simple Minimal Lower Layer Protocol (MLLP) client-server setup to send messages in HL7 format over TCP/IP. MLLP is the standard protocol for transmitting HL7 messages, however there technically is nothing that prevents HL7 messages from being transmitted over other protocols.
MLLP typically uses port 57954. Healthcare software applications have MLLP servers listening for messages from other applications on the hospital organization's network. 
Code is needed on both sides to translate the HL7 messages into the databases of various different applications that come from various different vendors (which I beilieve is where the ball gets dropped).
 
Even though MINERVA is a simple example of sending a patient admission message, my goal was to prove that this can be done relatively easily using libraries like NHapi and a little effort.

Just want to note that Fast Healthcare Interoperability Resources (FHIR) is a more modern version of HL7 that uses HTTP and JSON instead of MLLP and the pipe-delimited format. 
Obviously this is better, but a lot of the older super important health tech infrastucture inside hospitals (like EPIC, ARIA, PACS) uses HL7 v2.x.

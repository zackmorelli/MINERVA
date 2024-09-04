using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapiTools.Base.Net;

//This is currently an early stage testbed solely for proof-of-concept

namespace MINERVA
{
    class ADT_Test
    {
        static void Main(string[] args)
        {
            try
            {
                LogToDebugConsole("Program started. Automatically generating and sending ADT01 message for fake hard-coded patient \"Christopher Moltisanti\"");
                Adt01MessageBuilder adtMessageBuilder = new Adt01MessageBuilder();
                var adtMessage = adtMessageBuilder.Build();
                var pipeParser = new PipeParser();


                LogToDebugConsole("Message constructed succesfully. now attempting to send to server via MLLP.");
                Thread.Sleep(5000);
                //WriteMessageFile(pipeParser, adtMessage, "C:\\Users\\ztm00\\Desktop\\HL7TestOuputs", "Christopher_Moltisanti_ADT01_Pipe_Delimited_Output_Test.txt");

                var connection = new SimpleMLLPClient("localhost", 57594, Encoding.UTF8);
                LogToDebugConsole("Sending message:" + "\n" + pipeParser.Encode(adtMessage));
                var response = connection.SendHL7Message(adtMessage);
                var responseString = pipeParser.Encode(response);
                LogToDebugConsole("Received response:\n" + responseString);
                Thread.Sleep(4000);
            }
            catch (Exception e)
            {
                LogToDebugConsole($"An error occurred while creating the HL7 Message. \n\n {e.Message} \n\n {e.StackTrace} \n\n {e.InnerException}");
            }
        }

        private static void WriteMessageFile(ParserBase parser, IMessage hl7Message, string outputDirectory, string outputFileName)
        {
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            var fileName = Path.Combine(outputDirectory, outputFileName);

            LogToDebugConsole("Writing data to file...");

            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllText(fileName, parser.Encode(hl7Message));
            LogToDebugConsole($"Wrote data to file {fileName} successfully...");
        }

        private static void LogToDebugConsole(string informationToLog)
        {
            Debug.WriteLine(informationToLog);
        }




    }


}

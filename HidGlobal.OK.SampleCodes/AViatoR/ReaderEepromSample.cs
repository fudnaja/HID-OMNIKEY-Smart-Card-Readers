/*****************************************************************************************
    (c) 2017-2018 HID Global Corporation/ASSA ABLOY AB.  All rights reserved.

      Redistribution and use in source and binary forms, with or without modification,
      are permitted provided that the following conditions are met:
         - Redistributions of source code must retain the above copyright notice,
           this list of conditions and the following disclaimer.
         - Redistributions in binary form must reproduce the above copyright notice,
           this list of conditions and the following disclaimer in the documentation
           and/or other materials provided with the distribution.
           THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
           AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
           THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
           ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
           FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
           (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
           LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
           ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
           (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
           THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*****************************************************************************************/
using System;
using System.Linq;
using System.Text;
using HidGlobal.OK.Readers;
using HidGlobal.OK.Readers.Components;
using HidGlobal.OK.SampleCodes.Utilities;

namespace HidGlobal.OK.SampleCodes.AViatoR
{
    public class ReaderEepromSample
    {
        private static void PrintCommand(string name, string input, string output)
        {
            ConsoleWriter.Instance.PrintSplitter();
            ConsoleWriter.Instance.PrintCommand(name, input, output);
        }
        public class WriteEeprom
        {
            private void WriteEepromCommand(ISmartCardReader smartCardReader, string comment, ushort offset, string dataToWrite)
            {
                var eepromCommands = new Readers.AViatoR.Components.ReaderEeprom();

                string input = eepromCommands.WriteCommand(offset, dataToWrite);
                string output = ReaderHelper.SendCommand(smartCardReader, input);

                PrintCommand(comment, input, output);
            }

            public string ToHexString(string str)
            {
                var sb = new StringBuilder();

                var bytes = Encoding.Unicode.GetBytes(str);
                foreach (var t in bytes)
                {
                    sb.Append(t.ToString("X2"));
                }

                return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
            }

            void ExecuteExample(ISmartCardReader smartCardReader)
            {
                WriteEepromCommand(smartCardReader, "Test", 1, ToHexString("1102001219081|ศรีชัย ชัยศรี|5กม6763|iamsrichai@gmail.com~"));
                //WriteEepromCommand(smartCardReader, "Test2", 1, stringToHex("200"));
                //WriteEepromCommand(smartCardReader, "Test3", 1, stringToHex("121"));
                //WriteEepromCommand(smartCardReader, "Test4", 1, stringToHex("908"));
                //WriteEepromCommand(smartCardReader, "Test5", 1, stringToHex("1"));

                //WriteEepromCommand(smartCardReader, "Write 16 bytes of FF with offset address 0x0001", 0x0001, "0012");
                //WriteEepromCommand(smartCardReader, "Write 128 bytes of FF with offset address 0x0001", 0x0001,"1908");

                //WriteEepromCommand(smartCardReader, "Write 128 bytes of FF with offset address 0x0001", 0x0001,
                //    "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" + "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                //    "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" + "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                //    "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" + "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" +
                //    "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF" + "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
            }
            public void Run(string readerName)
            {
                using (var reader = new SmartCardReader(readerName))
                {
                    try
                    {
                        ConsoleWriter.Instance.PrintSplitter();
                        ConsoleWriter.Instance.PrintTask($"Connecting to {reader.PcscReaderName}");

                        ReaderHelper.ConnectToReader(reader);

                        ConsoleWriter.Instance.PrintMessage($"Connected\nConnection Mode: {reader.ConnectionMode}");

                        ExecuteExample(reader);

                        ConsoleWriter.Instance.PrintSplitter();
                    }
                    catch (Exception e)
                    {
                        ConsoleWriter.Instance.PrintError(e.Message);
                    }
                    finally
                    {
                        if (reader.IsConnected)
                        {
                            reader.Disconnect(CardDisposition.Unpower);
                            ConsoleWriter.Instance.PrintMessage("Reader connection closed");
                        }
                        ConsoleWriter.Instance.PrintSplitter();
                    }
                }
            }
        }

        public class ReadEeprom
        {
            public string FromHexString(string hexString)
            {
                var bytes = new byte[hexString.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
            }

            public string ToHexString(string str)
            {
                var sb = new StringBuilder();

                var bytes = Encoding.Unicode.GetBytes(str);
                foreach (var t in bytes)
                {
                    sb.Append(t.ToString("X2"));
                }

                return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
            }
            private void ReadEepromCommand(ISmartCardReader smartCardReader, string comment, ushort offset, byte dataLength)
            {
                var eepromCommands = new Readers.AViatoR.Components.ReaderEeprom();

                string input = eepromCommands.ReadCommand(offset, dataLength);
                string output = ReaderHelper.SendCommand(smartCardReader, input);
                int index = output.IndexOf(ToHexString("~"));
                string data = output.Substring(10, index- 10);
                string save = FromHexString(data);

                PrintCommand(comment, input, save);
            }
            private void ExecuteExample(ISmartCardReader smartCardReader)
            {
                ReadEepromCommand(smartCardReader, "Read 1 byte with offset address 0x0000", 0x0000, 0x80);
                //ReadEepromCommand(smartCardReader, "Read 16 bytes with offset address 0x00F0", 0x00F0, 0x10);
                //ReadEepromCommand(smartCardReader, "Read 128 bytes with offset address 0x0100", 0x0100, 0x80);
            }
            public void Run(string readerName)
            {
                using (var reader = new SmartCardReader(readerName))
                {
                    try
                    {
                        ConsoleWriter.Instance.PrintSplitter();
                        ConsoleWriter.Instance.PrintTask($"Connecting to {reader.PcscReaderName}");

                        ReaderHelper.ConnectToReader(reader);

                        ConsoleWriter.Instance.PrintMessage($"Connected\nConnection Mode: {reader.ConnectionMode}");

                        ExecuteExample(reader);

                        ConsoleWriter.Instance.PrintSplitter();
                    }
                    catch (Exception e)
                    {
                        ConsoleWriter.Instance.PrintError(e.Message);
                    }
                    finally
                    {
                        if (reader.IsConnected)
                        {
                            reader.Disconnect(CardDisposition.Unpower);
                            ConsoleWriter.Instance.PrintMessage("Reader connection closed");
                        }
                        ConsoleWriter.Instance.PrintSplitter();
                    }
                }
            }
        }
    }
}

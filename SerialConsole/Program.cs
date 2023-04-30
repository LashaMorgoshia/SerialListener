using System;
using System.IO.Ports;
using System.Threading;

namespace SerialConsole
{
    internal class Program
    {
        static SerialPort serialPort;
        static int Id = 6;

        /// <summary>
        /// სერიალის ინიცირება
        /// </summary>
        static void Config()
        {
            Console.Write("Please enter a port number to listen:");
            Id = int.Parse(Console.ReadLine());
            serialPort = new SerialPort($"COM{Id}", 9600); // Replace 9600 with the baud rate of your device
            EnsurePortIsOpen();
        }

        /// <summary>
        /// პორტის გახსნა
        /// </summary>
        static void EnsurePortIsOpen()
        {
            while (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.Open();
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine($"Waiting for COM{Id} to open ..");
                    Thread.Sleep(5000);
                }
            }

            Console.WriteLine($"COM{Id} is Opened, listening to bytes ..");
        }

        static void Read()
        {
            Config();

            try
            {
                string data = "";
                while (true && serialPort.IsOpen)
                {
                    if (serialPort.BytesToRead > 0)
                    {
                        byte[] buffer = new byte[serialPort.BytesToRead];
                        serialPort.Read(buffer, 0, buffer.Length);
                        data += System.Text.Encoding.ASCII.GetString(buffer);
                        if (data.IndexOf("|") > 0)
                        {
                            var lines = data.Split('|');
                            foreach (var line in lines)
                            {
                                if (!string.IsNullOrEmpty(line))
                                    Console.WriteLine(line);
                            }
                            data = "";
                        }
                    }
                }
            }
            catch { }

            Read();
        }

        static void Main(string[] args)
        {
            Read();

            Console.ReadLine();
        }
    }
}

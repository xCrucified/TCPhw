using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

class Program
{
    static int port = 8080;   
    static string address = "127.0.0.1";
    static string data;

    static void Main(string[] args)
    {
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
        TcpClient client = new TcpClient();

        client.Connect(ipPoint);

        try
        { 
                Console.Write("Enter a 'VIN' code to get car data >> ");
                data = Console.ReadLine();
                NetworkStream ns = client.GetStream();
                StreamWriter streamWriter= new StreamWriter(ns);
                streamWriter.WriteLine(data);
                streamWriter.Flush();

                StreamReader sr = new StreamReader(ns);
                string response = sr.ReadLine();

                Console.WriteLine("Server response: " + response);   
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            client.Close();
        }
    }
}

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

class Program
{
    // адрес та порт сервера, до якого відбувається підключення
    static int port = 8080;              // порт сервера
    static string address = "127.0.0.1"; // адреса сервера
    static string data;

    static void Main(string[] args)
    {
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

        TcpClient client = new TcpClient();

        // підключення до віддаленого хоста
        client.Connect(ipPoint);

        try
        { 
                // введення даних для відправки
                Console.Write("Enter a VIN code to get car data >> ");
                data = Console.ReadLine();
                // отримуємо потік для обміну повідомленнями
                NetworkStream ns = client.GetStream();
                
                // серіалізація об'єкта та відправка його
                //JsonSerializer.Serialize(ns, data);
                StreamWriter streamWriter= new StreamWriter(ns);

                streamWriter.WriteLine(data);
                streamWriter.Flush();

                // отримуємо відповідь
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
            // закриваємо підключення
            client.Close();
        }
    }
}

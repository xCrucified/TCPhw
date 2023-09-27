using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpClientSample
{
    public class Car
    {
        public string Name;
        public string VINcode;
    }

    class Program
    {
        static List<Car> list = new List<Car>
        {
            new Car() {Name = "Audi", VINcode = "AI2512BT" },
            new Car() {Name = "BMW", VINcode = "NR2345OW" },
            new Car() {Name = "Dodge", VINcode = "BK5167RS" }
        };

        static int port = 8080;
        static string address = "127.0.0.1"; // localhost
        static void Main(string[] args)
        {
            // створення кінцевої точки для запуску сервера
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            // створюємо сокет на вказаній кінцевій точці
            TcpListener listener = new TcpListener(ipPoint);

            // запуск приймання підключень на сервер
            listener.Start(10);

            while (true)
            {
                Console.WriteLine("Server started! Waiting for connection...");
                
                TcpClient client = listener.AcceptTcpClient(); // wait until connection

                try
                {
                    while (client.Connected)
                    {
                        NetworkStream ns = client.GetStream();

                        // отримуємо переданий об'єкт та десеріалізуємо його
                        //BinaryFormatter formatter = new BinaryFormatter();
                        //var request = JsonSerializer.Deserialize<string>(ns);

                        StreamReader streamReader= new StreamReader(ns);
                        
                        var request = streamReader.ReadLine();

                        Console.WriteLine($"Request data: {request} from {client.Client.RemoteEndPoint}");

                        // відправляємо відповідь
                        
                        string response = $"Result = {list.Where(x => x.VINcode == request).Select(x => x.Name).First()}";
                        Console.WriteLine(response);

                        StreamWriter sw = new StreamWriter(ns); // розмір буфера за замовчуванням: 1KB
                        sw.WriteLine(response);
                        sw.Flush(); // clear buffer
                    }

                    // закриваємо сокет
                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            listener.Stop();
        }
    }
}
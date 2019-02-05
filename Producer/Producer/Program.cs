using System;
using System.Linq;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("direct_logs", "direct");
                    var message = "Welcome to the trip, man!";
                    var body = Encoding.UTF8.GetBytes(message);

                    var secondMessage = "This is another message";
                    var secondBody = Encoding.UTF8.GetBytes(secondMessage);

                    channel.BasicPublish("direct_logs", "temp", null, body);
                    Console.WriteLine($"--> Sent: {message}");

                    channel.BasicPublish("direct_logs", "second",null, secondBody);
                    Console.WriteLine($"--> Sent: {secondMessage}");
                }
                Console.WriteLine("# Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}

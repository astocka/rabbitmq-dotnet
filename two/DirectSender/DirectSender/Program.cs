using System;
using System.Text;
using RabbitMQ.Client;

namespace DirectSender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Direct Message Sender App --\r\n");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_exchange", type: "direct");
                channel.QueueDeclare(queue: "messages", durable: false, exclusive: false, autoDelete: false,
                    arguments: null);
                var routingKey = "direct_message";

                Console.Write(">> ");
                var message = Console.ReadLine();
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "direct_exchange", routingKey: routingKey, basicProperties: null,
                    body: body);

                Console.WriteLine($"<< [{routingKey}] Sent: {message}");

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}

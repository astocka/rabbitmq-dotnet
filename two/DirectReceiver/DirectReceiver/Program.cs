using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DirectReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Direct Message Receiver App --\r\n");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_exchange", type: "direct");
                channel.QueueBind(queue: "messages", exchange: "direct_exchange", routingKey: "direct_message");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine($">> [{routingKey}] Receive: {message}");
                };

                channel.BasicConsume(queue: "messages", autoAck: true, consumer: consumer);

                Console.WriteLine("Press [enter] to exit.\r\n");
                Console.ReadLine();
            }
        }
    }
}

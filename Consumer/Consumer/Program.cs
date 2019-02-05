using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("direct_logs", "direct");

                var queueName = channel.QueueDeclare().QueueName;

                if (args.Length < 1)
                {
                    Console.Error.WriteLine($"Usage: {Environment.GetCommandLineArgs()[0]} [info] [warning] [error]");
                    Console.WriteLine("# Press [enter] to exit.");
                    Console.ReadLine();
                    Environment.ExitCode = 1;
                    return;
                }

                foreach (var severity in args)
                {
                    channel.QueueBind(queueName, "direct_logs", severity);
                }

                Console.WriteLine("# Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.Write("--> Please wait. The message is loading");
                    for (int i = 0; i < 5; i++)
                    {
                        Thread.Sleep(2000);
                        Console.Write(".");
                    }

                    Console.WriteLine();
                    Console.WriteLine("# Done.");
                    Console.WriteLine();
                    Console.WriteLine($"--> Received: {routingKey} : {message}");
                };

                channel.BasicConsume(queueName, true, consumer);

                Console.WriteLine();
                Console.WriteLine("# Press [enter] to exit.");
                Console.ReadLine();
            };
        }
    }
}

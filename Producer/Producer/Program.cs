﻿using System;
using System.Text;
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
                    channel.QueueDeclare("messages", false, false, false, null);

                    string message = "It works!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", "messages", null, body);
                    Console.WriteLine($" [x] Sent: {message}");
                }

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}

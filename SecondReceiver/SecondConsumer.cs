using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SecondReceiver
{
    class SecondConsumer
    {
        static void Main(string[] args)
        {
            try
            {
                var factory = new ConnectionFactory { HostName = "10.10.15.151", UserName = "guest" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "chat", type: "fanout");

                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                                      exchange: "chat",
                                      routingKey: "");


                    /*channel.QueueDeclare(queue: "hello",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);*/

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                    };
                    /*channel.BasicConsume(queue: "hello",
                                         noAck: true,
                                         consumer: consumer);*/
                    channel.BasicConsume(queue: queueName,
                                         noAck: true,
                                         consumer: consumer);

                    Console.WriteLine("Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}

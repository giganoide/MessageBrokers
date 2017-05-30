using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMqClient
{
    class Publisher
    {
        static void Main(string[] args)
        {
            //var factory = new ConnectionFactory {HostName = "localhost"};
            try
            {
                // *** rabbitmq on the nuk2
                var factory = new ConnectionFactory() {HostName = "10.10.15.151", UserName = "sender", Password = "s3nd3r"};

                // *** rabbitmq on the cloud
                //const string url = "amqp://pcauiutc:Bl742kUBNQhctDpn3nL5xirLIL8htuwd@puma.rmq.cloudamqp.com/pcauiutc";
                //var factory = new ConnectionFactory {Uri = url.Replace("amqp://", "amqps://")};

                var count = 1;
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    /*channel.QueueDeclare(queue: "hello",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);*/
                    //channel.ExchangeDeclare("chat", "fanout", durable:true);
                    channel.ExchangeDeclare("chat", ExchangeType.Direct, true);
                    var queueName = "TestQueue"; //channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                                      exchange: "chat",
                                      routingKey: "");

                    Console.WriteLine("Press [enter] to exit or a key to send a message");
                    while (Console.ReadKey().Key != ConsoleKey.Enter)
                    {
                        var message = "Hello World " + count + "!";
                        count++;
                        var body = Encoding.UTF8.GetBytes(message);

                        /*channel.BasicPublish(exchange: "",
                                             routingKey: "hello",
                                             basicProperties: null,
                                             body: body);*/
                        var props = channel.CreateBasicProperties();
                        props.Persistent = true;
                        channel.BasicPublish("chat", "", props, body);
                        Console.WriteLine("[x] Sent {0}", message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

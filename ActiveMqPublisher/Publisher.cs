using System;
using Apache.NMS;
using Apache.NMS.Util;

namespace ActiveMqPublisher
{
    class Publisher
    {
        static void Main(string[] args)
        {
            try
            {
                // Example connection strings:
                //    activemq:tcp://activemqhost:61616
                //    stomp:tcp://activemqhost:61613
                //    ems:tcp://tibcohost:7222
                //    msmq://localhost

                var connecturi = new Uri("activemq:tcp://10.10.15.151:61616");

                Console.WriteLine("About to connect to " + connecturi);

                // NOTE: ensure the nmsprovider-activemq.config file exists in the executable folder.

                //IConnectionFactory factory = new NMSConnectionFactory(connecturi);
                IConnectionFactory factory = new Apache.NMS.ActiveMQ.ConnectionFactory(connecturi);

                using (var connection = factory.CreateConnection())
                using (var session = connection.CreateSession())
                {
                    var destination = SessionUtil.GetDestination(session, "queue://FOO.BAR");

                    Console.WriteLine("Using destination: " + destination);

                    var count = 1;
                    // Create a consumer and producer

                    using (var producer = session.CreateProducer(destination))
                    {
                        // Start the connection so that messages will be processed.
                        connection.Start();
                        producer.DeliveryMode = MsgDeliveryMode.Persistent;
                        producer.RequestTimeout = TimeSpan.FromSeconds(10); ;

                        Console.WriteLine("Press [enter] to exit or a key to send a message");
                        while (Console.ReadKey().Key != ConsoleKey.Enter)
                        {
                            // Send a message
                            var request = session.CreateTextMessage("Hello World " + count + "!");
                            count++;
                            //request.NMSCorrelationID = "abc";
                            //request.Properties["NMSXGroupID"] = "cheese";
                            //request.Properties["myHeader"] = "Cheddar";

                            producer.Send(request);
                            Console.WriteLine("[x] Sent {0}", request.Text);
                        }
                    }
                }
            }
            catch
                    (Exception e) { Console.WriteLine(e.Message); }
            Console.ReadLine();
        }
    }
}

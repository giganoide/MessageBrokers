using System;
using System.Threading;
using Apache.NMS;
using Apache.NMS.Util;

namespace ActiveMqConsumer
{
    class Consumer
    {
        protected static AutoResetEvent semaphore = new AutoResetEvent(false);
        protected static ITextMessage message = null;
        protected static TimeSpan receiveTimeout = TimeSpan.FromSeconds(10);

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
                var factory = new Apache.NMS.ActiveMQ.ConnectionFactory(connecturi);

                using (var connection = factory.CreateConnection())
                using (var session = connection.CreateSession())
                {
                    var destination = SessionUtil.GetDestination(session, "queue://FOO.BAR");

                    Console.WriteLine("Using destination: " + destination);

                    // Create a consumer and producer
                    using (var consumer = session.CreateConsumer(destination))
                    {
                        // Start the connection so that messages will be processed.
                        connection.Start();
                        //consumer.Listener += new MessageListener(OnMessage);
                        consumer.Listener += (receivedMsg) =>
                        {
                            message = receivedMsg as ITextMessage;
                            Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
                            Console.WriteLine("Received message with text: " + message.Text);
                            semaphore.Set();
                        };

                        Console.WriteLine("Press [enter] to exit.");
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}

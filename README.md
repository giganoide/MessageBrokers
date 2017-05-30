# MessageBrokers

![alt text](https://www.rabbitmq.com/img/RabbitMQ-logo.svg "https://www.rabbitmq.com")
#### Publisher:
```C#
  public static void BasicPublish(this IModel model, string exchange, string routingKey, IBasicProperties basicProperties, byte[] body);
```
```C#
  channel.BasicPublish(("exchange", "routingKey", channel.CreateBasicProperties(), Encoding.UTF8.GetBytes("messaggio"));
```
#### Consumer:
```C#
  var consumer = new EventingBasicConsumer(channel);    
  consumer.Received += (model, ea) =>
  {
      var message = Encoding.UTF8.GetString(ea.Body);
      Console.WriteLine(" [x] Received: {0}", message);
  };
```

------------
![alt text](http://activemq.apache.org/index.data/activemq-5.x-box-reflection.png "http://activemq.apache.org")

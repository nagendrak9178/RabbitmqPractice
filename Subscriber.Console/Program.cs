using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Client Side RabbitMQ Message!");

/*
     The implementation for receiving messages is slightly more complex, as we need to constantly listen for messages that enter the 
     queue. However, to start off, we need to create a connection to our RabbitMQ server.
 */
var factory = new ConnectionFactory { HostName = "localhost" };
var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare("orders", exclusive:false);

/*
    We need to ensure to declare the queue, in case the Subscriber starts before the Producer.
    Without it, there would be no queue to subscribe to.
    Now we can implement the logic required to receive messages from the queue.First, we create a consumer. Messages will be pushed to us asynchronously, 
    therefore we define a callback method for the Received event, which will give us access to the message body through the eventArgs 
    parameter. Since we encoded our messages in the Producer code, we need to decode them to get our actual JSON message of the Order class. 
    To finish things off, we simply write this message to the console.
 */
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine("Message received from RB MQ Server: {0}", message);
};

/*
 We specify the queue we want to start consuming messages from. Next, we set autoAck to true, which will automatically handle
 acknowledgment of messages. Finally, we pass in our consumer object, which has our custom Received event handler logic which 
 will be executed when we receive a message.
 */
channel.BasicConsume(queue: "orders", autoAck:true, consumer: consumer);
Console.ReadKey();
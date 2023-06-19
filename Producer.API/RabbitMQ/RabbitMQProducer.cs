using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Producer.API.RabbitMQ
{
    public class RabbitMQProducer:IMessageProducer
    {
        public void SendMessage<T>(T message)
        {
            //create  a connection to the RabbitMQ server. i.e. Where RabbitMQ server is running.
            var factory = new ConnectionFactory { HostName = "localhost" };
            //Create a connection to the server which abstracts the socket connection.
            var connection = factory.CreateConnection();
            //Create a channel that allow us interact with RabbitMQ APIs. 
            using var channel = connection.CreateModel();
            /* Declare or create a queue on the server with the name of orders, if it doesn’t already exist. If it throws following error
              RabbitMQ.Client.Exceptions.OperationInterruptedException: The AMQP operation was interrupted: AMQP close-reason, initiated by Peer, code=405, text='RESOURCE_LOCKED - cannot obtain exclusive access to locked queue 'Orders_nag' in vhost '/'. It could be originally declared on another connection or the exclusive property value does not match that of the original declaration.
            Then use channel.QueueDeclare("Orders_nag", exclusive:false);
            */
            channel.QueueDeclare("orders", exclusive:false);

            /* Send a message to the newly declared Queue i.e. Orders as follows. Since RabbitMQ doesn’t allow 
             * plain strings or complex types to be sent in the message body, we must convert our Order class to JSON format,
             * and then encode it as a byte[]. With this done, we will publish our message to the orders queue, which we specify 
             * using the routingKey parameter.
             * */
            var jsonmessage = JsonConvert.SerializeObject(message);
            var jsonbody = Encoding.UTF8.GetBytes(jsonmessage);
            channel.BasicPublish(exchange: "",routingKey: "orders", body: jsonbody);
        }
    }
}

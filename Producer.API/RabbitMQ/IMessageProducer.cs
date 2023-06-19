namespace Producer.API.RabbitMQ
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T messge);
    }
}

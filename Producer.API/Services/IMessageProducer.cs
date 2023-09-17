namespace Producer.API.Services
{
    public interface IMessageProducer
    {
        public Task SendMesage<T>(T message);
    }
}

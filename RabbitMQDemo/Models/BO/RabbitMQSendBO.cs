namespace RabbitMQDemo.Models.BO
{
    public class RabbitMQSendBO
    {
        public required string Queue { get; set; }
        public required string QueueMsg { get; set; }
    }
}
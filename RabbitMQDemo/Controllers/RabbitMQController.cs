using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQDemo.Models.BO;

namespace RabbitMQDemo.Controllers;

[ApiController]
[Route("RabbitMQ")]
public class RabbitMQController : Controller
{
    
    [HttpPost("Send")]
    public string Send([FromBody] RabbitMQSendBO rabbitMQSendBO)
    {
        string sQueue = rabbitMQSendBO.Queue;
        string sQueueMsg = rabbitMQSendBO.QueueMsg;

        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: sQueue,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

        var message = sQueueMsg;
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: string.Empty,
                            routingKey: sQueue,
                            basicProperties: null,
                            body: body);

        return $"Queue:{sQueue} ,Sent {sQueueMsg}";
    }

    [HttpPost("Receive")]
    public string Receive([FromBody] RabbitMQReceiveBO rabbitMQReceiveBO)
    {
        string sQueue = rabbitMQReceiveBO.Queue;

        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: sQueue,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

        try{
            var data = channel.BasicGet(sQueue, true);
            var message = Encoding.UTF8.GetString(data.Body.ToArray());
            return message;
        }catch(Exception e){
            return e.Message;
        }

    }
}
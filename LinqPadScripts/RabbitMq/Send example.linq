<Query Kind="Program">
  <NuGetReference>RabbitMQ.Client</NuGetReference>
  <Namespace>RabbitMQ.Client</Namespace>
  <Namespace>RabbitMQ.Client.Events</Namespace>
</Query>

//Make sure the container is running
//Default port:15672
//Management portal: http://localhost:15672/
//Default UN and PW are guest
//https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html
void Main()
{
	//Send();
	Receive();
}

public void Send()
{
	using (var connection = GetConnection())
	{
		using (var channel = connection.CreateModel())
		{
			channel.QueueDeclare(queue: "hello",
								 durable: false,
								 exclusive: false,
								 autoDelete: false,
								 arguments: null);

			string message = "Hello World!";

			var body = Encoding.UTF8.GetBytes(message);

			channel.BasicPublish(exchange: "",
								 routingKey: "hello",
								 basicProperties: null,
								 body: body);

			Console.WriteLine(" [x] Sent {0}", message);
		}
	}
}

public void Receive()
{
	using (var connection = GetConnection())
	{
		using (var channel = connection.CreateModel())
		{
			channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            
			consumer.Received += (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				Console.WriteLine(" [x] Received {0}", message);
			};

			channel.BasicConsume(queue: "hello",
								 autoAck: true,
								 consumer: consumer);

			Console.WriteLine("Wait until you see the message is received.");
			Console.ReadLine();
		}
	}
}

private IConnection GetConnection()
{
	var factory = new ConnectionFactory() { HostName = "localhost" };

	var con = factory.CreateConnection();

	return con;
}
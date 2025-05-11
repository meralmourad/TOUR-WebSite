using System;

namespace Backend.IServices;

public interface IKafkaProducerService
{
    Task ProduceAsync(string topic, string message);
}


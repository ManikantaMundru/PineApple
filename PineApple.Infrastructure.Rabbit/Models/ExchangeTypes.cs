
namespace PineApple.Infrastructure.Rabbit.Models
{
    public enum ExchangeTypes
    {
        Fanout = 1,
        Topic = 2,
        Headers = 3,
        Direct = 4,
        Transient = 5
    }
}

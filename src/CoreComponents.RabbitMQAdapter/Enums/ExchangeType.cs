using System.ComponentModel;

namespace CoreComponents.RabbitMQAdapter.Enums
{
	public enum ExchangeType
	{
        [Description("fanout")]
        Fanout,
        [Description("heders")]
        Headers,
        [Description("topic")]
        Topic,
        [Description("direct")]
        Direct
    }
}


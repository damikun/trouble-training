
namespace APIServer.Domain.Core.Models.Events
{

    /// <summary>
    /// WebHookRemoved
    /// </summary>
    public class WebHookRemoved : DomainEvent
    {

        public WebHookRemoved()
        {

        }

        public long WebHookId { get; set; }

        // Add any custom props hire...

    }
}
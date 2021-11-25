
namespace APIServer.Domain.Core.Models.Events
{

    /// <summary>
    /// WebHookUpdated
    /// </summary>
    public class WebHookUpdated : DomainEvent
    {

        public WebHookUpdated()
        {

        }

        public long WebHookId { get; set; }

        // Add any custom props hire...

    }
}
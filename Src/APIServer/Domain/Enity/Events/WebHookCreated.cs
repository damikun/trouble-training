
namespace APIServer.Domain.Core.Models.Events {

    /// <summary>
    /// WebHookCreated
    /// </summary>
    public class WebHookCreated : DomainEvent {

        public WebHookCreated() {

        }

        public long WebHookId {get;set;}

        // Add any custom props hire...
    }
}
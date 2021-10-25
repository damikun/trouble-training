using System;

namespace APIServer.Domain.Core.Models.Events {
    /// <summary>
    /// Defines event object
    /// </summary>
    public class DomainEvent {
        public long ID { get; set; }

        #nullable enable
        public Guid? ActorID { get; set; }
        #nullable disable
        
        public DateTime TimeStamp { get; set; }

        public EventType EventType { get; set; }
    }
}
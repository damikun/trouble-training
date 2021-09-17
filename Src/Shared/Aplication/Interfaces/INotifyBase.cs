using MediatR;
using System;

namespace Shared.Aplication.Interfaces {

    /// <summary>Base Notification</summary>
    public interface INotificationBase : INotification {
        DateTime TimeStamp { get; set; }
        
        #nullable enable
        public string? ActivityId { get; set; }
        #nullable disable
    }
}

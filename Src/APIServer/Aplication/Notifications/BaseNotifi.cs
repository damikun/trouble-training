using System;
using Aplication.Interfaces;
using SharedCore.Aplication.Interfaces;

namespace APIServer.Aplication.Notifications {

    /// <summary>
    /// Base abstract class of all Notifi
    /// </summary>
    public abstract class BaseNotifi : INotificationBase {

        public string? ActivityId { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public string Type { get { return this.GetType().Name; } }
    }

}
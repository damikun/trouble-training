using System;
using SharedCore.Aplication.Interfaces;

namespace SharedCore.Aplication.Core.Commands {

    /// <summary>
    /// Base abstract class of all Notifi
    /// </summary>
    public abstract class BaseNotifi : INotificationBase {
        #nullable enable
        public string? ActivityId { get; set; }
        #nullable disable
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public string Type { get { return this.GetType().Name; } }
    }

}
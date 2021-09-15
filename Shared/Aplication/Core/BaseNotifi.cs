using System;
using Shared.Aplication.Interfaces;

namespace Shared.Aplication.Core.Commands {

    /// <summary>
    /// Base abstract class of all Notifi
    /// </summary>
    public abstract class BaseNotifi : INotificationBase {

        public string? ActivityId { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public string Type { get { return this.GetType().Name; } }
    }

}
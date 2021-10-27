using System;
using System.Diagnostics;
using SharedCore.Aplication.Interfaces;

namespace SharedCore.Aplication.Core.Commands {

    public abstract class CommandCore {

        #nullable enable
        public string? ActivityId { get; set; } = Activity.Current != null ? Activity.Current.Id : null;
        #nullable disable

        public DateTime TimeStamp { get; set; } = DateTime.Now;

        //Flags
        public CommandFlags Flags  { get; set; } = new CommandFlags();

        #nullable enable
        public long? monitor_time {get;set;} = null;
        #nullable disable
    }

    public class CommandFlags{
        public bool long_running = false;
    }

    public abstract class CommandBase<TResponse> : CommandCore, ICommandBase<TResponse> { }

    public abstract class CommandBase : CommandCore, ICommandBase { }


}
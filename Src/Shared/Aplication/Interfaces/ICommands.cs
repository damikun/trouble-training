using MediatR;
using SharedCore.Aplication.Core.Commands;

namespace SharedCore.Aplication.Interfaces {

    public interface ICommandBase<TResponse> : IRequest<TResponse>, ISharedCommandBase { }

    public interface ICommandBase : IRequest, ISharedCommandBase { }

    public interface ISharedCommandBase {

        #nullable enable
        string? ActivityId { get; set; }
        #nullable disable

        CommandFlags Flags { get; set; }
        
        #nullable enable
        long? monitor_time {get;set;} 
        #nullable disable
    }
   
}
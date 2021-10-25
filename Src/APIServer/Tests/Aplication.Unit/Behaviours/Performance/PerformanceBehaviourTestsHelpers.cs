using System.Threading.Tasks;
using SharedCore.Aplication.Payload;
using APIServer.Aplication.Shared.Errors;
using SharedCore.Aplication.Core.Commands;

namespace APIServer.Application.UnitTests.Behaviours
{
    public class PerformanceTestCommand : CommandBase<PerformanceTestPayload> { }

    public class PerformanceTestPayload : BasePayload<PerformanceTestPayload, ITestError> { }
    
    //---------------------------------

    public class TestPerformanceCommandHandler<T> where T :IBasePayload, new() {

        public TestPerformanceCommandHandler(){ }

        public async Task<T> HandleNormal() {

            await Task.CompletedTask;

            return new T();
        }

        public async Task<T> HandleDelayd() {

            await Task.Delay(100);

            return new T();
        }

    }
}
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace APIServer.Benchmark
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    [HtmlExporter]
    [SimpleJob(launchCount: 1, warmupCount: 10, targetCount: 1000)]
    public class PaginationBenchmark : BenchmarkBase
    {
        private static string QueryA { get; set; }
        private static string QueryB { get; set; }
        private static object Variables { get; set; }

        public PaginationBenchmark() : base(
            new CancellationToken())
        {
            QueryA = GetQueryA();
            QueryB = GetQueryB();
            Variables = GetQueryVariables();
        }

        //------------------------------------
        //------------------------------------

        private static string GetQueryA()
        {
            return @"query{
                webhooksA{
                    edges{
                        node{
                            id
                            isActive
                            webHookUrl
                        }
                    }
                }
            }";
        }

        private static string GetQueryB()
        {
            return @"query{
                webhooksB{
                    edges{
                        node{
                            id
                            isActive
                            webHookUrl
                        }
                    }
                }
            }";
        }

        private static object GetQueryVariables() => new { };

        //------------------------------------
        //------------------------------------

        [Benchmark(Baseline = true, Description = "Mediatr handler")]
        public async Task<bool> MediatR()
        {
            var response = await base.SendQueryAsync(QueryA);

            return AssertResult(response);
        }

        [Benchmark(Baseline = false, Description = "HC middleware")]
        public async Task<bool> Middleware()
        {
            var response = await base.SendQueryAsync(QueryB);

            return AssertResult(response);
        }

        private bool AssertResult(QueryResponse response)
        {
            return response?.raw_response?.StatusCode is not null ?
                (int)response.raw_response.StatusCode == 200 :
                false;
        }
    }
}
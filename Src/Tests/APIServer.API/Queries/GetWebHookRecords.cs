using Xunit;
using MediatR;
using System.Net.Http;
using FluentAssertions;
using Snapshooter.Xunit;
using APIServer.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace APIServer.API.IntegrationTests.WebHooks
{
    [Collection("Sequential")]
    public class QueryWebHookRecordsTests : BaseClassFixture
    {

        public QueryWebHookRecordsTests(XunitFixture fixture) : base(fixture)
        {

        }

        public static string GetTestQuery()
        {

            return @"query($hook_id: ID!){
                webHookRecords(hook_id:$hook_id){
                    edges{
                        node{
                            id
                        }
                    }
                }
            }";
        }

        [Fact]
        public async Task QueryWebHookRecords_Authorised()
        {
            var mutation = GetTestQuery();

            await RunAs(
              Common.GetDefaultUser(),
              Common.GetDefaultClinet());

            var variables = new
            {
                hook_id = "R1FMX1dlYkhvb2sKbDE="
            };

            var response = await HttpClient.ProcessQuery(mutation, variables);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);
        }

        [Fact]
        public async Task QueryWebHookRecords_Unauthorised()
        {
            var mutation = GetTestQuery();

            var variables = new
            {
                hook_id = "R1FMX1dlYkhvb2sKbDE="
            };

            var response = await HttpClient.ProcessQuery(mutation, variables);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);
        }

    }
}
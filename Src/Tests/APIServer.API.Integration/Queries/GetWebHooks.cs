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
    public class QueryWebHookTests : BaseClassFixture
    {
        private readonly IMediator _mediator;

        private readonly IDbContextFactory<ApiDbContext> _dbcontextfactory;

        public QueryWebHookTests(XunitFixture fixture):base(fixture){

        }

        public static string GetTestQuery(){

            return  @"query{
                webhooks{
                    edges{
                    node{
                        id
                        guid
                        isActive
                        webHookUrl
                        listeningEvents
                    }
                    }
                }
            }";
        }

        [Fact]
        public async Task QueryWebHook_Authorised()
        {
            var mutation = GetTestQuery();

            await RunAs(
              Common.GetDefaultUser(),
              Common.GetDefaultClinet());

            var response = await HttpClient.ProcessQuery(mutation);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);
        }

        [Fact]
        public async Task QueryWebHook_Unauthorised()
        {
            var mutation = GetTestQuery();

            var response = await HttpClient.ProcessQuery(mutation);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);
        }
       
    }
}
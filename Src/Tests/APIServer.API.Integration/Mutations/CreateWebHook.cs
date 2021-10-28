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
    public class CreateWebHookTests : BaseClassFixture
    {
        private readonly IMediator _mediator;

        private readonly IDbContextFactory<ApiDbContext> _dbcontextfactory;

        public CreateWebHookTests(XunitFixture fixture):base(fixture){

        }

        public static string GetTestMutation(){

            return  @"mutation($request: CreateWebHookInput){
                createWebHook(request:$request){
                    ... on CreateWebHookPayload{
                        hook{
                            id
                            isActive
                            webHookUrl
                            listeningEvents
                        }
                        errors{
                            ... on IBaseError{
                                message
                            }

                            ... on ValidationError{
                                message
                                fieldName
                            }
                        }
                        }
                    }
                }";
        }

        [Fact]
        public async Task CreateWebHook_Authorised()
        {

            var mutation = GetTestMutation();

            var variables =  new { 
                request = new{  webHookUrl="https://someurl/path", isActive=true }
            };

            await RunAs(
              Common.GetDefaultUser(),
              Common.GetDefaultClinet());

            var response = await HttpClient.ProcessQuery(mutation,variables);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);
        }

        [Fact]
        public async Task CreateWebHook_ValidationError()
        {

            var mutation = GetTestMutation();

            var variables =  new { 
                request = new{  webHookUrl="some_invalid_url", isActive=true }
            };

            await RunAs(
              Common.GetDefaultUser(),
              Common.GetDefaultClinet());

            var response = await HttpClient.ProcessQuery(mutation,variables);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);
        }

        [Fact]
        public async Task CreateWebHook_Unauthorised()
        {

            var mutation = GetTestMutation();
            
            var variables =  new { 
                request = new{  webHookUrl="https://someurl/path", isActive=true }
            };

            var response = await HttpClient.ProcessQuery(mutation,variables);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);
        }
       
    }
}
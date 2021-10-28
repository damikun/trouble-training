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
    public class RemoveWebHookTests : BaseClassFixture
    {
        private readonly IMediator _mediator;

        private readonly IDbContextFactory<ApiDbContext> _dbcontextfactory;

        public RemoveWebHookTests(XunitFixture fixture):base(fixture){

        }

        public static string GetTestMutation(){

            return  @"mutation($request: RemoveWebHookInput) {
                removeWebHook(request: $request) {
                    ... on RemoveWebHookPayload {
                    removed_id

                    errors {
                        ... on IBaseError {
                        message
                        }

                        ... on ValidationError {
                        message
                        fieldName
                        }
                    }
                    }
                }
            }";
        }

        [Fact]
        public async Task RemoveWebHook_Authorised()
        {

            var mutation = GetTestMutation();

            var variables =  new { 
                request = new { 
                    webHookId = 1
                }
            };

            await RunAs(
              Common.GetDefaultUser(),
              Common.GetDefaultClinet());

            var response = await HttpClient.ProcessQuery(mutation,variables);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);
        }


        [Fact]
        public async Task RemoveWebHook_ValidationError()
        {

            var mutation = GetTestMutation();

            var variables =  new { 
                request = new { 
                    webHookId = 999
                }
            };

            await RunAs(
              Common.GetDefaultUser(),
              Common.GetDefaultClinet());

            var response = await HttpClient.ProcessQuery(mutation,variables);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);
        }
       
        [Fact]
        public async Task RemoveWebHook_Unauthorised()
        {

            var mutation = GetTestMutation();
            
            var variables =  new { 
                request = new { 
                    webHookId = 2
                }
            };

            var response = await HttpClient.ProcessQuery(mutation,variables);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);
        }


    }
}
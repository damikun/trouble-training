using Xunit;
using System.Net.Http;
using FluentAssertions;
using Snapshooter.Xunit;
using System.Threading.Tasks;

namespace APIServer.API.IntegrationTests.WebHooks
{
    [Collection("Sequential")]
    public class UpdateWebHookTests : BaseClassFixture
    {

        public UpdateWebHookTests(XunitFixture fixture) : base(fixture)
        {

        }

        public static string GetTestMutation()
        {

            return @"mutation($request: UpdateWebHookInput){
                updateWebHook(request:$request){
                    ... on UpdateWebHookPayload{
                    hook{
                        id
                        isActive
                        webHookUrl
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
        public async Task UpdateWebHook_Authorised()
        {

            var mutation = GetTestMutation();

            var variables = new
            {
                request = new
                {
                    webHookId = 1,
                    isActive = false,
                    webHookUrl = "https://somenewurl"
                }
            };

            await RunAs(
              Common.GetDefaultUser(),
              Common.GetDefaultClinet());

            var response = await HttpClient.ProcessQuery(mutation, variables);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);
        }


        [Fact]
        public async Task UpdateWebHook_ValidationError()
        {

            var mutation = GetTestMutation();

            var variables = new
            {
                request = new
                {
                    webHookId = 9999,
                    isActive = false,
                    webHookUrl = "https://somenewurl"
                }
            };

            await RunAs(
              Common.GetDefaultUser(),
              Common.GetDefaultClinet());

            var response = await HttpClient.ProcessQuery(mutation, variables);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);
        }

        [Fact]
        public async Task UpdateWebHook_Unauthorised()
        {

            var mutation = GetTestMutation();

            var variables = new
            {
                request = new
                {
                    webHookId = 1,
                    isActive = false,
                    webHookUrl = "https://somenewurl"
                }
            };

            var response = await HttpClient.ProcessQuery(mutation, variables);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);
        }


    }
}
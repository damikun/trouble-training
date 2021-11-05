using Xunit;
using System.Net.Http;
using FluentAssertions;
using Snapshooter.Xunit;
using System.Threading.Tasks;

namespace APIServer.API.IntegrationTests.WebHooks
{   
    // This test basic functionality of graphql test server
     [Collection("Sequential")]
    public class UserQueryTests : BaseClassFixture
    {
        public UserQueryTests(XunitFixture fixture):base(fixture)
        {

        }

        [Fact]
        public async Task QueryServer_ValidQuery_Unauthorised()
        {   

            var query = @"query { 
                    me { 
                      id
                    }
                  }";

            var response = await HttpClient.ProcessQuery(query);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);

        }

        [Fact]
        public async Task QueryServer_ValidQuery_Authorised()
        {   

            var query = @"query { 
                    me { 
                      id
                    }
                  }";

            await RunAs(
              Common.GetDefaultUser(),
              Common.GetDefaultClinet());

            var response = await HttpClient.ProcessQuery(query);

            response.raw_response.StatusCode.Should().Be(200);

            Snapshot.Match(response.data_content);

        }

        [Fact]
        public async Task QueryServer_InValidQuery_NOK()
        {   
            var query = @"query { 
                    me { 
                      not_existing_field
                    }
                  }";

            var response = await HttpClient.ProcessQuery(query);

            response.raw_response.StatusCode.Should().Be(400);

            Snapshot.Match(response.data_content);
        }

    }
}
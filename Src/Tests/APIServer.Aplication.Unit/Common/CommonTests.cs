using Xunit;
using APIServer.Aplication.Shared;
using System.Text.RegularExpressions;

namespace APIServer.Application.UnitTests.Behaviours
{
    public class CommonTests
    {
        public CommonTests(){ }

        [Theory]
        [InlineData("")]
        [InlineData("someprefix/test")]
        [InlineData("some_random_text")]
        [InlineData("192.168.0.1")]
        [InlineData("192.168.0.1:9000/test")]
        public void UrlRegex_Fail(string url)
        {            
            var result = Regex.Match(url,Common.URI_REGEX);

            Assert.False(result.Success);
        }

        [Theory]
        [InlineData("www.test.com")]
        [InlineData("http://test/test")]
        [InlineData("http://test.com/test")]
        [InlineData("https://test.com/test")]
        [InlineData("https://test.com:9000/test")]
        [InlineData("https://192.168.0.1/test")]
        [InlineData("http://192.168.0.1/test")]
        [InlineData("http://192.168.0.1:9000/test")]
        public void UrlRegex_Ok(string url)
        {   
            var result = Regex.Match(url,Common.URI_REGEX);

            Assert.True(result.Success);
        }

    }
}
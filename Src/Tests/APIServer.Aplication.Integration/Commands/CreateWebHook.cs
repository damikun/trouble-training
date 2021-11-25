using Xunit;
using MediatR;
using System.Linq;
using FluentAssertions;
using APIServer.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIServer.Aplication.Shared.Errors;
using APIServer.Aplication.Commands.WebHooks;
using Microsoft.Extensions.DependencyInjection;

namespace APIServer.Application.IntegrationTests.WebHooks
{
    public class CreateWebHookTests : BaseClassFixture
    {
        private readonly IMediator _mediator;

        private readonly IDbContextFactory<ApiDbContext> _dbcontextfactory;

        public CreateWebHookTests(XunitFixture fixture) : base(fixture)
        {

            _mediator = this.TestServer.Services
                .GetService<IMediator>();

            _dbcontextfactory = this.TestServer.Services
                .GetService<IDbContextFactory<ApiDbContext>>();
        }

        [Fact]
        public async Task CreateWebHook_NoError()
        {

            TestCommon.SetAndGetAuthorisedTestContext(this.TestServer);

            await using ApiDbContext dbContext =
                _dbcontextfactory.CreateDbContext();

            var count_before = dbContext.WebHooks.AsNoTracking().Count();

            var response = await _mediator.Send(new CreateWebHook()
            {
                WebHookUrl = "https://test",
                IsActive = true
            });

            response.Should().NotBeNull();

            response.Should().BeOfType<CreateWebHookPayload>()
                .Subject.errors.Any().Should().BeFalse();

            Assert.True(count_before < dbContext.WebHooks.Count());

            response.hook.Should().NotBeNull();

            dbContext.WebHooks.AsNoTracking().Any(e => e.ID == response.hook.ID).Should().BeTrue();
        }

        [Fact]
        public async Task CreateWebHook_ValidationError()
        {

            TestCommon.SetAndGetAuthorisedTestContext(this.TestServer);

            await using ApiDbContext dbContext =
                _dbcontextfactory.CreateDbContext();

            var count_before = dbContext.WebHooks.AsNoTracking().Count();

            var response = await _mediator.Send(new CreateWebHook()
            {
                WebHookUrl = "This is not valid url",
                IsActive = true
            });

            response.Should().NotBeNull();

            response.Should().BeOfType<CreateWebHookPayload>()
                .Subject.errors.Any().Should().BeTrue();

            response.Should().BeOfType<CreateWebHookPayload>()
                .Subject.errors.First().Should().BeOfType<ValidationError>()
                    .Subject.FieldName.Should().Be("WebHookUrl");

            Assert.True(count_before == dbContext.WebHooks.Count());

            response.hook.Should().BeNull();
        }

        [Fact]
        public async Task CreateWebHook_UnauthorisedError()
        {
            TestCommon.SetAndGetUnAuthorisedTestConetxt(this.TestServer);

            await using ApiDbContext dbContext =
                _dbcontextfactory.CreateDbContext();

            var count_before = dbContext.WebHooks.AsNoTracking().Count();

            var response = await _mediator.Send(new CreateWebHook()
            {
                WebHookUrl = "https://test",
                IsActive = true
            });

            response.Should().NotBeNull();

            response.Should().BeOfType<CreateWebHookPayload>()
                .Subject.errors.Any().Should().BeTrue();

            response.Should().BeOfType<CreateWebHookPayload>()
                .Subject.errors.First().Should().BeOfType<UnAuthorised>();

            Assert.True(count_before == dbContext.WebHooks.Count());

            response.hook.Should().BeNull();

        }
    }
}
using Xunit;
using MediatR;
using System.Linq;
using FluentAssertions;
using APIServer.Persistence;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using APIServer.Aplication.Shared.Errors;
using APIServer.Domain.Core.Models.WebHooks;
using APIServer.Aplication.Commands.WebHooks;
using Microsoft.Extensions.DependencyInjection;

namespace APIServer.Application.IntegrationTests.WebHooks
{
    public class UpdateWebHookTests : BaseClassFixture
    {
        private readonly IMediator _mediator;

        private readonly IDbContextFactory<ApiDbContext> _dbcontextfactory;

        public UpdateWebHookTests(XunitFixture fixture) : base(fixture)
        {

            _mediator = this.TestServer.Services
                .GetService<IMediator>();

            _dbcontextfactory = this.TestServer.Services
                .GetService<IDbContextFactory<ApiDbContext>>();
        }

        [Fact]
        public async Task UpdateWebHook_NoError()
        {

            TestCommon.SetAndGetAuthorisedTestContext(this.TestServer);

            await using ApiDbContext dbContext =
                _dbcontextfactory.CreateDbContext();

            WebHook hook = new WebHook()
            {
                WebHookUrl = "https://testurl/random",
                IsActive = false
            };

            dbContext.WebHooks.Add(hook);

            await dbContext.SaveChangesAsync();

            dbContext.WebHooks.Any(e => e.ID == hook.ID).Should().BeTrue();

            bool updatet_state = true;

            var response = await _mediator.Send(new UpdateWebHook()
            {
                WebHookId = hook.ID,
                WebHookUrl = hook.WebHookUrl,
                IsActive = updatet_state,
                HookEvents = hook.HookEvents != null ?
                     hook.HookEvents.ToHashSet() : new HashSet<HookEventType>(),
                Secret = hook.Secret,
            });

            response.Should().NotBeNull();

            response.Should().BeOfType<UpdateWebHookPayload>()
                .Subject.errors.Any().Should().BeFalse();

            dbContext.WebHooks.Any(e => e.ID == hook.ID)
                .Should().BeTrue();

            dbContext.WebHooks.AsNoTracking().Where(e => e.ID == hook.ID)
                .First().IsActive.Should().Be(updatet_state);
        }

        [Fact]
        public async Task UpdateWebHook_ValidationError_UnknownId()
        {
            TestCommon.SetAndGetAuthorisedTestContext(this.TestServer);

            await using ApiDbContext dbContext =
                _dbcontextfactory.CreateDbContext();

            long some_unexisting_id = 999;

            dbContext.WebHooks.AsNoTracking()
                .Any(e => e.ID == some_unexisting_id).Should().BeFalse();

            var response = await _mediator.Send(new UpdateWebHook()
            {
                WebHookId = some_unexisting_id,
                WebHookUrl = "https://someurl",
                IsActive = false,
            });

            response.Should().NotBeNull();

            response.Should().BeOfType<UpdateWebHookPayload>()
                .Subject.errors.Any().Should().BeTrue();

            response.Should().BeOfType<UpdateWebHookPayload>()
                .Subject.errors.First().Should().BeOfType<ValidationError>()
                    .Subject.FieldName.Should().Be("WebHookId");
        }

        [Fact]
        public async Task UpdateWebHook_ValidationError_WrongUrlFormat()
        {
            TestCommon.SetAndGetAuthorisedTestContext(this.TestServer);

            await using ApiDbContext dbContext =
                _dbcontextfactory.CreateDbContext();

            WebHook hook = new WebHook()
            {
                WebHookUrl = "https://testurl/random2",
                IsActive = false
            };

            dbContext.WebHooks.Add(hook);

            await dbContext.SaveChangesAsync();

            dbContext.WebHooks.AsNoTracking()
                .Any(e => e.ID == hook.ID).Should().BeTrue();

            var response = await _mediator.Send(new UpdateWebHook()
            {
                WebHookId = hook.ID,
                WebHookUrl = "invalid url format",
                IsActive = false,
            });

            response.Should().NotBeNull();

            response.Should().BeOfType<UpdateWebHookPayload>()
                .Subject.errors.Any().Should().BeTrue();

            response.Should().BeOfType<UpdateWebHookPayload>()
                .Subject.errors.First().Should().BeOfType<ValidationError>()
                    .Subject.FieldName.Should().Be("WebHookUrl");

            dbContext.WebHooks.AsNoTracking()
                .Where(e => e.ID == hook.ID).First()
                    .WebHookUrl.Should().NotBe("invalid url format");
        }

        [Fact]
        public async Task UpdateWebHook_UnauthorisedError()
        {
            TestCommon.SetAndGetUnAuthorisedTestConetxt(this.TestServer);

            await using ApiDbContext dbContext =
                _dbcontextfactory.CreateDbContext();

            WebHook hook = new WebHook()
            {
                WebHookUrl = "https://testurl/random4",
                IsActive = true
            };

            dbContext.WebHooks.Add(hook);

            await dbContext.SaveChangesAsync();

            dbContext.WebHooks.Any(e => e.ID == hook.ID).Should().BeTrue();

            var response = await _mediator.Send(new UpdateWebHook()
            {
                WebHookId = hook.ID,
                WebHookUrl = hook.WebHookUrl,
                IsActive = false,
            });

            response.Should().NotBeNull();

            response.Should().BeOfType<UpdateWebHookPayload>()
                .Subject.errors.Any().Should().BeTrue();

            response.Should().BeOfType<UpdateWebHookPayload>()
                .Subject.errors.First().Should().BeOfType<UnAuthorised>();

            dbContext.WebHooks.Any(e => e.ID == hook.ID).Should().BeTrue();
        }
    }
}
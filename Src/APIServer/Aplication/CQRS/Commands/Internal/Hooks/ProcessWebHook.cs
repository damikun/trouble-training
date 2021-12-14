using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using APIServer.Domain.Core.Models.WebHooks;
using APIServer.Persistence;
using SharedCore.Aplication.Core.Commands;

namespace APIServer.Aplication.Commands.Internall.Hooks
{

    /// <summary>
    /// Command for processing WebHook event
    /// </summary>
    public class ProcessWebHook : CommandBase
    {
        public long HookId { get; set; }
        public dynamic Event { get; set; }
        public HookEventType EventType { get; set; }
    }

    /// <summary>
    /// Command handler for <c>ProcessWebHook</c>
    /// </summary>
    public class ProcessWebHookHandler : IRequestHandler<ProcessWebHook, Unit>
    {

        /// <summary>
        /// Injected <c>ApiDbContext</c>
        /// </summary>
        private readonly IDbContextFactory<ApiDbContext> _factory;

        /// <summary>
        /// Injected <c>IMediator</c>
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Injected <c>IHttpClientFactory</c>
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Main Constructor
        /// </summary>
        public ProcessWebHookHandler(
            IDbContextFactory<ApiDbContext> factory,
            IMediator mediator,
            IHttpClientFactory httpClient)
        {
            _factory = factory;
            _mediator = mediator;
            _clientFactory = httpClient;
        }

        /// <summary>
        /// Command handler for  <c>ProcessWebHook</c>
        /// </summary>
        public async Task<Unit> Handle(ProcessWebHook request, CancellationToken cancellationToken)
        {

            WebHookRecord record = new WebHookRecord()
            {
                WebHookID = request.HookId,
                Guid = Guid.NewGuid().ToString(),
                HookType = request.EventType,
                Timestamp = DateTime.Now
            };

            if (request == null || request.HookId <= 0)
            {
                record.Result = RecordResult.parameter_error;
            }

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            try
            {

                WebHook hook = null;

                try
                {
                    hook = await dbContext.WebHooks
                        .Where(e => e.ID == request.HookId)
                        .FirstOrDefaultAsync(cancellationToken);

                }
                catch (Exception ex)
                {
                    record.Result = RecordResult.dataQueryError;
                    record.Exception = ex.ToString();

                    return Unit.Value;
                }

                if (hook != null)
                {

                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        IncludeFields = true,
                    };

                    var serialised_request_body = new StringContent(
                          JsonSerializer.Serialize<dynamic>(request.Event, options),
                          Encoding.UTF8,
                          "application/json");


                    var httpClient = _clientFactory.CreateClient();

                    /* Set Headers */
                    httpClient.DefaultRequestHeaders.Add("X-Trouble-Delivery", record.Guid);

                    if (!string.IsNullOrWhiteSpace(hook.Secret))
                    {
                        httpClient.DefaultRequestHeaders.Add("X-Trouble-Secret", hook.Secret);
                    }

                    httpClient.DefaultRequestHeaders.Add("X-Trouble-Event", request.EventType.ToString().ToLowerInvariant());

                    record.RequestBody = await serialised_request_body.ReadAsStringAsync(cancellationToken);

                    var serialized_headers = new StringContent(
                                     JsonSerializer.Serialize(httpClient.DefaultRequestHeaders.ToList(), options),
                                     Encoding.UTF8,
                                     "application/json");

                    record.RequestHeaders = await serialized_headers.ReadAsStringAsync(cancellationToken);

                    if (!string.IsNullOrWhiteSpace(hook.WebHookUrl))
                    {
                        try
                        {
                            using var httpResponse = await httpClient.PostAsync(hook.WebHookUrl, serialised_request_body);

                            if (httpResponse != null)
                            {
                                record.StatusCode = (int)httpResponse.StatusCode;

                                if (httpResponse.Content != null)
                                {
                                    record.ResponseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                                }
                            }

                            record.Result = RecordResult.ok;
                        }
                        catch (Exception ex)
                        {
                            record.Result = RecordResult.http_error;
                            record.Exception = ex.ToString();

                            //throw ex;
                        }
                    }
                    else
                    {
                        record.Result = RecordResult.parameter_error;
                    }
                }
                else
                {
                    record.Result = RecordResult.parameter_error;
                }

            }
            finally
            {

                try
                {
                    dbContext.WebHooksHistory.Add(record);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                catch { }

            }

            return Unit.Value;
        }
    }
}

using System;
using APIServer.Domain.Core.Models.WebHooks;

namespace APIServer.Aplication.GraphQL.DTO
{

    public class GQL_WebHookRecord
    {
        public GQL_WebHookRecord()
        {

        }

        /// <summary>
        /// Hook record DB Id
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Linked Webhook GQL Id
        /// </summary>
        public long WebHookID { get; set; }


        /// <summary>
        /// Linked Webhook Id
        /// </summary>
        public long WebHookSystemID { get; set; }

        /// <summary>
        /// Record GUID
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Linked Webhook System Id
        /// </summary>
        public GQL_WebHook WebHook { get; set; }

        /// <summary>
        /// TriggerType
        /// </summary>
        public HookEventType TriggerType { get; set; }

        /// <summary>
        /// Hook result enum
        /// </summary>
        public RecordResult Result { get; set; }

        /// <summary>
        /// Response status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Response json
        /// </summary>
        public string ResponseBody { get; set; }

        /// <summary>
        /// Request json
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// Request Headers json
        /// </summary>
        public string RequestHeaders { get; set; }

        /// <summary>
        /// Exception
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// Hook Call Timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}



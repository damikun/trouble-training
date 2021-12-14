
using System;
using System.Collections.Generic;
using APIServer.Domain.Core.Models.WebHooks;

namespace APIServer.Aplication.GraphQL.DTO
{

    public class GQL_WebHook
    {
        public GQL_WebHook()
        {

        }
        // <summary>
        /// Hook DB Id
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Unique GUID
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Webhook endpoint
        /// </summary>
        public string WebHookUrl { get; set; }

        /// <summary>
        /// Content Type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Is active / NotActiv
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Hook Events context
        /// </summary>
        public HookEventType[] ListeningEvents { get; set; }

        /// <summary>
        /// Hook call records history
        /// </summary>
        public IEnumerable<GQL_WebHookRecord> Records { get; set; }

        /// <summary>
        /// Timestamp of last hook trigger
        /// </summary>
        /// <value></value>
        public DateTime? LastTrigger { get; set; }
    }
}



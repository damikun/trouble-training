using System;

namespace APIServer.Domain.Core.Models.WebHooks {
    public class WebHookHeader {

        /// <summary>
        /// Hook header ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Linked Webhook Id
        /// </summary>
        public long WebHookID { get; set; }

        /// <summary>
        /// Linked Webhook
        /// </summary>
        public WebHook WebHook { get; set; }

        /// <summary>
        /// Header Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Header content
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Header created time
        /// </summary>
        public DateTime CreatedTimestamp { get; set; }
    }
}
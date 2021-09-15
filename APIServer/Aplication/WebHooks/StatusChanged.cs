using System;
using APIServer.Domain.Core.Models.WebHooks;

namespace APIServer.Aplication.WebHooks {

    /// <summary>
    /// Hook_StatusChanged
    /// </summary>
    public class Hook_StatusChanged : WebHookNotifyBase<HookResourceAction, Hook_StatusChangedPayload> {

        public Hook_StatusChanged(HookResourceAction action) : base(action) {

        }

        public Hook_StatusChanged(
            HookResourceAction action,
            Hook_User_DTO actor,
            Hook_StatusChangedPayload payload) : base(action) {
            this.actor = actor;
            this.payload = payload;
        }
    }


    [Serializable]
    public class Hook_StatusChangedPayload {
        public long issue_id { get; set; }
        public string issue_name { get; set; }
        public string old_status { get; set; }
        public string new_status { get; set; }
    }

}
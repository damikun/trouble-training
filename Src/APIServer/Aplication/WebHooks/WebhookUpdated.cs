using System;
using APIServer.Domain.Core.Models.WebHooks;

namespace APIServer.Aplication.WebHooks {

    /// <summary>
    /// Hook_StatusChanged
    /// </summary>
    public class Hook_HookUpdated : WebHookNotifyBase<HookResourceAction, Hook_HookUpdatedPayload> {

        public Hook_HookUpdated(HookResourceAction action) : base(action) { }

        public Hook_HookUpdated(
            HookResourceAction action,
            Hook_User_DTO actor,
            Hook_HookUpdatedPayload payload) : base(action) {
            this.actor = actor;
            this.payload = payload;
        }
    }

    [Serializable]
    public class Hook_HookUpdatedPayload {

        // Add any custom payload hire
    }

}
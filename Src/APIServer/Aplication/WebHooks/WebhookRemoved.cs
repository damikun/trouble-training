using System;
using APIServer.Domain.Core.Models.WebHooks;

namespace APIServer.Aplication.WebHooks {

    /// <summary>
    /// Hook_StatusChanged
    /// </summary>
    public class Hook_HookRemoved : WebHookNotifyBase<HookResourceAction, Hook_HookRemovedPayload> {

        public Hook_HookRemoved(HookResourceAction action) : base(action) {

        }

        public Hook_HookRemoved(
            HookResourceAction action,
            Hook_User_DTO actor,
            Hook_HookRemovedPayload payload) : base(action) {
            this.actor = actor;
            this.payload = payload;
        }
    }


    [Serializable]
    public class Hook_HookRemovedPayload {
        // Add any custom payload hire
    }

}
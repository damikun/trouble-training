using System;

namespace APIServer.Domain.Core.Models.WebHooks {

    public interface IWebHookNotifyBase { }

    /// <summary>
    /// Base abstract class of WebHook
    /// </summary>
    [Serializable]
    public class WebHookNotifyBase<T, U> : IWebHookNotifyBase where T : System.Enum {

        public WebHookNotifyBase(T action) {
            this._action = action;
        }

        private T _action { get; set; }

        public string action {
            get { return _action.ToString(); }
        }
        public U payload { get; set; }

        #nullable enable
        public Hook_User_DTO? actor { get; set; } = new Hook_User_DTO();
        #nullable disable

        public DateTime timeStamp { get; set; } = DateTime.Now;
    }

    [Serializable]
    public class Hook_User_DTO {
        public string id { get; set; }
        public string name { get; set; }
    }

}
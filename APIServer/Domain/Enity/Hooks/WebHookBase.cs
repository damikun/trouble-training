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

        public Hook_User_DTO? actor { get; set; } = new Hook_User_DTO();

        public DateTime timeStamp { get; set; } = DateTime.Now;
    }

    [Serializable]
    public class Hook_User_DTO {
        public long id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

}
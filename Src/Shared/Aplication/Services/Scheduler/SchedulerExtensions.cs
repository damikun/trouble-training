using System.Diagnostics;
using SharedCore.Aplication.Interfaces;

namespace SharedCore.Aplication.Services
{
    public static class SchedulerExtensions
    {
        public static void SetActivityIdAsParrentId(this ISharedCommandBase command)
        {
            // This sets activity parrent / children relation..
            // Id can == null !!!
            if (command.ActivityId != null
                && Activity.Current?.ParentId == null)
            {
                Activity.Current.SetParentId(Activity.Current.Id);
            }
        }
    }
}
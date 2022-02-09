
namespace APIServer.Domain.Core.Models.WebHooks
{

    /// <summary>
    /// Represents source hook trigger group
    /// </summary>
    public enum HookEventType
    {
        // *********************************************************************
        // Take this as example you can implement any custom event source
        // *********************************************************************

        /// <summary>
        /// Hook - created, hook removed, hook updated, hook enabled/disabled
        /// </summary>
        hook,

        /// <summary>
        /// File - create, delete, revision
        /// </summary>
        file,

        /// <summary>
        /// Note - create, update, deleted
        /// </summary>
        note,

        /// <summary>
        /// Project - created, add/remove user, archived
        /// </summary>
        project,

        /// <summary>
        /// Milestone - created, deleted, closed, re-opened
        /// </summary>
        milestone

        //etc etc..
    }
}


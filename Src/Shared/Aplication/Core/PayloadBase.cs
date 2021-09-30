using System;
using System.Collections.Generic;
using HotChocolate;

namespace Aplication.Payload  {

    public interface IBasePayload {

        void AddError(object o);

    }

    public abstract class BasePayload<U, T> : IBasePayload where U : BasePayload<U, T>, new() {

        public BasePayload() {
            this.errors = new List<T>();
        }

        /// <summary>
        /// List of possible union errors
        /// </summary>
        /// <value></value>
        public List<T> errors { get; set; }

        /// <summary>
        /// Add errors collection and return itself
        /// </summary>
        [GraphQLIgnore]
        public U PushError(params T[] errors) {
            this.errors.AddRange(errors);

            return (U)this;
        }

        /// <summary>
        /// Return new instance with errors
        /// </summary>
        /// <param name="errors"></param>
        [GraphQLIgnore]
        public static U Error(params T[] errors) {
            U u = new U();
            u.errors.AddRange(errors);
            return u;
        }

        /// <summary>
        /// Returns new instance
        /// </summary>
        [GraphQLIgnore]
        public static U Success() {
            return new U();
        }

        [GraphQLIgnore]
        public void AddError(object o) {

            if (o is T) {
                T tmp = (T)o;
                this.errors.Add(tmp);
            } else {
                throw new NotSupportedException("Error type does not match base payload supported types");
            }
        }
    }

}
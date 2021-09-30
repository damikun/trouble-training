
// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using System;
using Device.Aplication.Shared.Errors;
using SharedCore.Aplication.Payload;

namespace Device.Aplication.Shared { 

    public static partial class Common {    

        public static TResponse HandleBaseCommandException<TResponse>(Exception ex ){
            IBasePayload payload = ((IBasePayload)Activator.CreateInstance<TResponse>());

            payload.AddError(new InternalServerError(ex.Message));

            return (TResponse)payload;
        }
    }
}
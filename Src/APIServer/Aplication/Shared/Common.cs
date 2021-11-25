
// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using System;
using System.Collections.Generic;
using APIServer.Aplication.Shared.Errors;
using SharedCore.Aplication.Payload;
using System.Linq;
using System.Threading.Tasks;

namespace APIServer.Aplication.Shared
{

    public static partial class Common
    {

        public static TResponse HandleBaseCommandException<TResponse>(Exception ex)
        {
            IBasePayload payload = ((IBasePayload)Activator.CreateInstance<TResponse>());

            payload.AddError(new InternalServerError(ex.Message));

            return (TResponse)payload;
        }
    }
}
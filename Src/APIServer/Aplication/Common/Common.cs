
// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using System;
using APIServer.Aplication.Shared.Errors;
using SharedCore.Aplication.Payload;

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
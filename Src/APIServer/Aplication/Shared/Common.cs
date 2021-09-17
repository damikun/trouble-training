
// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using System;
using System.Diagnostics;
using APIServer.Aplication.Shared.Errors;
using Aplication.Payload;
using Serilog;

namespace APIServer.Aplication.Shared {

    public static partial class Common {    

        // Check if object is derived from specific type
        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
            while (toCheck != null && toCheck != typeof(object)) {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur) {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        // Helper to set curron opentelemtry log error
        public static void SetOtelError(string error) {
            var current = Activity.Current;
            current?.SetTag("otel.status_code", "ERROR");
            current?.SetTag("otel.status_description", error);
        }

        public static void SetOtelError(string error, ILogger logger, bool perform_log = true) {
            SetOtelError(error);

            if(perform_log && logger != null){
                logger.Error(error);
            }
        }

        public static void CheckAndSetOtelExceptionError(Exception ex, ILogger logger) {
            if(!ex.Data.Contains("command_failed")){
                
                ex.Data.Add("command_failed",true);

                Common.SetOtelError(ex?.ToString(),logger);
            }
        }
        
        public static TResponse HandleBaseCommandException<TResponse>(Exception ex){
            IBasePayload payload = ((IBasePayload)Activator.CreateInstance<TResponse>());

            payload.AddError(new InternalServerError(ex.Message));

            return (TResponse)payload;
        }
        

    }
}
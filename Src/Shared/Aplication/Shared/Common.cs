
// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using SharedCore.Domain.Models;

namespace SharedCore.Aplication.Shared {

    public static class Common {    

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

        public async static Task<HttpRequest> HandleTracingActivityRename(HttpRequest req) {
            req.EnableBuffering();

            try{
                using (var buffer = new MemoryStream()) {

                    await req.Body.CopyToAsync(buffer);

                    buffer.Position = 0L;

                    using (var reader = new StreamReader(buffer))
                    {
                        var requestBodyAsString = await reader.ReadToEndAsync();

                        if(requestBodyAsString !=null){

                            PersistedRequestQueryBody parsed_body = null;

                            try{
                                parsed_body = JsonConvert.DeserializeObject<PersistedRequestQueryBody>(requestBodyAsString);

                                if(parsed_body !=null && parsed_body.id !=null){

                                    if(Activity.Current != null){
                                        Activity.Current.DisplayName=string.Format(
                                            "Path: {0}, Id:{1}","/graphql",parsed_body.id);
                                    }
                                }
                            
                            }catch{
                                // Ignore this
                            }                                         
                        }
                    }
                }
            }finally{
                req.Body.Position = 0L;
            }     

            return req;
        }
        
    }
}
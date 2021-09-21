
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BFF.Configuration {

    public class PersistedRequestQueryBody {

        public string id {get;set;}

        // Ignoring any other as variables
    }

    public static class TracingExtensions {
        public async static Task<HttpRequest> HandleTraingActivityRename(HttpRequest req){
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
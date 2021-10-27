using System;
using Serilog;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using SharedCore.Aplication.Interfaces;

namespace SharedCore.Aplication.Services {

    /// <summary>Telemetry helpers</summary>
    public class Telemetry : ITelemetry {

        private readonly IOptions<TelemetryOptions> _options;

        /// <summary>
        /// Main constructor of TelemetryProvider
        /// </summary>
        public Telemetry(IOptions<TelemetryOptions> options) {

            _options = options;

            AppSource = new(_options.Value.SourceName);

         }

        public Activity Current { get { return Activity.Current; } }

        public ActivitySource AppSource {get; private set;}

        public void SetOtelError(string error, bool log = false) {

            var current = Activity.Current;
            current?.SetTag("otel.status_code", "ERROR");

            if(!string.IsNullOrWhiteSpace(error)){
   
                current?.SetTag("otel.status_description", error);

                if(log)
                    Log.Error(error);
            }
        }

        public void SetOtelError(Exception ex) {

            if(ex == null)
                return;

            if(!ex.Data.Contains("command_failed")){
                
               SetOtelError(ex.ToString(),true);
            }
        }

        public void SetOtelWarning(string message) {
            
            var current = Activity.Current;

            current?.SetTag("otel.status_code", "WARNING");

            if(!string.IsNullOrWhiteSpace(message)){
                current?.SetTag("otel.status_description", message);
            }
        }
    }
}

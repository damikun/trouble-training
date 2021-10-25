using System;
using System.Diagnostics;

namespace SharedCore.Aplication.Interfaces {

    /// <summary>
    /// Telemetry helpers
    /// </summary>
    public interface ITelemetry {
            
        Activity Current {get;}


        void SetOtelError(string error, bool log = false);

        void SetOtelError(Exception ex);

        void SetOtelWarning(string error);

    }
   
}
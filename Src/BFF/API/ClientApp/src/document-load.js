import { WebTracerProvider,SimpleSpanProcessor  } from '@opentelemetry/sdk-trace-web';
import { ZoneContextManager } from '@opentelemetry/context-zone';
import { FetchInstrumentation } from '@opentelemetry/instrumentation-fetch';
import { registerInstrumentations } from '@opentelemetry/instrumentation';
const { CollectorTraceExporter } =  require('@opentelemetry/exporter-collector-http');

const collectorOptions = {
     url: 'https://localhost:5015/traces',
    headers: {
      "Content-Type": "application/json"
     },
    concurrencyLimit: 10, // an optional limit on pending requests
  };
  
const provider = new WebTracerProvider();
const exporter = new CollectorTraceExporter(collectorOptions);
const fetchInstrumentation = new FetchInstrumentation();

provider.addSpanProcessor(new SimpleSpanProcessor(exporter));

provider.register({
  // Changing default contextManager to use ZoneContextManager - supports asynchronous operations - optional
  contextManager: new ZoneContextManager(),
});

fetchInstrumentation.setTracerProvider(provider);

// Registering instrumentations
registerInstrumentations({
    instrumentations: [new FetchInstrumentation()],
});
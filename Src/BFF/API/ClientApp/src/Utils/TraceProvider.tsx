import React from "react";
import { WebTracerProvider  } from '@opentelemetry/sdk-trace-web';
import { SimpleSpanProcessor } from '@opentelemetry/sdk-trace-base';
import { ZoneContextManager } from '@opentelemetry/context-zone';
import { FetchInstrumentation } from '@opentelemetry/instrumentation-fetch';
import { registerInstrumentations } from '@opentelemetry/instrumentation';
import { Resource } from '@opentelemetry/resources';
import { OTLPTraceExporter  } from "@opentelemetry/exporter-trace-otlp-http"
import {TRACES_ENDPOINT} from "../constants"

const collectorOptions = {
    url: TRACES_ENDPOINT, 
    headers: {
    "Content-Type": "application/json", 
    'Access-Control-Allow-Headers': '*',
    'X-CSRF': '1',
  },
    concurrencyLimit: 10,
  };

// Trace provider (Main aplication trace)
const provider = new WebTracerProvider({
  resource: new Resource({
    "service.name": "Frontend",
  }
  )});

// Exporter (opentelemetry collector hidden behind bff proxy)
const exporter = new OTLPTraceExporter (collectorOptions);

// Instrumentation configurations for frontend
const fetchInstrumentation = new FetchInstrumentation({
  ignoreUrls : ["https://some-ignored-url.com"]
});

fetchInstrumentation.setTracerProvider(provider);

provider.addSpanProcessor(new SimpleSpanProcessor(exporter));

provider.register({
  contextManager: new ZoneContextManager(),
});

// Registering instrumentations
registerInstrumentations({
    instrumentations: [new FetchInstrumentation()],
});

export type TraceProviderProps = {
  children?: React.ReactNode;
};

export default function TraceProvider({ children }: TraceProviderProps) {

  return (
    <>
      {children}
    </>
  );
}

/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 77cbb508cd2444c1f28ab1b7a1d7e1ca */

import { ConcreteRequest } from "relay-runtime";

export type HookEventType = "FILE" | "HOOK" | "MILESTONE" | "NOTE" | "PROJECT" | "%future added value";
export type RecordResult = "DATA_QUERY_ERROR" | "HTTP_ERROR" | "OK" | "PARAMETER_ERROR" | "UNDEFINED" | "%future added value";
export type HooksLogsDetailQueryVariables = {
    hook_record_id: string;
};
export type HooksLogsDetailQueryResponse = {
    readonly webHookRecord: {
        readonly id: string;
        readonly statusCode: number;
        readonly timestamp: string;
        readonly triggerType: HookEventType;
        readonly guid: string | null;
        readonly result: RecordResult;
        readonly webHook: {
            readonly id: string;
            readonly systemid: unknown;
            readonly webHookUrl: string | null;
        } | null;
        readonly exception: string | null;
        readonly requestHeaders: string | null;
        readonly requestBody: string | null;
        readonly responseBody: string | null;
    } | null;
};
export type HooksLogsDetailQuery = {
    readonly response: HooksLogsDetailQueryResponse;
    readonly variables: HooksLogsDetailQueryVariables;
};



/*
query HooksLogsDetailQuery(
  $hook_record_id: ID!
) {
  webHookRecord(hook_record_id: $hook_record_id) {
    id
    statusCode
    timestamp
    triggerType
    guid
    result
    webHook {
      id
      systemid
      webHookUrl
    }
    exception
    requestHeaders
    requestBody
    responseBody
  }
}
*/

const node: ConcreteRequest = (function(){
var v0 = [
  {
    "defaultValue": null,
    "kind": "LocalArgument",
    "name": "hook_record_id"
  }
],
v1 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "id",
  "storageKey": null
},
v2 = [
  {
    "alias": null,
    "args": [
      {
        "kind": "Variable",
        "name": "hook_record_id",
        "variableName": "hook_record_id"
      }
    ],
    "concreteType": "GQL_WebHookRecord",
    "kind": "LinkedField",
    "name": "webHookRecord",
    "plural": false,
    "selections": [
      (v1/*: any*/),
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "statusCode",
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "timestamp",
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "triggerType",
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "guid",
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "result",
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "concreteType": "GQL_WebHook",
        "kind": "LinkedField",
        "name": "webHook",
        "plural": false,
        "selections": [
          (v1/*: any*/),
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "systemid",
            "storageKey": null
          },
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "webHookUrl",
            "storageKey": null
          }
        ],
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "exception",
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "requestHeaders",
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "requestBody",
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "responseBody",
        "storageKey": null
      }
    ],
    "storageKey": null
  }
];
return {
  "fragment": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Fragment",
    "metadata": null,
    "name": "HooksLogsDetailQuery",
    "selections": (v2/*: any*/),
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "HooksLogsDetailQuery",
    "selections": (v2/*: any*/)
  },
  "params": {
    "id": "77cbb508cd2444c1f28ab1b7a1d7e1ca",
    "metadata": {},
    "name": "HooksLogsDetailQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = 'b701ab0888657efdb4d2992f4989feec';
export default node;

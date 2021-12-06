/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 1ca9aa6c32da8b0f79d98c000542072c */

import { ConcreteRequest } from "relay-runtime";

export type HookEventType = "FILE" | "HOOK" | "MILESTONE" | "NOTE" | "PROJECT" | "%future added value";
export type HooksEditQueryVariables = {
    hookid: string;
};
export type HooksEditQueryResponse = {
    readonly webHookById: {
        readonly id: string;
        readonly systemid: unknown;
        readonly webHookUrl: string | null;
        readonly isActive: boolean;
        readonly listeningEvents: ReadonlyArray<HookEventType> | null;
    } | null;
    readonly webHookEventsTriggers: ReadonlyArray<string | null> | null;
};
export type HooksEditQuery = {
    readonly response: HooksEditQueryResponse;
    readonly variables: HooksEditQueryVariables;
};



/*
query HooksEditQuery(
  $hookid: ID!
) {
  webHookById(webhook_id: $hookid) {
    id
    systemid
    webHookUrl
    isActive
    listeningEvents
  }
  webHookEventsTriggers
}
*/

const node: ConcreteRequest = (function(){
var v0 = [
  {
    "defaultValue": null,
    "kind": "LocalArgument",
    "name": "hookid"
  }
],
v1 = [
  {
    "alias": null,
    "args": [
      {
        "kind": "Variable",
        "name": "webhook_id",
        "variableName": "hookid"
      }
    ],
    "concreteType": "GQL_WebHook",
    "kind": "LinkedField",
    "name": "webHookById",
    "plural": false,
    "selections": [
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "id",
        "storageKey": null
      },
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
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "isActive",
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "listeningEvents",
        "storageKey": null
      }
    ],
    "storageKey": null
  },
  {
    "alias": null,
    "args": null,
    "kind": "ScalarField",
    "name": "webHookEventsTriggers",
    "storageKey": null
  }
];
return {
  "fragment": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Fragment",
    "metadata": null,
    "name": "HooksEditQuery",
    "selections": (v1/*: any*/),
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "HooksEditQuery",
    "selections": (v1/*: any*/)
  },
  "params": {
    "id": "1ca9aa6c32da8b0f79d98c000542072c",
    "metadata": {},
    "name": "HooksEditQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = '2229ad24e59a9df850317e0530ca957b';
export default node;

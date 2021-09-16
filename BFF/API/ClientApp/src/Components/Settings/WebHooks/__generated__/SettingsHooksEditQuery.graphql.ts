/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 306de519f3683359cc01e0832dfd6830 */

import { ConcreteRequest } from "relay-runtime";
export type HookEventType = "FILE" | "HOOK" | "MILESTONE" | "NOTE" | "PROJECT" | "%future added value";
export type SettingsHooksEditQueryVariables = {
    hookid: string;
};
export type SettingsHooksEditQueryResponse = {
    readonly webHookById: {
        readonly id: string;
        readonly systemid: unknown;
        readonly webHookUrl: string | null;
        readonly isActive: boolean;
        readonly listeningEvents: ReadonlyArray<HookEventType> | null;
    } | null;
    readonly webHookEventsTriggers: ReadonlyArray<string | null> | null;
};
export type SettingsHooksEditQuery = {
    readonly response: SettingsHooksEditQueryResponse;
    readonly variables: SettingsHooksEditQueryVariables;
};



/*
query SettingsHooksEditQuery(
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
    "name": "SettingsHooksEditQuery",
    "selections": (v1/*: any*/),
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "SettingsHooksEditQuery",
    "selections": (v1/*: any*/)
  },
  "params": {
    "id": "306de519f3683359cc01e0832dfd6830",
    "metadata": {},
    "name": "SettingsHooksEditQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = '189fd53b6b30bf85596af9b30d21c140';
export default node;

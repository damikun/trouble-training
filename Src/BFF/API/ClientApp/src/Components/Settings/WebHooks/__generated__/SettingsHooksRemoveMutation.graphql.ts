/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 88d3fb8a0c7cef7c5d314782f703ed0b */

import { ConcreteRequest } from "relay-runtime";

export type RemoveWebHookInput = {
    webHookId: unknown;
};
export type SettingsHooksRemoveMutationVariables = {
    request?: RemoveWebHookInput | null | undefined;
    connections: Array<string>;
};
export type SettingsHooksRemoveMutationResponse = {
    readonly removeWebHook: {
        readonly errors: ReadonlyArray<{
            readonly message?: string | null | undefined;
        } | null> | null;
        readonly removed_id: string | null;
    } | null;
};
export type SettingsHooksRemoveMutation = {
    readonly response: SettingsHooksRemoveMutationResponse;
    readonly variables: SettingsHooksRemoveMutationVariables;
};



/*
mutation SettingsHooksRemoveMutation(
  $request: RemoveWebHookInput
) {
  removeWebHook(request: $request) {
    errors {
      __typename
      ... on IBaseError {
        __isIBaseError: __typename
        message
      }
    }
    removed_id
  }
}
*/

const node: ConcreteRequest = (function(){
var v0 = {
  "defaultValue": null,
  "kind": "LocalArgument",
  "name": "connections"
},
v1 = {
  "defaultValue": null,
  "kind": "LocalArgument",
  "name": "request"
},
v2 = [
  {
    "kind": "Variable",
    "name": "request",
    "variableName": "request"
  }
],
v3 = {
  "kind": "InlineFragment",
  "selections": [
    {
      "alias": null,
      "args": null,
      "kind": "ScalarField",
      "name": "message",
      "storageKey": null
    }
  ],
  "type": "IBaseError",
  "abstractKey": "__isIBaseError"
},
v4 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "removed_id",
  "storageKey": null
};
return {
  "fragment": {
    "argumentDefinitions": [
      (v0/*: any*/),
      (v1/*: any*/)
    ],
    "kind": "Fragment",
    "metadata": null,
    "name": "SettingsHooksRemoveMutation",
    "selections": [
      {
        "alias": null,
        "args": (v2/*: any*/),
        "concreteType": "RemoveWebHookPayload",
        "kind": "LinkedField",
        "name": "removeWebHook",
        "plural": false,
        "selections": [
          {
            "alias": null,
            "args": null,
            "concreteType": null,
            "kind": "LinkedField",
            "name": "errors",
            "plural": true,
            "selections": [
              (v3/*: any*/)
            ],
            "storageKey": null
          },
          (v4/*: any*/)
        ],
        "storageKey": null
      }
    ],
    "type": "Mutation",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": [
      (v1/*: any*/),
      (v0/*: any*/)
    ],
    "kind": "Operation",
    "name": "SettingsHooksRemoveMutation",
    "selections": [
      {
        "alias": null,
        "args": (v2/*: any*/),
        "concreteType": "RemoveWebHookPayload",
        "kind": "LinkedField",
        "name": "removeWebHook",
        "plural": false,
        "selections": [
          {
            "alias": null,
            "args": null,
            "concreteType": null,
            "kind": "LinkedField",
            "name": "errors",
            "plural": true,
            "selections": [
              {
                "alias": null,
                "args": null,
                "kind": "ScalarField",
                "name": "__typename",
                "storageKey": null
              },
              (v3/*: any*/)
            ],
            "storageKey": null
          },
          (v4/*: any*/),
          {
            "alias": null,
            "args": null,
            "filters": null,
            "handle": "deleteEdge",
            "key": "",
            "kind": "ScalarHandle",
            "name": "removed_id",
            "handleArgs": [
              {
                "kind": "Variable",
                "name": "connections",
                "variableName": "connections"
              }
            ]
          }
        ],
        "storageKey": null
      }
    ]
  },
  "params": {
    "id": "88d3fb8a0c7cef7c5d314782f703ed0b",
    "metadata": {},
    "name": "SettingsHooksRemoveMutation",
    "operationKind": "mutation",
    "text": null
  }
};
})();
(node as any).hash = 'cebb218bd9b1b180a1c3ef597685fd38';
export default node;

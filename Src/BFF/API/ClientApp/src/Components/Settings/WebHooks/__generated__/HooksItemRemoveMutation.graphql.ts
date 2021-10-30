/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 9ed62d930b4035555b3d4c366407c1d7 */

import { ConcreteRequest } from "relay-runtime";

export type RemoveWebHookInput = {
    webHookId: unknown;
};
export type HooksItemRemoveMutationVariables = {
    request?: RemoveWebHookInput | null | undefined;
    connections: Array<string>;
};
export type HooksItemRemoveMutationResponse = {
    readonly removeWebHook: {
        readonly errors: ReadonlyArray<{
            readonly message?: string | null | undefined;
        } | null> | null;
        readonly removed_id: string | null;
    } | null;
};
export type HooksItemRemoveMutation = {
    readonly response: HooksItemRemoveMutationResponse;
    readonly variables: HooksItemRemoveMutationVariables;
};



/*
mutation HooksItemRemoveMutation(
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
    "name": "HooksItemRemoveMutation",
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
    "name": "HooksItemRemoveMutation",
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
    "id": "9ed62d930b4035555b3d4c366407c1d7",
    "metadata": {},
    "name": "HooksItemRemoveMutation",
    "operationKind": "mutation",
    "text": null
  }
};
})();
(node as any).hash = 'eb08175b4edb6266d5531a0926777bbd';
export default node;

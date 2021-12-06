/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash f59d75be0417097a84eb8cd14a774f06 */

import { ConcreteRequest } from "relay-runtime";

export type HookEventType = "FILE" | "HOOK" | "MILESTONE" | "NOTE" | "PROJECT" | "%future added value";
export type CreateWebHookInput = {
    hookEvents?: Array<HookEventType> | null | undefined;
    isActive: boolean;
    secret?: string | null | undefined;
    webHookUrl?: string | null | undefined;
};
export type HooksNewMutationVariables = {
    request?: CreateWebHookInput | null | undefined;
    connections: Array<string>;
};
export type HooksNewMutationResponse = {
    readonly createWebHook: {
        readonly errors: ReadonlyArray<{
            readonly message?: string | null | undefined;
        } | null> | null;
        readonly hook: {
            readonly id: string;
            readonly systemid: unknown;
            readonly webHookUrl: string | null;
            readonly isActive: boolean;
        } | null;
    } | null;
};
export type HooksNewMutation = {
    readonly response: HooksNewMutationResponse;
    readonly variables: HooksNewMutationVariables;
};



/*
mutation HooksNewMutation(
  $request: CreateWebHookInput
) {
  createWebHook(request: $request) {
    errors {
      __typename
      ... on IBaseError {
        __isIBaseError: __typename
        message
      }
    }
    hook {
      id
      systemid
      webHookUrl
      isActive
    }
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
  "concreteType": "GQL_WebHook",
  "kind": "LinkedField",
  "name": "hook",
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
    }
  ],
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
    "name": "HooksNewMutation",
    "selections": [
      {
        "alias": null,
        "args": (v2/*: any*/),
        "concreteType": "CreateWebHookPayload",
        "kind": "LinkedField",
        "name": "createWebHook",
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
    "name": "HooksNewMutation",
    "selections": [
      {
        "alias": null,
        "args": (v2/*: any*/),
        "concreteType": "CreateWebHookPayload",
        "kind": "LinkedField",
        "name": "createWebHook",
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
            "handle": "prependNode",
            "key": "",
            "kind": "LinkedHandle",
            "name": "hook",
            "handleArgs": [
              {
                "kind": "Variable",
                "name": "connections",
                "variableName": "connections"
              },
              {
                "kind": "Literal",
                "name": "edgeTypeName",
                "value": "GQL_WebHook"
              }
            ]
          }
        ],
        "storageKey": null
      }
    ]
  },
  "params": {
    "id": "f59d75be0417097a84eb8cd14a774f06",
    "metadata": {},
    "name": "HooksNewMutation",
    "operationKind": "mutation",
    "text": null
  }
};
})();
(node as any).hash = '3d10ada554bfe0338acb4be199a1c209';
export default node;

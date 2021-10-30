/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 0708eda8907dca524f5a173a75cfb4ea */

import { ConcreteRequest } from "relay-runtime";

export type HookEventType = "FILE" | "HOOK" | "MILESTONE" | "NOTE" | "PROJECT" | "%future added value";
export type UpdateWebHookInput = {
    webHookId: unknown;
    webHookUrl?: string | null | undefined;
    secret?: string | null | undefined;
    isActive: boolean;
    hookEvents?: Array<HookEventType> | null | undefined;
};
export type HooksEditMutationVariables = {
    request?: UpdateWebHookInput | null | undefined;
};
export type HooksEditMutationResponse = {
    readonly updateWebHook: {
        readonly errors: ReadonlyArray<{
            readonly message?: string | null | undefined;
        } | null> | null;
        readonly hook: {
            readonly id: string;
            readonly webHookUrl: string | null;
            readonly isActive: boolean;
            readonly listeningEvents: ReadonlyArray<HookEventType> | null;
        } | null;
    } | null;
};
export type HooksEditMutation = {
    readonly response: HooksEditMutationResponse;
    readonly variables: HooksEditMutationVariables;
};



/*
mutation HooksEditMutation(
  $request: UpdateWebHookInput
) {
  updateWebHook(request: $request) {
    errors {
      __typename
      ... on IBaseError {
        __isIBaseError: __typename
        message
      }
    }
    hook {
      id
      webHookUrl
      isActive
      listeningEvents
    }
  }
}
*/

const node: ConcreteRequest = (function(){
var v0 = [
  {
    "defaultValue": null,
    "kind": "LocalArgument",
    "name": "request"
  }
],
v1 = [
  {
    "kind": "Variable",
    "name": "request",
    "variableName": "request"
  }
],
v2 = {
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
v3 = {
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
};
return {
  "fragment": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Fragment",
    "metadata": null,
    "name": "HooksEditMutation",
    "selections": [
      {
        "alias": null,
        "args": (v1/*: any*/),
        "concreteType": "UpdateWebHookPayload",
        "kind": "LinkedField",
        "name": "updateWebHook",
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
              (v2/*: any*/)
            ],
            "storageKey": null
          },
          (v3/*: any*/)
        ],
        "storageKey": null
      }
    ],
    "type": "Mutation",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "HooksEditMutation",
    "selections": [
      {
        "alias": null,
        "args": (v1/*: any*/),
        "concreteType": "UpdateWebHookPayload",
        "kind": "LinkedField",
        "name": "updateWebHook",
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
              (v2/*: any*/)
            ],
            "storageKey": null
          },
          (v3/*: any*/)
        ],
        "storageKey": null
      }
    ]
  },
  "params": {
    "id": "0708eda8907dca524f5a173a75cfb4ea",
    "metadata": {},
    "name": "HooksEditMutation",
    "operationKind": "mutation",
    "text": null
  }
};
})();
(node as any).hash = 'abc1bbf068d9013e769762f6a4792e73';
export default node;

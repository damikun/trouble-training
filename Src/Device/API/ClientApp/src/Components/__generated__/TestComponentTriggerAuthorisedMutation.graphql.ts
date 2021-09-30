/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ConcreteRequest } from "relay-runtime";
export type TestComponentTriggerAuthorisedMutationVariables = {};
export type TestComponentTriggerAuthorisedMutationResponse = {
    readonly triggerAuthorisedRequest: {
        readonly errors: ReadonlyArray<{
            readonly message?: string | null;
        } | null> | null;
    } | null;
};
export type TestComponentTriggerAuthorisedMutation = {
    readonly response: TestComponentTriggerAuthorisedMutationResponse;
    readonly variables: TestComponentTriggerAuthorisedMutationVariables;
};



/*
mutation TestComponentTriggerAuthorisedMutation {
  triggerAuthorisedRequest {
    errors {
      __typename
      ... on IBaseError {
        __isIBaseError: __typename
        message
      }
    }
  }
}
*/

const node: ConcreteRequest = (function(){
var v0 = {
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
};
return {
  "fragment": {
    "argumentDefinitions": [],
    "kind": "Fragment",
    "metadata": null,
    "name": "TestComponentTriggerAuthorisedMutation",
    "selections": [
      {
        "alias": null,
        "args": null,
        "concreteType": "Trigger_AuthorisedPayload",
        "kind": "LinkedField",
        "name": "triggerAuthorisedRequest",
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
              (v0/*: any*/)
            ],
            "storageKey": null
          }
        ],
        "storageKey": null
      }
    ],
    "type": "Mutation",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": [],
    "kind": "Operation",
    "name": "TestComponentTriggerAuthorisedMutation",
    "selections": [
      {
        "alias": null,
        "args": null,
        "concreteType": "Trigger_AuthorisedPayload",
        "kind": "LinkedField",
        "name": "triggerAuthorisedRequest",
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
              (v0/*: any*/)
            ],
            "storageKey": null
          }
        ],
        "storageKey": null
      }
    ]
  },
  "params": {
    "cacheID": "a98db2823a3058ef27b0e55929b97cd1",
    "id": null,
    "metadata": {},
    "name": "TestComponentTriggerAuthorisedMutation",
    "operationKind": "mutation",
    "text": "mutation TestComponentTriggerAuthorisedMutation {\n  triggerAuthorisedRequest {\n    errors {\n      __typename\n      ... on IBaseError {\n        __isIBaseError: __typename\n        message\n      }\n    }\n  }\n}\n"
  }
};
})();
(node as any).hash = '7c0e6fdedaf8a3fd8ca03c125125ab32';
export default node;

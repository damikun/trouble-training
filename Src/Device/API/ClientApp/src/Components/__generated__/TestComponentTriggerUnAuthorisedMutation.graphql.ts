/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ConcreteRequest } from "relay-runtime";
export type TestComponentTriggerUnAuthorisedMutationVariables = {};
export type TestComponentTriggerUnAuthorisedMutationResponse = {
    readonly triggerUnAuthorisedRequest: {
        readonly errors: ReadonlyArray<{
            readonly message?: string | null;
        } | null> | null;
    } | null;
};
export type TestComponentTriggerUnAuthorisedMutation = {
    readonly response: TestComponentTriggerUnAuthorisedMutationResponse;
    readonly variables: TestComponentTriggerUnAuthorisedMutationVariables;
};



/*
mutation TestComponentTriggerUnAuthorisedMutation {
  triggerUnAuthorisedRequest {
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
    "name": "TestComponentTriggerUnAuthorisedMutation",
    "selections": [
      {
        "alias": null,
        "args": null,
        "concreteType": "Trigger_UnAuthorisedPayload",
        "kind": "LinkedField",
        "name": "triggerUnAuthorisedRequest",
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
    "name": "TestComponentTriggerUnAuthorisedMutation",
    "selections": [
      {
        "alias": null,
        "args": null,
        "concreteType": "Trigger_UnAuthorisedPayload",
        "kind": "LinkedField",
        "name": "triggerUnAuthorisedRequest",
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
    "cacheID": "47669b88d2e320a312fbd0b5c61f9859",
    "id": null,
    "metadata": {},
    "name": "TestComponentTriggerUnAuthorisedMutation",
    "operationKind": "mutation",
    "text": "mutation TestComponentTriggerUnAuthorisedMutation {\n  triggerUnAuthorisedRequest {\n    errors {\n      __typename\n      ... on IBaseError {\n        __isIBaseError: __typename\n        message\n      }\n    }\n  }\n}\n"
  }
};
})();
(node as any).hash = 'f0cfba3c0fe479705f28180f7694b386';
export default node;

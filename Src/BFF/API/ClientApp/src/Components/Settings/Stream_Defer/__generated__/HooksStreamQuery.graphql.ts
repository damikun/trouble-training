/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash b99f8558b91c57b70256e08aad9e539e */

import { ConcreteRequest } from "relay-runtime";

import { FragmentRefs } from "relay-runtime";
export type HooksStreamQueryVariables = {};
export type HooksStreamQueryResponse = {
    readonly " $fragmentRefs": FragmentRefs<"HooksStreamListFragment">;
};
export type HooksStreamQuery = {
    readonly response: HooksStreamQueryResponse;
    readonly variables: HooksStreamQueryVariables;
};



/*
query HooksStreamQuery {
  ...HooksStreamListFragment
}

fragment HooksStreamListFragment on Query {
  webHooksTestStream @stream(label: "HooksStreamListFragment$stream$webHooksTestStream", initial_count: 2) {
    id
    ...ItemFragment
  }
}

fragment ItemFragment on GQL_WebHook {
  id
  systemid
  webHookUrl
  isActive
}
*/

const node: ConcreteRequest = {
  "fragment": {
    "argumentDefinitions": [],
    "kind": "Fragment",
    "metadata": null,
    "name": "HooksStreamQuery",
    "selections": [
      {
        "args": null,
        "kind": "FragmentSpread",
        "name": "HooksStreamListFragment"
      }
    ],
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": [],
    "kind": "Operation",
    "name": "HooksStreamQuery",
    "selections": [
      {
        "if": null,
        "kind": "Stream",
        "label": "HooksStreamListFragment$stream$webHooksTestStream",
        "selections": [
          {
            "alias": null,
            "args": null,
            "concreteType": "GQL_WebHook",
            "kind": "LinkedField",
            "name": "webHooksTestStream",
            "plural": true,
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
          }
        ]
      }
    ]
  },
  "params": {
    "id": "b99f8558b91c57b70256e08aad9e539e",
    "metadata": {},
    "name": "HooksStreamQuery",
    "operationKind": "query",
    "text": null
  }
};
(node as any).hash = 'a744c3d17d3dacea44eac272d12dba7e';
export default node;

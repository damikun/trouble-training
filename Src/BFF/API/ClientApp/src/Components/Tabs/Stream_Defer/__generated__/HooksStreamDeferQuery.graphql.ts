/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 9cddf4e406483e947e1ddf19be01d965 */

import { ConcreteRequest } from "relay-runtime";

import { FragmentRefs } from "relay-runtime";
export type HooksStreamDeferQueryVariables = {};
export type HooksStreamDeferQueryResponse = {
    readonly " $fragmentRefs": FragmentRefs<"HooksStreamDeferListFragment">;
};
export type HooksStreamDeferQuery = {
    readonly response: HooksStreamDeferQueryResponse;
    readonly variables: HooksStreamDeferQueryVariables;
};



/*
query HooksStreamDeferQuery {
  ...HooksStreamDeferListFragment
}

fragment HooksStreamDeferListFragment on Query {
  webHooksTestStream @stream(label: "HooksStreamDeferListFragment$stream$webHooksTestStream", initial_count: 2) {
    id
    ...ItemFragment @defer(label: "HooksStreamDeferListFragment$defer$ItemFragment")
  }
}

fragment ItemFragment on GQL_WebHook {
  id
  systemid
  webHookUrl
  isActive
}
*/

const node: ConcreteRequest = (function(){
var v0 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "id",
  "storageKey": null
};
return {
  "fragment": {
    "argumentDefinitions": [],
    "kind": "Fragment",
    "metadata": null,
    "name": "HooksStreamDeferQuery",
    "selections": [
      {
        "args": null,
        "kind": "FragmentSpread",
        "name": "HooksStreamDeferListFragment"
      }
    ],
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": [],
    "kind": "Operation",
    "name": "HooksStreamDeferQuery",
    "selections": [
      {
        "if": null,
        "kind": "Stream",
        "label": "HooksStreamDeferListFragment$stream$webHooksTestStream",
        "selections": [
          {
            "alias": null,
            "args": null,
            "concreteType": "GQL_WebHook",
            "kind": "LinkedField",
            "name": "webHooksTestStream",
            "plural": true,
            "selections": [
              (v0/*: any*/),
              {
                "if": null,
                "kind": "Defer",
                "label": "HooksStreamDeferListFragment$defer$ItemFragment",
                "selections": [
                  (v0/*: any*/),
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
                ]
              }
            ],
            "storageKey": null
          }
        ]
      }
    ]
  },
  "params": {
    "id": "9cddf4e406483e947e1ddf19be01d965",
    "metadata": {},
    "name": "HooksStreamDeferQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = 'e09561fd43f2cee063a7e2a25213175c';
export default node;

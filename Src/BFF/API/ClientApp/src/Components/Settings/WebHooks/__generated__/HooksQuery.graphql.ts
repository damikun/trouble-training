/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 6a44e73da11a3654ece8143e6b102906 */

import { ConcreteRequest } from "relay-runtime";

import { FragmentRefs } from "relay-runtime";
export type HooksQueryVariables = {};
export type HooksQueryResponse = {
    readonly " $fragmentRefs": FragmentRefs<"HooksListFragment">;
};
export type HooksQuery = {
    readonly response: HooksQueryResponse;
    readonly variables: HooksQueryVariables;
};



/*
query HooksQuery {
  ...HooksListFragment
}

fragment HooksItemFragment on GQL_WebHook {
  id
  systemid
  webHookUrl
  isActive
}

fragment HooksListFragment on Query {
  webhooks {
    pageInfo {
      hasPreviousPage
      hasNextPage
      startCursor
      endCursor
    }
    edges @stream(label: "HooksListFragment$stream$edges", initialCount: 2) {
      node {
        id
        ...HooksItemFragment @defer(label: "HooksListFragment$defer$HooksItemFragment")
        __typename
      }
      cursor
    }
  }
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
    "name": "HooksQuery",
    "selections": [
      {
        "args": null,
        "kind": "FragmentSpread",
        "name": "HooksListFragment"
      }
    ],
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": [],
    "kind": "Operation",
    "name": "HooksQuery",
    "selections": [
      {
        "alias": null,
        "args": null,
        "concreteType": "WebhooksConnection",
        "kind": "LinkedField",
        "name": "webhooks",
        "plural": false,
        "selections": [
          {
            "alias": null,
            "args": null,
            "concreteType": "PageInfo",
            "kind": "LinkedField",
            "name": "pageInfo",
            "plural": false,
            "selections": [
              {
                "alias": null,
                "args": null,
                "kind": "ScalarField",
                "name": "hasPreviousPage",
                "storageKey": null
              },
              {
                "alias": null,
                "args": null,
                "kind": "ScalarField",
                "name": "hasNextPage",
                "storageKey": null
              },
              {
                "alias": null,
                "args": null,
                "kind": "ScalarField",
                "name": "startCursor",
                "storageKey": null
              },
              {
                "alias": null,
                "args": null,
                "kind": "ScalarField",
                "name": "endCursor",
                "storageKey": null
              }
            ],
            "storageKey": null
          },
          {
            "if": null,
            "kind": "Stream",
            "label": "HooksListFragment$stream$edges",
            "selections": [
              {
                "alias": null,
                "args": null,
                "concreteType": "WebhooksEdge",
                "kind": "LinkedField",
                "name": "edges",
                "plural": true,
                "selections": [
                  {
                    "alias": null,
                    "args": null,
                    "concreteType": "GQL_WebHook",
                    "kind": "LinkedField",
                    "name": "node",
                    "plural": false,
                    "selections": [
                      (v0/*: any*/),
                      {
                        "alias": null,
                        "args": null,
                        "kind": "ScalarField",
                        "name": "__typename",
                        "storageKey": null
                      },
                      {
                        "if": null,
                        "kind": "Defer",
                        "label": "HooksListFragment$defer$HooksItemFragment",
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
                  },
                  {
                    "alias": null,
                    "args": null,
                    "kind": "ScalarField",
                    "name": "cursor",
                    "storageKey": null
                  }
                ],
                "storageKey": null
              }
            ]
          },
          {
            "kind": "ClientExtension",
            "selections": [
              {
                "alias": null,
                "args": null,
                "kind": "ScalarField",
                "name": "__id",
                "storageKey": null
              }
            ]
          }
        ],
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "filters": null,
        "handle": "connection",
        "key": "HooksListConnection_webhooks",
        "kind": "LinkedHandle",
        "name": "webhooks"
      }
    ]
  },
  "params": {
    "id": "6a44e73da11a3654ece8143e6b102906",
    "metadata": {},
    "name": "HooksQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = '85969c49798468722e2421e1dc4ef65e';
export default node;

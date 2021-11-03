/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 297466b8de04fddca7c17646fa8a901a */

import { ConcreteRequest } from "relay-runtime";

import { FragmentRefs } from "relay-runtime";
export type HooksListRefetchQueryVariables = {
    after?: string | null | undefined;
    first?: number | null | undefined;
};
export type HooksListRefetchQueryResponse = {
    readonly " $fragmentRefs": FragmentRefs<"HooksListFragment">;
};
export type HooksListRefetchQuery = {
    readonly response: HooksListRefetchQueryResponse;
    readonly variables: HooksListRefetchQueryVariables;
};



/*
query HooksListRefetchQuery(
  $after: String
  $first: Int
) {
  ...HooksListFragment_2HEEH6
}

fragment HooksItemFragment on GQL_WebHook {
  id
  systemid
  webHookUrl
  isActive
}

fragment HooksListFragment_2HEEH6 on Query {
  webhooks(first: $first, after: $after) {
    pageInfo {
      hasPreviousPage
      hasNextPage
      startCursor
      endCursor
    }
    edges @stream(label: "HooksListFragment$stream$edges", initial_count: 2) {
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
var v0 = [
  {
    "defaultValue": null,
    "kind": "LocalArgument",
    "name": "after"
  },
  {
    "defaultValue": null,
    "kind": "LocalArgument",
    "name": "first"
  }
],
v1 = [
  {
    "kind": "Variable",
    "name": "after",
    "variableName": "after"
  },
  {
    "kind": "Variable",
    "name": "first",
    "variableName": "first"
  }
],
v2 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "id",
  "storageKey": null
};
return {
  "fragment": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Fragment",
    "metadata": null,
    "name": "HooksListRefetchQuery",
    "selections": [
      {
        "args": (v1/*: any*/),
        "kind": "FragmentSpread",
        "name": "HooksListFragment"
      }
    ],
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "HooksListRefetchQuery",
    "selections": [
      {
        "alias": null,
        "args": (v1/*: any*/),
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
                      (v2/*: any*/),
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
                          (v2/*: any*/),
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
        "args": (v1/*: any*/),
        "filters": null,
        "handle": "connection",
        "key": "HooksListConnection_webhooks",
        "kind": "LinkedHandle",
        "name": "webhooks"
      }
    ]
  },
  "params": {
    "id": "297466b8de04fddca7c17646fa8a901a",
    "metadata": {},
    "name": "HooksListRefetchQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = '3110e82c2371d7327607d76ff9664771';
export default node;

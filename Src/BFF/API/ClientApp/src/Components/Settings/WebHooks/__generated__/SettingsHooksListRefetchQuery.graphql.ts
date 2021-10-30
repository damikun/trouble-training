/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 4290e22caf0448421a7a4f292a74e528 */

import { ConcreteRequest } from "relay-runtime";

import { FragmentRefs } from "relay-runtime";
export type SettingsHooksListRefetchQueryVariables = {
    after?: string | null | undefined;
    first?: number | null | undefined;
};
export type SettingsHooksListRefetchQueryResponse = {
    readonly " $fragmentRefs": FragmentRefs<"SettingsHooksListFragment">;
};
export type SettingsHooksListRefetchQuery = {
    readonly response: SettingsHooksListRefetchQueryResponse;
    readonly variables: SettingsHooksListRefetchQueryVariables;
};



/*
query SettingsHooksListRefetchQuery(
  $after: String
  $first: Int
) {
  ...SettingsHooksListFragment_2HEEH6
}

fragment SettingsHooksItemFragment on GQL_WebHook {
  id
  systemid
  webHookUrl
  isActive
}

fragment SettingsHooksListFragment_2HEEH6 on Query {
  webhooks(first: $first, after: $after) {
    pageInfo {
      hasPreviousPage
      hasNextPage
      startCursor
      endCursor
    }
    edges @stream(label: "SettingsHooksListFragment$stream$edges", initialCount: 1) {
      node {
        id
        ...SettingsHooksItemFragment
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
];
return {
  "fragment": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Fragment",
    "metadata": null,
    "name": "SettingsHooksListRefetchQuery",
    "selections": [
      {
        "args": (v1/*: any*/),
        "kind": "FragmentSpread",
        "name": "SettingsHooksListFragment"
      }
    ],
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "SettingsHooksListRefetchQuery",
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
            "label": "SettingsHooksListFragment$stream$edges",
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
                      },
                      {
                        "alias": null,
                        "args": null,
                        "kind": "ScalarField",
                        "name": "__typename",
                        "storageKey": null
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
        "key": "SettingsHooksListConnection_webhooks",
        "kind": "LinkedHandle",
        "name": "webhooks"
      }
    ]
  },
  "params": {
    "id": "4290e22caf0448421a7a4f292a74e528",
    "metadata": {},
    "name": "SettingsHooksListRefetchQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = '1469bfb1638f97f64377f02fafadb220';
export default node;

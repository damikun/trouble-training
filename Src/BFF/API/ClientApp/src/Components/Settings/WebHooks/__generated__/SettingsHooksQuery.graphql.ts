/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 8a72a6d2d4faa4205991e8499e1721f1 */

import { ConcreteRequest } from "relay-runtime";
import { FragmentRefs } from "relay-runtime";
export type SettingsHooksQueryVariables = {};
export type SettingsHooksQueryResponse = {
    readonly " $fragmentRefs": FragmentRefs<"SettingsHooksListFragment">;
};
export type SettingsHooksQuery = {
    readonly response: SettingsHooksQueryResponse;
    readonly variables: SettingsHooksQueryVariables;
};



/*
query SettingsHooksQuery {
  ...SettingsHooksListFragment
}

fragment SettingsHooksItemFragment on GQL_WebHook {
  id
  systemid
  webHookUrl
  isActive
}

fragment SettingsHooksListFragment on Query {
  webhooks {
    pageInfo {
      hasPreviousPage
      hasNextPage
      startCursor
      endCursor
    }
    edges {
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

const node: ConcreteRequest = {
  "fragment": {
    "argumentDefinitions": [],
    "kind": "Fragment",
    "metadata": null,
    "name": "SettingsHooksQuery",
    "selections": [
      {
        "args": null,
        "kind": "FragmentSpread",
        "name": "SettingsHooksListFragment"
      }
    ],
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": [],
    "kind": "Operation",
    "name": "SettingsHooksQuery",
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
        "key": "SettingsHooksListConnection_webhooks",
        "kind": "LinkedHandle",
        "name": "webhooks"
      }
    ]
  },
  "params": {
    "id": "8a72a6d2d4faa4205991e8499e1721f1",
    "metadata": {},
    "name": "SettingsHooksQuery",
    "operationKind": "query",
    "text": null
  }
};
(node as any).hash = '849dae2a7d5589901579ec361f0bfff6';
export default node;

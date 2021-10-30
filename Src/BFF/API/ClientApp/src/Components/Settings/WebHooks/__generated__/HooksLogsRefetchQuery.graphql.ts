/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash cf33244e99070c3d23f796bdfcbb4e8b */

import { ConcreteRequest } from "relay-runtime";

import { FragmentRefs } from "relay-runtime";
export type HooksLogsRefetchQueryVariables = {
    after?: string | null | undefined;
    first?: number | null | undefined;
    hookid: string;
};
export type HooksLogsRefetchQueryResponse = {
    readonly " $fragmentRefs": FragmentRefs<"HooksLogsFragment_webHookRecords">;
};
export type HooksLogsRefetchQuery = {
    readonly response: HooksLogsRefetchQueryResponse;
    readonly variables: HooksLogsRefetchQueryVariables;
};



/*
query HooksLogsRefetchQuery(
  $after: String
  $first: Int
  $hookid: ID!
) {
  ...HooksLogsFragment_webHookRecords_3PuEnP
}

fragment HooksLogsFragment_webHookRecords_3PuEnP on Query {
  webHookRecords(first: $first, after: $after, hook_id: $hookid) {
    pageInfo {
      hasPreviousPage
      hasNextPage
      startCursor
      endCursor
    }
    edges {
      cursor
      node {
        id
        ...HooksLogsItemFragment
        __typename
      }
    }
  }
}

fragment HooksLogsItemFragment on GQL_WebHookRecord {
  id
  statusCode
  timestamp
  triggerType
  guid
  result
  webHook {
    id
    systemid
    webHookUrl
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
  },
  {
    "defaultValue": null,
    "kind": "LocalArgument",
    "name": "hookid"
  }
],
v1 = {
  "kind": "Variable",
  "name": "after",
  "variableName": "after"
},
v2 = {
  "kind": "Variable",
  "name": "first",
  "variableName": "first"
},
v3 = [
  (v1/*: any*/),
  (v2/*: any*/),
  {
    "kind": "Variable",
    "name": "hook_id",
    "variableName": "hookid"
  }
],
v4 = {
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
    "name": "HooksLogsRefetchQuery",
    "selections": [
      {
        "args": [
          (v1/*: any*/),
          (v2/*: any*/),
          {
            "kind": "Variable",
            "name": "hookid",
            "variableName": "hookid"
          }
        ],
        "kind": "FragmentSpread",
        "name": "HooksLogsFragment_webHookRecords"
      }
    ],
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "HooksLogsRefetchQuery",
    "selections": [
      {
        "alias": null,
        "args": (v3/*: any*/),
        "concreteType": "WebHookRecordsConnection",
        "kind": "LinkedField",
        "name": "webHookRecords",
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
            "concreteType": "WebHookRecordsEdge",
            "kind": "LinkedField",
            "name": "edges",
            "plural": true,
            "selections": [
              {
                "alias": null,
                "args": null,
                "kind": "ScalarField",
                "name": "cursor",
                "storageKey": null
              },
              {
                "alias": null,
                "args": null,
                "concreteType": "GQL_WebHookRecord",
                "kind": "LinkedField",
                "name": "node",
                "plural": false,
                "selections": [
                  (v4/*: any*/),
                  {
                    "alias": null,
                    "args": null,
                    "kind": "ScalarField",
                    "name": "statusCode",
                    "storageKey": null
                  },
                  {
                    "alias": null,
                    "args": null,
                    "kind": "ScalarField",
                    "name": "timestamp",
                    "storageKey": null
                  },
                  {
                    "alias": null,
                    "args": null,
                    "kind": "ScalarField",
                    "name": "triggerType",
                    "storageKey": null
                  },
                  {
                    "alias": null,
                    "args": null,
                    "kind": "ScalarField",
                    "name": "guid",
                    "storageKey": null
                  },
                  {
                    "alias": null,
                    "args": null,
                    "kind": "ScalarField",
                    "name": "result",
                    "storageKey": null
                  },
                  {
                    "alias": null,
                    "args": null,
                    "concreteType": "GQL_WebHook",
                    "kind": "LinkedField",
                    "name": "webHook",
                    "plural": false,
                    "selections": [
                      (v4/*: any*/),
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
                      }
                    ],
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
        "args": (v3/*: any*/),
        "filters": [
          "hook_id"
        ],
        "handle": "connection",
        "key": "HooksLogsConnection_webHookRecords",
        "kind": "LinkedHandle",
        "name": "webHookRecords"
      }
    ]
  },
  "params": {
    "id": "cf33244e99070c3d23f796bdfcbb4e8b",
    "metadata": {},
    "name": "HooksLogsRefetchQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = 'dfd757f4123c3f299b15bccdac04def7';
export default node;

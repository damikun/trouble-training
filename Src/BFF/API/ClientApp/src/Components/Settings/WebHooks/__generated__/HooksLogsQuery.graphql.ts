/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash b1185fc23cf6631cfd996a90761d1210 */

import { ConcreteRequest } from "relay-runtime";

import { FragmentRefs } from "relay-runtime";
export type HooksLogsQueryVariables = {
    hookid: string;
};
export type HooksLogsQueryResponse = {
    readonly serverDateTime: string;
    readonly " $fragmentRefs": FragmentRefs<"HooksLogsFragment_webHookRecords">;
};
export type HooksLogsQuery = {
    readonly response: HooksLogsQueryResponse;
    readonly variables: HooksLogsQueryVariables;
};



/*
query HooksLogsQuery(
  $hookid: ID!
) {
  ...HooksLogsFragment_webHookRecords_36DEwC
  serverDateTime
}

fragment HooksLogsFragment_webHookRecords_36DEwC on Query {
  webHookRecords(first: 20, hook_id: $hookid) {
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
    "name": "hookid"
  }
],
v1 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "serverDateTime",
  "storageKey": null
},
v2 = {
  "kind": "Literal",
  "name": "first",
  "value": 20
},
v3 = [
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
    "name": "HooksLogsQuery",
    "selections": [
      (v1/*: any*/),
      {
        "args": [
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
    "name": "HooksLogsQuery",
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
      },
      (v1/*: any*/)
    ]
  },
  "params": {
    "id": "b1185fc23cf6631cfd996a90761d1210",
    "metadata": {},
    "name": "HooksLogsQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = 'bced0d9589c29342dc2df8f95b06a93b';
export default node;

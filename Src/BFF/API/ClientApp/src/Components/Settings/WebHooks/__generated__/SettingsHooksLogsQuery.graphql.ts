/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 926bdab27f2710d2461c43e80c8a75e2 */

import { ConcreteRequest } from "relay-runtime";
import { FragmentRefs } from "relay-runtime";
export type SettingsHooksLogsQueryVariables = {
    hookid: string;
};
export type SettingsHooksLogsQueryResponse = {
    readonly serverDateTime: string;
    readonly " $fragmentRefs": FragmentRefs<"SettingsHooksLogsFragment_webHookRecords">;
};
export type SettingsHooksLogsQuery = {
    readonly response: SettingsHooksLogsQueryResponse;
    readonly variables: SettingsHooksLogsQueryVariables;
};



/*
query SettingsHooksLogsQuery(
  $hookid: ID!
) {
  ...SettingsHooksLogsFragment_webHookRecords_36DEwC
  serverDateTime
}

fragment SettingsHooksLogsFragment_webHookRecords_36DEwC on Query {
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
        ...SettingsHooksLogsItemFragment
        __typename
      }
    }
  }
}

fragment SettingsHooksLogsItemFragment on GQL_WebHookRecord {
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
    "name": "SettingsHooksLogsQuery",
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
        "name": "SettingsHooksLogsFragment_webHookRecords"
      }
    ],
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "SettingsHooksLogsQuery",
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
        "key": "SettingsHooksLogsConnection_webHookRecords",
        "kind": "LinkedHandle",
        "name": "webHookRecords"
      },
      (v1/*: any*/)
    ]
  },
  "params": {
    "id": "926bdab27f2710d2461c43e80c8a75e2",
    "metadata": {},
    "name": "SettingsHooksLogsQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = '6a1c4dd46aa89e2333c775a6d700e30e';
export default node;

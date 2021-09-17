/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ReaderFragment } from "relay-runtime";
import { FragmentRefs } from "relay-runtime";
export type SettingsHooksLogsFragment_webHookRecords = {
    readonly webHookRecords: {
        readonly __id: string;
        readonly pageInfo: {
            readonly hasPreviousPage: boolean;
            readonly hasNextPage: boolean;
            readonly startCursor: string | null;
            readonly endCursor: string | null;
        };
        readonly edges: ReadonlyArray<{
            readonly cursor: string;
            readonly node: {
                readonly id: string;
                readonly " $fragmentRefs": FragmentRefs<"SettingsHooksLogsItemFragment">;
            } | null;
        }> | null;
    } | null;
    readonly " $refType": "SettingsHooksLogsFragment_webHookRecords";
};
export type SettingsHooksLogsFragment_webHookRecords$data = SettingsHooksLogsFragment_webHookRecords;
export type SettingsHooksLogsFragment_webHookRecords$key = {
    readonly " $data"?: SettingsHooksLogsFragment_webHookRecords$data;
    readonly " $fragmentRefs": FragmentRefs<"SettingsHooksLogsFragment_webHookRecords">;
};



const node: ReaderFragment = (function(){
var v0 = [
  "webHookRecords"
];
return {
  "argumentDefinitions": [
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
  "kind": "Fragment",
  "metadata": {
    "connection": [
      {
        "count": "first",
        "cursor": "after",
        "direction": "forward",
        "path": (v0/*: any*/)
      }
    ],
    "refetch": {
      "connection": {
        "forward": {
          "count": "first",
          "cursor": "after"
        },
        "backward": null,
        "path": (v0/*: any*/)
      },
      "fragmentPathInResult": [],
      "operation": require('./SettingsHooksLogsRefetchQuery.graphql')
    }
  },
  "name": "SettingsHooksLogsFragment_webHookRecords",
  "selections": [
    {
      "alias": "webHookRecords",
      "args": [
        {
          "kind": "Variable",
          "name": "hook_id",
          "variableName": "hookid"
        }
      ],
      "concreteType": "WebHookRecordsConnection",
      "kind": "LinkedField",
      "name": "__SettingsHooksLogsConnection_webHookRecords_connection",
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
                  "name": "__typename",
                  "storageKey": null
                },
                {
                  "args": null,
                  "kind": "FragmentSpread",
                  "name": "SettingsHooksLogsItemFragment"
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
    }
  ],
  "type": "Query",
  "abstractKey": null
};
})();
(node as any).hash = '348fb4d25c2be5fff797000831f1640e';
export default node;

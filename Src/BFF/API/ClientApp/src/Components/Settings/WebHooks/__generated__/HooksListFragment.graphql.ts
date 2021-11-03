/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ReaderFragment } from "relay-runtime";
import HooksListRefetchQuery from "./HooksListRefetchQuery.graphql";
import { FragmentRefs } from "relay-runtime";
export type HooksListFragment = {
    readonly webhooks: {
        readonly __id: string;
        readonly pageInfo: {
            readonly hasPreviousPage: boolean;
            readonly hasNextPage: boolean;
            readonly startCursor: string | null;
            readonly endCursor: string | null;
        };
        readonly edges: ReadonlyArray<{
            readonly node: {
                readonly id: string;
                readonly " $fragmentRefs": FragmentRefs<"HooksItemFragment">;
            } | null;
        }> | null;
    } | null;
    readonly " $refType": "HooksListFragment";
};
export type HooksListFragment$data = HooksListFragment;
export type HooksListFragment$key = {
    readonly " $data"?: HooksListFragment$data | undefined;
    readonly " $fragmentRefs": FragmentRefs<"HooksListFragment">;
};



const node: ReaderFragment = (function(){
var v0 = [
  "webhooks"
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
      "operation": HooksListRefetchQuery
    }
  },
  "name": "HooksListFragment",
  "selections": [
    {
      "alias": "webhooks",
      "args": null,
      "concreteType": "WebhooksConnection",
      "kind": "LinkedField",
      "name": "__HooksListConnection_webhooks_connection",
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
          "kind": "Stream",
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
                      "name": "__typename",
                      "storageKey": null
                    },
                    {
                      "kind": "Defer",
                      "selections": [
                        {
                          "args": null,
                          "kind": "FragmentSpread",
                          "name": "HooksItemFragment"
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
    }
  ],
  "type": "Query",
  "abstractKey": null
};
})();
(node as any).hash = '3110e82c2371d7327607d76ff9664771';
export default node;

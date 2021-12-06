/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ReaderFragment } from "relay-runtime";

import { FragmentRefs } from "relay-runtime";
export type HooksStreamDeferListFragment = {
    readonly webHooksTestStream: ReadonlyArray<{
        readonly id: string;
        readonly " $fragmentRefs": FragmentRefs<"ItemFragment">;
    } | null> | null;
    readonly " $refType": "HooksStreamDeferListFragment";
};
export type HooksStreamDeferListFragment$data = HooksStreamDeferListFragment;
export type HooksStreamDeferListFragment$key = {
    readonly " $data"?: HooksStreamDeferListFragment$data | undefined;
    readonly " $fragmentRefs": FragmentRefs<"HooksStreamDeferListFragment">;
};



const node: ReaderFragment = {
  "argumentDefinitions": [],
  "kind": "Fragment",
  "metadata": null,
  "name": "HooksStreamDeferListFragment",
  "selections": [
    {
      "kind": "Stream",
      "selections": [
        {
          "alias": null,
          "args": null,
          "concreteType": "GQL_WebHook",
          "kind": "LinkedField",
          "name": "webHooksTestStream",
          "plural": true,
          "selections": [
            {
              "alias": null,
              "args": null,
              "kind": "ScalarField",
              "name": "id",
              "storageKey": null
            },
            {
              "kind": "Defer",
              "selections": [
                {
                  "args": null,
                  "kind": "FragmentSpread",
                  "name": "ItemFragment"
                }
              ]
            }
          ],
          "storageKey": null
        }
      ]
    }
  ],
  "type": "Query",
  "abstractKey": null
};
(node as any).hash = 'de2fcc07fd881d6a65c778cacf1d36a6';
export default node;

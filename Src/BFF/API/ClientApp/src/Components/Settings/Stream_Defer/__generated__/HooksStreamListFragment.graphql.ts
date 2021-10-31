/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ReaderFragment } from "relay-runtime";

import { FragmentRefs } from "relay-runtime";
export type HooksStreamListFragment = {
    readonly webHooksTestStream: ReadonlyArray<{
        readonly id: string;
        readonly " $fragmentRefs": FragmentRefs<"ItemFragment">;
    } | null> | null;
    readonly " $refType": "HooksStreamListFragment";
};
export type HooksStreamListFragment$data = HooksStreamListFragment;
export type HooksStreamListFragment$key = {
    readonly " $data"?: HooksStreamListFragment$data | undefined;
    readonly " $fragmentRefs": FragmentRefs<"HooksStreamListFragment">;
};



const node: ReaderFragment = {
  "argumentDefinitions": [],
  "kind": "Fragment",
  "metadata": null,
  "name": "HooksStreamListFragment",
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
              "args": null,
              "kind": "FragmentSpread",
              "name": "ItemFragment"
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
(node as any).hash = '5d36c114dd601e3e8bcdc5c6e247164c';
export default node;

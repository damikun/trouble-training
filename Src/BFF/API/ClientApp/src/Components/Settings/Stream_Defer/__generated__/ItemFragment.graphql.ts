/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ReaderFragment } from "relay-runtime";

import { FragmentRefs } from "relay-runtime";
export type ItemFragment = {
    readonly id: string;
    readonly systemid: unknown;
    readonly webHookUrl: string | null;
    readonly isActive: boolean;
    readonly " $refType": "ItemFragment";
};
export type ItemFragment$data = ItemFragment;
export type ItemFragment$key = {
    readonly " $data"?: ItemFragment$data | undefined;
    readonly " $fragmentRefs": FragmentRefs<"ItemFragment">;
};



const node: ReaderFragment = {
  "argumentDefinitions": [],
  "kind": "Fragment",
  "metadata": null,
  "name": "ItemFragment",
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
    }
  ],
  "type": "GQL_WebHook",
  "abstractKey": null
};
(node as any).hash = '129d0f2f79f5bf9e2dc07b6b3c7acf22';
export default node;

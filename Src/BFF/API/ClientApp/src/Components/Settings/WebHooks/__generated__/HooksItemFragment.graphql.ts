/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ReaderFragment } from "relay-runtime";

import { FragmentRefs } from "relay-runtime";
export type HooksItemFragment = {
    readonly id: string;
    readonly systemid: unknown;
    readonly webHookUrl: string | null;
    readonly isActive: boolean;
    readonly " $refType": "HooksItemFragment";
};
export type HooksItemFragment$data = HooksItemFragment;
export type HooksItemFragment$key = {
    readonly " $data"?: HooksItemFragment$data | undefined;
    readonly " $fragmentRefs": FragmentRefs<"HooksItemFragment">;
};



const node: ReaderFragment = {
  "argumentDefinitions": [],
  "kind": "Fragment",
  "metadata": null,
  "name": "HooksItemFragment",
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
(node as any).hash = 'be418b7ecfe90ea03eacff016f4e1900';
export default node;

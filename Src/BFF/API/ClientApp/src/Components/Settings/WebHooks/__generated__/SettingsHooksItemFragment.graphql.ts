/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ReaderFragment } from "relay-runtime";

import { FragmentRefs } from "relay-runtime";
export type SettingsHooksItemFragment = {
    readonly id: string;
    readonly systemid: unknown;
    readonly webHookUrl: string | null;
    readonly isActive: boolean;
    readonly " $refType": "SettingsHooksItemFragment";
};
export type SettingsHooksItemFragment$data = SettingsHooksItemFragment;
export type SettingsHooksItemFragment$key = {
    readonly " $data"?: SettingsHooksItemFragment$data | undefined;
    readonly " $fragmentRefs": FragmentRefs<"SettingsHooksItemFragment">;
};



const node: ReaderFragment = {
  "argumentDefinitions": [],
  "kind": "Fragment",
  "metadata": null,
  "name": "SettingsHooksItemFragment",
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
(node as any).hash = '01c446f3587ed9fa4fcd56acf5c44817';
export default node;

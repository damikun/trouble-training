/* tslint:disable */
/* eslint-disable */
// @ts-nocheck

import { ReaderFragment } from "relay-runtime";

import { FragmentRefs } from "relay-runtime";
export type HookEventType = "FILE" | "HOOK" | "MILESTONE" | "NOTE" | "PROJECT" | "%future added value";
export type RecordResult = "DATA_QUERY_ERROR" | "HTTP_ERROR" | "OK" | "PARAMETER_ERROR" | "UNDEFINED" | "%future added value";
export type SettingsHooksLogsItemFragment = {
    readonly id: string;
    readonly statusCode: number;
    readonly timestamp: string;
    readonly triggerType: HookEventType;
    readonly guid: string | null;
    readonly result: RecordResult;
    readonly webHook: {
        readonly id: string;
        readonly systemid: unknown;
        readonly webHookUrl: string | null;
    } | null;
    readonly " $refType": "SettingsHooksLogsItemFragment";
};
export type SettingsHooksLogsItemFragment$data = SettingsHooksLogsItemFragment;
export type SettingsHooksLogsItemFragment$key = {
    readonly " $data"?: SettingsHooksLogsItemFragment$data | undefined;
    readonly " $fragmentRefs": FragmentRefs<"SettingsHooksLogsItemFragment">;
};



const node: ReaderFragment = (function(){
var v0 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "id",
  "storageKey": null
};
return {
  "argumentDefinitions": [],
  "kind": "Fragment",
  "metadata": null,
  "name": "SettingsHooksLogsItemFragment",
  "selections": [
    (v0/*: any*/),
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
        (v0/*: any*/),
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
    }
  ],
  "type": "GQL_WebHookRecord",
  "abstractKey": null
};
})();
(node as any).hash = '8ea8b80fb5b6c48feef3d4efe7594793';
export default node;

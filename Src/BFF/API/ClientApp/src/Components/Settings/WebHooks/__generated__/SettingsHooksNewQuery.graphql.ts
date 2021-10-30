/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 15bac3f2b6c81a8e084d783130e26773 */

import { ConcreteRequest } from "relay-runtime";

export type SettingsHooksNewQueryVariables = {};
export type SettingsHooksNewQueryResponse = {
    readonly webHookEventsTriggers: ReadonlyArray<string | null> | null;
};
export type SettingsHooksNewQuery = {
    readonly response: SettingsHooksNewQueryResponse;
    readonly variables: SettingsHooksNewQueryVariables;
};



/*
query SettingsHooksNewQuery {
  webHookEventsTriggers
}
*/

const node: ConcreteRequest = (function(){
var v0 = [
  {
    "alias": null,
    "args": null,
    "kind": "ScalarField",
    "name": "webHookEventsTriggers",
    "storageKey": null
  }
];
return {
  "fragment": {
    "argumentDefinitions": [],
    "kind": "Fragment",
    "metadata": null,
    "name": "SettingsHooksNewQuery",
    "selections": (v0/*: any*/),
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": [],
    "kind": "Operation",
    "name": "SettingsHooksNewQuery",
    "selections": (v0/*: any*/)
  },
  "params": {
    "id": "15bac3f2b6c81a8e084d783130e26773",
    "metadata": {},
    "name": "SettingsHooksNewQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = '3d3fd6b3aeb5807b2e5df191b43382d6';
export default node;

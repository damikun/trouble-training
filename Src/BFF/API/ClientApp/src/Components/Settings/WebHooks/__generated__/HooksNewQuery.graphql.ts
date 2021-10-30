/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 269863d7fd66919edae7766c1d84f4f9 */

import { ConcreteRequest } from "relay-runtime";

export type HooksNewQueryVariables = {};
export type HooksNewQueryResponse = {
    readonly webHookEventsTriggers: ReadonlyArray<string | null> | null;
};
export type HooksNewQuery = {
    readonly response: HooksNewQueryResponse;
    readonly variables: HooksNewQueryVariables;
};



/*
query HooksNewQuery {
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
    "name": "HooksNewQuery",
    "selections": (v0/*: any*/),
    "type": "Query",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": [],
    "kind": "Operation",
    "name": "HooksNewQuery",
    "selections": (v0/*: any*/)
  },
  "params": {
    "id": "269863d7fd66919edae7766c1d84f4f9",
    "metadata": {},
    "name": "HooksNewQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = '39e180d8e608890edabfa77145e94e9f';
export default node;

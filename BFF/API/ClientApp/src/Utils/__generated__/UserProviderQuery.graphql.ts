/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 7efb0df1701410878c28317b144766d0 */

import { ConcreteRequest } from "relay-runtime";
export type UserProviderQueryVariables = {};
export type UserProviderQueryResponse = {
    readonly me: {
        readonly guid: string | null;
        readonly firstName: string | null;
    } | null;
};
export type UserProviderQuery = {
    readonly response: UserProviderQueryResponse;
    readonly variables: UserProviderQueryVariables;
};



/*
query UserProviderQuery {
  me {
    guid
    firstName
  }
}
*/

const node: ConcreteRequest = (function(){
var v0 = [
  {
    "alias": null,
    "args": null,
    "concreteType": "GQL_User",
    "kind": "LinkedField",
    "name": "me",
    "plural": false,
    "selections": [
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
        "name": "firstName",
        "storageKey": null
      }
    ],
    "storageKey": null
  }
];
return {
  "fragment": {
    "argumentDefinitions": [],
    "kind": "Fragment",
    "metadata": null,
    "name": "UserProviderQuery",
    "selections": (v0/*: any*/),
    "type": "Querry",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": [],
    "kind": "Operation",
    "name": "UserProviderQuery",
    "selections": (v0/*: any*/)
  },
  "params": {
    "id": "7efb0df1701410878c28317b144766d0",
    "metadata": {},
    "name": "UserProviderQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = 'eca19789b409ba82958d74519d2f46b3';
export default node;

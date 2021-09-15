/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 1c86c2c1da73968a7ac920a9a077af84 */

import { ConcreteRequest } from "relay-runtime";
export type UserProviderQueryVariables = {};
export type UserProviderQueryResponse = {
    readonly me: {
        readonly id: string;
        readonly name: string | null;
    } | null;
};
export type UserProviderQuery = {
    readonly response: UserProviderQueryResponse;
    readonly variables: UserProviderQueryVariables;
};



/*
query UserProviderQuery {
  me {
    id
    name
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
        "name": "id",
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "name",
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
    "type": "Query",
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
    "id": "1c86c2c1da73968a7ac920a9a077af84",
    "metadata": {},
    "name": "UserProviderQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = '164a46198a3093985fdd717487d265c5';
export default node;

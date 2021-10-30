/* tslint:disable */
/* eslint-disable */
// @ts-nocheck
/* @relayHash 9c6f1f3926fad9be130e3bf12552447e */

import { ConcreteRequest } from "relay-runtime";

export type UserProviderQueryVariables = {};
export type UserProviderQueryResponse = {
    readonly me: {
        readonly id: string;
        readonly name: string | null;
        readonly sessionId: string;
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
    sessionId
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
      },
      {
        "alias": null,
        "args": null,
        "kind": "ScalarField",
        "name": "sessionId",
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
    "id": "9c6f1f3926fad9be130e3bf12552447e",
    "metadata": {},
    "name": "UserProviderQuery",
    "operationKind": "query",
    "text": null
  }
};
})();
(node as any).hash = 'c286cf347237f71a26a8641ef3f29069';
export default node;

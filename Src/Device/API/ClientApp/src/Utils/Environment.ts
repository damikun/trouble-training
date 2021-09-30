
import {
  Environment,
  Network,
  RecordSource,
  Store,
} from "relay-runtime";
import { BASE_SERVER_URL, GQL_ENDPOINT } from "../constants";

function fetchQuery(
  //@ts-ignore
  operation,
  //@ts-ignore
  variables,

) {

  // Otherwise, fetch data from server
  return fetch(`${BASE_SERVER_URL}/${GQL_ENDPOINT}`, {
    credentials: "include",
    method: "POST",
    mode: 'cors',
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json"
    },

    body: JSON.stringify({
      id: operation.id, // NOTE: pass md5 hash to the server
      query: operation.text, // this is now obsolete because text is null
      variables,
      operationName: operation.name,
    }),
  })
    .then((response) => {
      return response.json();
    })
    .then((json) => {
      return json;
    }).catch((ex)=>console.log(ex));
}

const STORE_ENTRIES = 250;
const STORE_CACHE_RELEASE_TIME = 3 * 5 * 1000; // 2 mins

export function createEnvironment() {
const network = Network.create(fetchQuery);

const source = new RecordSource();
const store = new Store(source, {
	gcReleaseBufferSize: STORE_ENTRIES,
	queryCacheExpirationTime: STORE_CACHE_RELEASE_TIME,
});
  
  return new Environment({
    network,
    store,
  });
}

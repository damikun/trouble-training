
import { meros } from "meros";
import {
  Environment,
  FetchFunction,
  Network,
  Observable,
  RecordSource,
  Store,
} from "relay-runtime";
import type { ExecutionPatchResult } from 'graphql';
import { BASE_SERVER_URL, GQL_ENDPOINT } from "../constants";

const fetchGraphQL: FetchFunction = (operation, variables, _cacheConfig) => {
	return Observable.create((sink) => {
		(async () => {

		const response = await fetch(`${BASE_SERVER_URL}/${GQL_ENDPOINT}`, {
        credentials: "include",
        method: "POST",
        mode: 'cors',
		body: JSON.stringify({
		id: operation.id, // NOTE: pass md5 hash to the server
		// query: operation.text, // this is now obsolete because text is null
		variables,
		// operationName: operation.name,
				}),
        headers: {
          // Accept: "application/json",
          "Content-Type": "application/json",
          'X-CSRF': '1'
        },
			});

			const parts = await meros<ExecutionPatchResult>(response);

				if (isAsyncIterable(parts)) {
					for await (const part of parts) {
						if (!part.json) {
							sink.error(new Error('Failed to parse part as json'));
							break;
						}
	
						  //@ts-ignore
						sink.next(part?.body);
					}
				} else {
	
					sink.next(await parts.json());
				}

			sink.complete();
		})();
	});
};


const STORE_ENTRIES = 250;
const STORE_CACHE_RELEASE_TIME = 3 * 5 * 1000; // 2 mins

export function createEnvironment() {
const network = Network.create(fetchGraphQL);

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

function isAsyncIterable(input: unknown): input is AsyncIterable<unknown> {
	return (
		typeof input === 'object' &&
		input !== null &&
		((input as any)[Symbol.toStringTag] === 'AsyncGenerator' ||
			Symbol.asyncIterator in input)
	);
}
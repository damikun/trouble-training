import clsx from "clsx";
import {  Suspense } from "react";
import { useFragment, useLazyLoadQuery } from "react-relay/hooks";
import { graphql } from "babel-plugin-relay/macro";
import { HooksStreamDeferListFragment$key } from "./__generated__/HooksStreamDeferListFragment.graphql";
import {  HooksStreamDeferQuery } from "./__generated__/HooksStreamDeferQuery.graphql";
import Item from "./Item";

///////////////////////////////////////////////////////////////
// !!!! THIS IS ONLY TEST CLASS FOR @Defer and @Stream !!!!
///////////////////////////////////////////////////////////////

const HooksStreamDeferQueryTag = graphql`
  query HooksStreamDeferQuery {
    ...HooksStreamDeferListFragment
  }
`;

export default function HooksStreamDefer() {

  const data = useLazyLoadQuery<HooksStreamDeferQuery>(
    HooksStreamDeferQueryTag,
    { },
    // For test reasons we use network-only to rerender on each route change
    { fetchPolicy: "store-and-network",UNSTABLE_renderPolicy:"partial"},
  );

  return (
    <div
      className={clsx(
        "flex w-full h-full rounded-b-md max-h-full overflow-hidden",
        "relative max-w-full overflow-y-scroll scrollbarwidth",
        "scrollbarhide scrollbarhide2"
      )}
    >
      <div className="absolute w-full align-middle h-full">
        <div className="h-full relative max-w-full flex-col">
          <div className="flex flex-col max-w-3xl space-y-2 h-full">
            <div className="flex justify-between flex-nowrap space-x-2">
              <div className="flex font-bold text-gray-800 text-md text-lg px-1">
                @Stream+@Defer
              </div>
            </div>

            <div className="flex h-full">
              <HooksStreamDeferList dataRef={data} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

///////////////////////////////////////////////
///////////////////////////////////////////////

export const HooksStreamDeferListFragment = graphql`
  fragment HooksStreamDeferListFragment on Query 
 {
  webHooksTestStream @stream(initial_count: 2) {
      id
      ...ItemFragment @defer
    }
  }
`;

type HooksStreamDeferListProps = {
  dataRef: HooksStreamDeferListFragment$key | null;
};

function HooksStreamDeferList({ dataRef }: HooksStreamDeferListProps) {
  
  const entity = useFragment(HooksStreamDeferListFragment, dataRef);

  return (
    <div className="flex flex-col divide-y w-full">
      {entity?.webHooksTestStream?.map((entity) => {
        return entity ? (
          <Suspense fallback={null}>
            <Item key={entity?.id} dataRef={entity} />
          </Suspense>
        ) : (
          <></>
        );
      })}
    </div>
  );
}
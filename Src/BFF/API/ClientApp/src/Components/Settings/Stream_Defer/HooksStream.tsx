import clsx from "clsx";
import {  Suspense } from "react";
import { useFragment, useLazyLoadQuery } from "react-relay/hooks";
import { graphql } from "babel-plugin-relay/macro";
import { HooksStreamListFragment$key } from "./__generated__/HooksStreamListFragment.graphql";
import {  HooksStreamQuery } from "./__generated__/HooksStreamQuery.graphql";
import Item from "./Item";

///////////////////////////////////////////////////////////////
// !!!! THIS IS ONLY TEST CLASS FOR @Defer and @Stream !!!!
///////////////////////////////////////////////////////////////

const HooksStreamQueryTag = graphql`
  query HooksStreamQuery {
    ...HooksStreamListFragment
  }
`;

export default function HooksStream() {

  const data = useLazyLoadQuery<HooksStreamQuery>(
    HooksStreamQueryTag,
    { },
    // For test reasons we use network-only to rerender on each route change
    { fetchPolicy: "network-only",UNSTABLE_renderPolicy:"partial", fetchKey:"Key1"},
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
                @Stream 
              </div>
            </div>

            <div className="flex h-full">
              <HooksStreamList dataRef={data} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

///////////////////////////////////////////////
///////////////////////////////////////////////

export const HooksStreamListFragment = graphql`
  fragment HooksStreamListFragment on Query 
 {
  webHooksTestStream @stream(initialCount: 2) {
      id
      ...ItemFragment
    }
  }
`;

type HooksStreamListProps = {
  dataRef: HooksStreamListFragment$key | null;
};

function HooksStreamList({ dataRef }: HooksStreamListProps) {
  
  const entity = useFragment(HooksStreamListFragment, dataRef);

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
import clsx from "clsx";
import { useTransition, useCallback, useEffect, Suspense } from "react";
import { useFragment, useLazyLoadQuery, usePaginationFragment } from "react-relay/hooks";
import { graphql } from "babel-plugin-relay/macro";
import { HooksQuery } from "./__generated__/HooksQuery.graphql";
import StayledButton from "../../../UIComponents/Buttons/StayledButton";
import { useNavigate } from "react-router";
import { useHooksContext } from "../Settings";
import { HooksListFragment$key } from "./__generated__/HooksListFragment.graphql";
import { HooksListRefetchQuery } from "./__generated__/HooksListRefetchQuery.graphql";
import HooksItem from "./HooksItem";

const HooksQueryTag = graphql`
  query HooksQuery {
    ...HooksListFragment
  }
`;

export default Hooks

function Hooks() {

  const data = useLazyLoadQuery<HooksQuery>(
    HooksQueryTag,
    { },
    { fetchPolicy: "store-or-network",UNSTABLE_renderPolicy:"partial"},
  );

  const navigate = useNavigate();

  const [isInFlight, startTransition] = useTransition();

  const hanldeCreateNewNavigate = useCallback(() => {
    !isInFlight &&
      startTransition(() => {
        navigate(`New`);
      });
  }, [startTransition, navigate,isInFlight]);

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
                WebHook
              </div>

              <StayledButton
                variant="secondaryblue"
                size="normal"
                onClick={hanldeCreateNewNavigate}
              >
                Create new
              </StayledButton>
            </div>

            <p className="flex prose-sm px-1 w-full">
              WebHooks let external services to be notified when certain events
              happen. It send`s the POST request to list of specified URLs with
              custom body content.
            </p>

            <div className="flex h-full">
              <HooksList dataRef={data} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

///////////////////////////////////////////////
///////////////////////////////////////////////

export const HooksListFragment = graphql`
  fragment HooksListFragment on Query 
  @argumentDefinitions(
    first: { type: Int }
    after: { type: String }
  ) @refetchable(queryName: "HooksListRefetchQuery") {
    webhooks(
      first: $first
      after: $after
    ) @connection(key: "HooksListConnection_webhooks"){
      __id
      pageInfo {
        hasPreviousPage
        hasNextPage
        startCursor
        endCursor
      }
      edges @stream(initialCount:2){
        node{
          id
          ...HooksItemFragment@defer
        }
      }
    }
  }
`;

type HooksListProps = {
  dataRef: HooksListFragment$key | null;
};

function HooksList({ dataRef }: HooksListProps) {
  
  const entity = useFragment(HooksListFragment, dataRef);

  const pagination = usePaginationFragment<
  HooksListRefetchQuery,
  HooksListFragment$key
>(HooksListFragment, dataRef);

  const hooksCtx = useHooksContext();

  useEffect(() => {
    hooksCtx?.setConnectionId(
      pagination.data?.webhooks?.__id
        ? pagination.data?.webhooks?.__id
        : ""
    );
  }, [pagination, pagination.data, pagination.data?.webhooks?.__id]);

  return (
    <div className="flex flex-col divide-y w-full">
      {entity?.webhooks?.edges?.map((entity) => {
        return entity ? (
          <Suspense fallback={null}>
            <HooksItem key={entity?.node?.id} dataRef={entity.node} />
          </Suspense>
        ) : (
          <></>
        );
      })}
    </div>
  );
}
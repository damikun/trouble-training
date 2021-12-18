import clsx from "clsx";
import React, { useCallback, useMemo } from "react";
import {
  useFragment,
  useLazyLoadQuery,
  usePaginationFragment,
} from "react-relay/hooks";
import { useParams, useSearchParams } from "react-router-dom";
import { graphql } from "babel-plugin-relay/macro";
import { HooksLogsQuery } from "./__generated__/HooksLogsQuery.graphql";
import StayledInfinityScrollContainer from "../../../UIComponents/ScrollContainter/StayledInfinityScrollContainer";
import { HooksLogsRefetchQuery } from "./__generated__/HooksLogsRefetchQuery.graphql";
import { HooksLogsFragment_webHookRecords$key } from "./__generated__/HooksLogsFragment_webHookRecords.graphql";
import {
  HooksLogsItemFragment$key,
  RecordResult,
} from "./__generated__/HooksLogsItemFragment.graphql";
import ActivityTimeStamp from "../../../UIComponents/Timestamp/ActivityTimeStamp";
import Badge from "../../../UIComponents/Badged/Badge";
import Modal from "../../../UIComponents/Modal/Modal";
import HooksLogsDetail from "./HooksLogsDetail";
import AssignedDetailPlaceholder from "../../../UIComponents/DetailPlaceholder";

const HooksLogsQueryTag = graphql`
  query HooksLogsQuery($hookid: ID!) {
    ...HooksLogsFragment_webHookRecords
      @arguments(first: 20, after: null, hookid: $hookid)

    serverDateTime
  }
`;

export const HooksLogsFragment = graphql`
  fragment HooksLogsFragment_webHookRecords on Query
  @argumentDefinitions(
    first: { type: Int }
    after: { type: String }
    hookid: { type: "ID!" }
  )
  @refetchable(queryName: "HooksLogsRefetchQuery") {
    webHookRecords(first: $first, after: $after, hook_id: $hookid)
      @connection(key: "HooksLogsConnection_webHookRecords") {
      __id
      pageInfo {
        hasPreviousPage
        hasNextPage
        startCursor
        endCursor
      }
      edges {
        cursor
        node {
          id
          ...HooksLogsItemFragment
        }
      }
    }
  }
`;

export default React.memo(HooksLogs);

function HooksLogs() {
  const { hookid }: any = useParams();

  const data = useLazyLoadQuery<HooksLogsQuery>(
    HooksLogsQueryTag,
    {
      hookid: hookid,
    },
    {
      fetchPolicy: "store-and-network",
    }
  );

  const pagination = usePaginationFragment<
    HooksLogsRefetchQuery,
    HooksLogsFragment_webHookRecords$key
  >(HooksLogsFragment, data);

  function HandleScrollEnd() {
    pagination.hasNext && !pagination.isLoadingNext && pagination.loadNext(20);
  }

  const isEmpty = pagination.data.webHookRecords?.edges
    ? pagination.data.webHookRecords?.edges.length <= 0
    : true;

  const [searchParams, setSearchParams] = useSearchParams();

  const activity_id = searchParams.get("hook_id");

  const handleModalClose = useCallback(() => {

    searchParams.delete("hook_id");
    setSearchParams(searchParams);
  }, [searchParams, setSearchParams]);

  const handleItemDetail = useCallback(
    (hook_id: string | null | undefined) => {
      searchParams.delete("hook_id");
      if (hook_id) {
        searchParams.append("hook_id", hook_id);
      }
      setSearchParams(searchParams);
    },
    [searchParams, setSearchParams]
  );

  return (
    <>
      <Modal
        position="top"
        isOpen={activity_id !== null}
        OnClose={handleModalClose}
        component={
          <HooksLogsDetail onClose={handleModalClose} />
        }
        fallback={<AssignedDetailPlaceholder />}
      />
      <div
        className={clsx(
          "flex w-full h-full rounded-b-md max-h-full overflow-hidden",
          "relative max-w-full overflow-y-scroll scrollbarwidth",
          "scrollbarhide scrollbarhide2"
        )}
      >
        <div className="absolute h-full w-full align-middle">
          <div className="h-full relative max-w-full flex-col">
            <div className="flex h-full flex-col max-w-3xl space-y-2">
              <Header />

              <StayledInfinityScrollContainer
                isLoadingMore={pagination.isLoadingNext}
                isLoading={false}
                bgcolor="bg-transparent"
                onEnd={HandleScrollEnd}
                // className="bg-white border shadow-sm"
                isEmptyMessage={"No HookRecords available"}
                isEmpty={isEmpty}
                divide
              >
                {pagination?.data?.webHookRecords?.edges?.map((entity) => {
                  return entity.node ? (
                    <ProjectSettingsHooksItem
                      key={entity.node.id}
                      dataRef={entity.node}
                      serverDate={data.serverDateTime}
                      onDetail={handleItemDetail}
                    />
                  ) : (
                    <></>
                  );
                })}
              </StayledInfinityScrollContainer>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

///////////////////////////////////////////
///////////////////////////////////////////

function Header() {
  return (
    <div className="flex justify-between flex-nowrap space-x-2">
      <div className="flex font-bold text-gray-800 text-md text-lg px-1 h-8">
        WebHook History
      </div>
    </div>
  );
}

///////////////////////////////////////////
///////////////////////////////////////////

export const HooksLogsItemFragment = graphql`
  fragment HooksLogsItemFragment on GQL_WebHookRecord {
    id
    statusCode
    timestamp
    triggerType
    guid
    result
    webHook {
      id
      systemid
      webHookUrl
    }
  }
`;

type ProjectSettingsHooksItemProps = {
  dataRef: HooksLogsItemFragment$key | null;
  onDetail?: (id: string) => void;
  serverDate?: string | null | undefined;
};

function ProjectSettingsHooksItem({
  dataRef,
  onDetail,
  serverDate,
}: ProjectSettingsHooksItemProps) {
  const entity = useFragment(HooksLogsItemFragment, dataRef);

  const ActivityMemorisedTimestamp = useMemo(
    () => (entity?.timestamp ? new Date(entity.timestamp) : undefined),
    [entity]
  );

  const ServerMemorisedTimestamp = useMemo(
    () => (serverDate ? new Date(serverDate) : new Date()),
    [serverDate, entity]
  );

  const handleDetail = useCallback(
    (e: any) => {
      onDetail && entity?.id && onDetail(entity?.id);
    },
    [onDetail, entity]
  );

  return (
    <div
      className={clsx(
        "flex p-3 md:px-5 first:rounded-t-md last:rounded-b-md",
        "items-center space-x-10 justify-between bg-white",
        "cursor-pointer hover:bg-gray-50"
      )}
      onClick={handleDetail}
    >
      <div className="flex space-x-2 w-full items-center">
        <div className="flex w-2/12 justify-center">
          <StateSection state={entity?.result} />
        </div>

        <div className="flex flex-col w-4/12 justify-center capitalize">
          <HookTriggerType trigger={entity?.triggerType} />

          <div className="flex text-xs font-semibold text-gray-500">
            {entity?.guid}
          </div>
        </div>

        <div className="flex-1 flex space-x-2 items-center justify-end">
          <div className="flex space-x-2 items-center">
            <div className="flex max-w-16 justify-end">
              <ActivityTimeStamp
                date={ActivityMemorisedTimestamp}
                currentDT={ServerMemorisedTimestamp}
                recentLimit={1}
              />
            </div>
            <div className="flex w-16 justify-center">
              <StatusCodeSection status={entity?.statusCode} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

///////////////////////////////////////////
///////////////////////////////////////////

type HookTriggerTypeProps = {
  trigger: string | null | undefined;
};

function HookTriggerType({ trigger }: HookTriggerTypeProps) {
  return (
    <div
      className={clsx(
        "flex py-0.5 truncate-1-lines",
        "break-all rounded-md font-semibold",
        "whitespace-pre"
      )}
    >
      {trigger}
    </div>
  );
}

///////////////////////////////////////////
///////////////////////////////////////////

type StatusCodeSectionProps = {
  status: number | null | undefined;
};

function StatusCodeSection({ status }: StatusCodeSectionProps) {
  const status_style = useMemo(() => {
    if (status) {
      if (status >= 200 && status <= 299) {
        return "text-white bg-green-400";
      } else {
        return "text-white bg-red-400";
      }
    } else {
      return "bg-gray-200 text-gray-700";
    }
  }, []);

  if (!status) {
    return <></>;
  }

  return (
    <div
      className={clsx(
        "px-1.5 py-0.5 flex leading-none truncate-1-lines",
        "break-all rounded-md font-semibold text-xs w-10 text-center justify-center",
        status_style
      )}
    >
      {status}
    </div>
  );
}

///////////////////////////////////////////
///////////////////////////////////////////

type StateSectionProps = {
  state: RecordResult | null | undefined;
};

function StateSection({ state }: StateSectionProps) {
  if (!state) {
    return <></>;
  }

  const varinat = state === "OK" ? "secondarygreen" : "secondaryellow";

  return (
    <div className="max-w-16">
      <Badge
        turncate
        border={false}
        className="text-xxs"
        size="thin"
        variant={varinat}
      >
        {state}
      </Badge>
    </div>
  );
}

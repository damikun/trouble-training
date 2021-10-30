import clsx from "clsx";
import { useTransition, useCallback, useEffect } from "react";
import { useFragment, useLazyLoadQuery, useMutation, usePaginationFragment } from "react-relay/hooks";
import { graphql } from "babel-plugin-relay/macro";
import { SettingsHooksQuery } from "./__generated__/SettingsHooksQuery.graphql";
import StayledButton from "../../../UIComponents/Buttons/StayledButton";
import { useNavigate } from "react-router";
import { useHooksContext } from "../Settings";
import { SettingsHooksListFragment$key } from "./__generated__/SettingsHooksListFragment.graphql";
import { SettingsHooksItemFragment$key } from "./__generated__/SettingsHooksItemFragment.graphql";
import { SettingsHooksRemoveMutation } from "./__generated__/SettingsHooksRemoveMutation.graphql";
import { useToast } from "../../../UIComponents/Toast/ToastProvider";
import { HandleErrors } from "../../../Utils/ErrorHelper";
import StayledPromtButton from "../../../UIComponents/Buttons/StayledPromtButton";
import { faFileMedicalAlt, faPen } from "@fortawesome/free-solid-svg-icons";
import { ReactComponent as DeleteImage } from "../../../Images/remove.svg";
import { SettingsHooksListRefetchQuery } from "./__generated__/SettingsHooksListRefetchQuery.graphql";

const SettingsHooksQueryTag = graphql`
  query SettingsHooksQuery {
    ...SettingsHooksListFragment
  }
`;

export default SettingsHooks

function SettingsHooks() {

  const data = useLazyLoadQuery<SettingsHooksQuery>(
    SettingsHooksQueryTag,
    { },
    { fetchPolicy: "store-or-network"}
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
              <SettingsHooksList dataRef={data} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

///////////////////////////////////////////////
///////////////////////////////////////////////

export const SettingsHooksListFragment = graphql`
  fragment SettingsHooksListFragment on Query 
  @argumentDefinitions(
    first: { type: Int }
    after: { type: String }
  ) @refetchable(queryName: "SettingsHooksListRefetchQuery") {
    webhooks(
      first: $first
      after: $after
    ) @connection(key: "SettingsHooksListConnection_webhooks"){
      __id
      pageInfo {
        hasPreviousPage
        hasNextPage
        startCursor
        endCursor
      }
      edges @stream(initialCount:1){
        node{
          id
          ...SettingsHooksItemFragment
        }
      }
    }
  }
`;

type SettingsHooksListProps = {
  dataRef: SettingsHooksListFragment$key | null;
};

function SettingsHooksList({ dataRef }: SettingsHooksListProps) {
  
  const entity = useFragment(SettingsHooksListFragment, dataRef);

  const pagination = usePaginationFragment<
  SettingsHooksListRefetchQuery,
  SettingsHooksListFragment$key
>(SettingsHooksListFragment, dataRef);

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
          <SettingsHooksItem key={entity?.node?.id} dataRef={entity.node} />
        ) : (
          <></>
        );
      })}
    </div>
  );
}

///////////////////////////////////////////////
///////////////////////////////////////////////

export const SettingsHooksItemFragment = graphql`
  fragment SettingsHooksItemFragment on GQL_WebHook {
    id
    systemid
    webHookUrl
    isActive
  }
`;

type SettingsHooksItemProps = {
  dataRef: SettingsHooksItemFragment$key | null;
};

function SettingsHooksItem({ dataRef }: SettingsHooksItemProps) {
  const entity = useFragment(SettingsHooksItemFragment, dataRef);

  const navigate = useNavigate();

  const [
    commitRemove,
    removeInFlight,
  ] = useMutation<SettingsHooksRemoveMutation>(graphql`
    mutation SettingsHooksRemoveMutation(
      $request: RemoveWebHookInput
      $connections: [ID!]!
    ) {
      removeWebHook(request: $request) {
        ... on RemoveWebHookPayload {
          errors {
            ... on IBaseError {
              message
            }
          }

          removed_id @deleteEdge(connections: $connections)

        }
      }
    }
  `);

  const toast = useToast();

  const HooksCtx = useHooksContext();

  const handleRemove = useCallback(() => {
    entity &&
      !removeInFlight &&
      (entity.systemid as number) > 0 &&
      commitRemove({
        variables: {
          request: {
            webHookId: entity.systemid as number,
          },
          connections: HooksCtx?.connection_id ? [HooksCtx.connection_id] : [],
        },

        onError(error) {
          toast?.pushError("Failed to process mutation");
          console.log(error);
        },

        onCompleted(data) {},
        updater(store, data) {
          HandleErrors(toast, data.removeWebHook?.errors);
        },
      });
  }, [toast, commitRemove, entity,removeInFlight,HooksCtx?.connection_id ]);

  const [_,startTransition] = useTransition();

  const handleEdit = useCallback(() => {
    startTransition(() => {
      navigate(`/Hooks/Edit/${entity?.id}`);
    });
  }, []);

  const handleHooksLogs = useCallback(() => {
    startTransition(() => {
      navigate(`/Hooks/Logs/${entity?.id}`);
    });
  }, []);

  return (
    <div
      className={clsx(
        "flex p-3 md:px-5 justify-between bg-white first:rounded-t-md last:rounded-b-md",
        "items-center space-x-10 hover:bg-gray"
      )}
    >
      <div className="font- truncate-1-lines break-all">
        {entity?.webHookUrl}
      </div>
      <div className="flex space-x-2">
        <div className="flex justify-center">
          <StayledButton
            onClick={handleHooksLogs}
            iconLeft={faFileMedicalAlt}
            disabled={removeInFlight}
            size="normal"
            variant="primarygray"
          >
            Logs
          </StayledButton>
        </div>
        <div className="flex  justify-center">
          <StayledButton
            onClick={handleEdit}
            iconLeft={faPen}
            disabled={removeInFlight}
            size="normal"
            variant="primarygray"
          >
            Edit
          </StayledButton>
        </div>
        <div className="flex w-20 justify-center">
          <StayledPromtButton
            promt_className="md:max-w-lg"
            promt_buttonTitle={"Delete"}
            promt_isProcessing={removeInFlight}
            promt_variant="error"
            promt_minWidth="min-w-24"
            promt_position="justify-start"
            promt_callback={handleRemove}
            promt_title="Delete WebHook"
            button_size="normal"
            className="hover:text-red-500 text-gray-400 duration-300"
            promt_content={
              <HookDeleteModalContent hookUri={entity?.webHookUrl} />
            }
            variant="primaryred"
          >
            Delete
          </StayledPromtButton>
        </div>
      </div>
    </div>
  );
}

///////////////////////////////////
///////////////////////////////////

type HookDeleteModalContentProps = {
  hookUri?: string | null | undefined;
};

function HookDeleteModalContent({ hookUri }: HookDeleteModalContentProps) {
  return (
    <div className="flex flex-col w-full max-w-full space-y-5 mt-5 items-center justify-center">
      <div className="flex justify-center">
        <DeleteImage className="h-32" />
      </div>

      {hookUri && (
        <div className="truncate-1-lines break-all italic text-base">
          {hookUri}
        </div>
      )}

      <div className="flex justify-center space-x-2 text-gray-500">
        <p className="max-w-md text-center text-base font-normal">
          You are atempting to delete Webhook from project. Are you shure about
          this action?
        </p>
      </div>
    </div>
  );
}

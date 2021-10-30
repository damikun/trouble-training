import clsx from "clsx";
import { useTransition, useCallback} from "react";
import { useFragment, useMutation } from "react-relay/hooks";
import { graphql } from "babel-plugin-relay/macro";
import StayledButton from "../../../UIComponents/Buttons/StayledButton";
import { useNavigate } from "react-router";
import { useHooksContext } from "../Settings";
import { HooksItemFragment$key } from "./__generated__/HooksItemFragment.graphql";
import { HooksItemRemoveMutation } from "./__generated__/HooksItemRemoveMutation.graphql";
import { useToast } from "../../../UIComponents/Toast/ToastProvider";
import { HandleErrors } from "../../../Utils/ErrorHelper";
import StayledPromtButton from "../../../UIComponents/Buttons/StayledPromtButton";
import { faFileMedicalAlt, faPen } from "@fortawesome/free-solid-svg-icons";
import { ReactComponent as DeleteImage } from "../../../Images/remove.svg";

export const HooksItemFragment = graphql`
  fragment HooksItemFragment on GQL_WebHook {
    id
    systemid
    webHookUrl
    isActive
  }
`;

type HooksItemProps = {
  dataRef: HooksItemFragment$key | null;
};

export default function HooksItem({ dataRef }: HooksItemProps) {
  const entity = useFragment(HooksItemFragment, dataRef);

  const navigate = useNavigate();

  const [
    commitRemove,
    removeInFlight,
  ] = useMutation<HooksItemRemoveMutation>(graphql`
    mutation HooksItemRemoveMutation(
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

  //@ts-ignore
  const [_,startTransition] = useTransition({
      busyDelayMs: 2000,
    });

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

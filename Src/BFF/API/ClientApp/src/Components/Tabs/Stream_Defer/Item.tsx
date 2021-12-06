import clsx from "clsx";
import { useTransition, useCallback} from "react";
import { useFragment } from "react-relay/hooks";
import { graphql } from "babel-plugin-relay/macro";
import StayledButton from "../../../UIComponents/Buttons/StayledButton";
import { useNavigate } from "react-router";
import { ItemFragment$key } from "./__generated__/ItemFragment.graphql";
import { faFileMedicalAlt, faPen } from "@fortawesome/free-solid-svg-icons";


///////////////////////////////////////////////////////////////
// !!!! THIS IS ONLY TEST CLASS FOR @Defer and @Stream !!!!
///////////////////////////////////////////////////////////////

export const ItemFragment = graphql`
  fragment ItemFragment on GQL_WebHook {
    id
    systemid
    webHookUrl
    isActive
  }
`;

type ItemProps = {
  dataRef: ItemFragment$key | null;
};

export default function Item({ dataRef }: ItemProps) {
  const entity = useFragment(ItemFragment, dataRef);

  const navigate = useNavigate();

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
        "flex p-3 md:px-5 justify-between bg-white",
        "first:rounded-t-md last:rounded-b-md",
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
            size="normal"
            variant="primarygray"
          >
            Edit
          </StayledButton>
        </div>
      </div>
    </div>
  );
}

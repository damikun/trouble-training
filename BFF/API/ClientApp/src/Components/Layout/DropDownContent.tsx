import { unstable_useTransition, useCallback } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import NameInitials from "../../Utils/NameInitials";
import { faSignOutAlt } from "@fortawesome/free-solid-svg-icons";
import { usertype } from "../../Utils/UserProvider";
import { useNavigate } from "react-router-dom";
import clsx from "clsx";
import Avatar from "../../UIComponents/Avatar/Avatar";
import React from "react";
import { IconProp } from "@fortawesome/fontawesome-svg-core";

type RenderDropDownContentProps = {
  user: usertype;
};

export default React.memo(RenderUserDropDownContent);

function RenderUserDropDownContent({ user }: RenderDropDownContentProps) {
  return (
    <div className="flex flex-col py-2 items-center w-48 text-gray-800 ">
      <AvatarSection user={user} />

      <div className="w-full">
        <hr className="mt-2 whitespace-pre-line" />
      </div>
      <div className="flex flex-col w-full pt-2">
        <MenuItem name={"Logout"} icon={faSignOutAlt} to={"/logout"} />
      </div>
    </div>
  );
}

/////////////////////////////////////////
/////////////////////////////////////////

type AvatarSectionProps = {
  user: usertype;
};

function AvatarSection({ user }: AvatarSectionProps) {

  return (
    <div className="flex flex-col relative p-2 text-base space-y-3">
      <div
        className={clsx(
          "flex flex-col uppercase mx-auto",
          "text-white select-none my-auto"
        )}
      >
        <Avatar
          size="w-16 h-16"
          border={false}
          src={undefined}
          label={NameInitials(user?.me?.name)}
          // info={user?.me?.role}

        />
      </div>
      <div className="flex  py-1 font-bold text-normal px-2 capitalize">
        {user?.me?.name
          ? `${user?.me?.name}`
          : "Unknown"}
      </div>
    </div>
  );
}

/////////////////////////////////////////
/////////////////////////////////////////

type MenuItemProps = {
  icon: IconProp;
  to: string;
  name: string;
};

function MenuItem({ icon, name, to }: MenuItemProps) {
  const history = useNavigate();

  const [startTransition] = unstable_useTransition({
    busyDelayMs: 10000,
  });

  const handleNavigate = useCallback(() => {
    startTransition(() => {
      history(to);
    });
  }, [to]);

  return (
    <div
      className={clsx(
        "flex flex-row justify-between w-full py-1",
        "hover:bg-gray-100 cursor-pointer px-2 h-10",
        "text-gray-700"
      )}
      onClick={handleNavigate}
    >
      <div className="flex items-center tracking-wide font-semibold">
        {name}
      </div>
      <div className="flex items-center w-8">
        <FontAwesomeIcon className="mx-auto" icon={icon} />
      </div>
    </div>
  );
}

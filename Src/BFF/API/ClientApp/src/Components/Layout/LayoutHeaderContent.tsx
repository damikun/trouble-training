import DropDownContent from "./DropDownContent";
import NameInitials from "../../Utils/NameInitials";

import {faChevronCircleDown,
} from "@fortawesome/free-solid-svg-icons";
import { useUserStore } from "../../Utils/UserProvider";
import Avatar from "../../UIComponents/Avatar/Avatar";
import StayledDropDown from "../../UIComponents/DropDownMenu/StayledDropDown";
import { useMemo } from "react";
// import { ReactComponent as Logo } from "../../../Images/logo.svg";
import clsx from "clsx";


export type CreateNewModalType = {
  visible: boolean;
  item_gqlid: string;
};

export default function LayoutHeaderMd() {
  const userStore = useUserStore();

  const Selected = useMemo(() => {
    return {
      value: "User",
      content: (
        <Avatar
          border={false}
          className="ring-0 m-0.5 ring-white"
          size="w-6 h-6"
          showStatus={false}
          src={undefined}
          label={NameInitials(
            userStore?.user?.me?.name
          )}
        />
      ),
    };
  }, [userStore]);


  return (
    <>
      <div className="bg-transparent px-3 sm:px-5 lg:px-10 text-white">
        <div className="relative flex items-center justify-between h-14">
          {userStore?.user?.me && (
            <>
              <div className="flex flex-row font-bold ">
                <div
                  className={clsx(
                    "flex justify-center items-center w-24",
                    "fill-current text-center cursor-pointer"
                  )}
                >
                  {/* <Logo className="fill-current filter-invert w-16" /> */}
                </div>

              </div>
              <div className="ml-2 flex items-center w-full md:ml-6 justify-end">

                <div className="flex relative flex-col pl-2">
                  <StayledDropDown
                    id="__UserMenuDropdown"
                    iconPosition="left"
                    deletable={false}
                    enabled
                    icon={faChevronCircleDown}
                    padding={false}
                    rounding="full"
                    variant="lightgray"
                    position="bottomright"
                    orientation="downleft"
                    isError={false}
                    selected={Selected}
                    init="Menu"
                  >
                    <DropDownContent user={userStore.user} />
                  </StayledDropDown>
                </div>
              </div>
            </>
          )}
        </div>
      </div>
    </>
  );
}

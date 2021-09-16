import clsx from "clsx";
import { useUserStore } from "../../../Utils/UserProvider";
import { ReactComponent as WelcomeSVG } from "../../../Images/welcome.svg";

export default function Welcome() {
  const store = useUserStore()

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
          <div className="flex relative flex-col max-w-3xl space-y-2 h-full">
            <div className="flex justify-between flex-nowrap space-x-2">
              <div className="flex font-bold text-gray-800 text-md text-lg px-1">
                Hi there <p className="text-blue-500 px-2 select-none">{store?.user?.me?.name}</p>
              </div>
            </div>

            <p className="flex prose-sm px-1 w-full">
              This is FullStack app workshop result
            </p>

            <div className="flex relative h-full ">
              <WelcomeSVG className="max-w-full fill max-h-80 " />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

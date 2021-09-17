import clsx from "clsx";
import React from "react";

export default function DetailPlaceholder() {
  return (
    <div className="flex w-full min-w-18rem md:w-md lg:w-lg text-xs md:text-sm flex-col select-text my-auto rounded-md shadow-lg py-5 flex-grow px-5 bg-white items-center">
      <div className="flex flex-col w-full">
        <div className="flex flex-row font-semibold flex-no-wrap text-nowrap justify-between align-content-center">
          <div className="my-auto align-content-center select-none">
            <TextPlaceholder className="w-12" />
          </div>
          <TextPlaceholder className="w-4" />
        </div>

        <div className="flex-1 flex-grow mt-2 ">
          <div className="flex flex-col w-full">
            <div className="grid md:grid-cols-6 grid-flow-row md:grid-flow-col gap-0 md:gap-10 w-full max-w-full">
              <div className="flex col-span-4 w-full overflow-hidden">
                <LeftSectionPlaceholder />
              </div>
              <div className="col-span-2 overflow-hidden">
                <div className="flex">
                  <RightSectionPlaceholder />
                </div>
              </div>
            </div>
            <div className="grid md:grid-cols-6 grid-flow-row md:grid-flow-col gap-0 md:gap-10 w-full max-w-full">
              <div className="flex col-span-4">
                <div className="w-full">
                  <LeftSectionPlaceholder />

                  <DescriptionPlaceholder />
                </div>
              </div>
              <div className="col-span-2 whitespace-pre">
                <div className="">
                  <RightSectionPlaceholder />

                  <LeftSectionPlaceholder />

                  <LeftSectionPlaceholder />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

type TextPlaceholderProps = {
  className?: string;
};

function TextPlaceholder({ className }: TextPlaceholderProps) {
  return (
    <div className={"flex py-1 "}>
      <div
        className={clsx(
          "flex whitespace-pre h-3 bg-gray-300 rounded-sm",
          className ? className : "w-10"
        )}
      ></div>
    </div>
  );
}

function RightSectionPlaceholder() {
  return (
    <div className="flex flex-col align-top break-normal w-full py-2">
      <div className="flex pb-2 text-gray-700 font-extrabold text-sm  uppercase tracking-wide ">
        <div className="flex font-semibold">
          <TextPlaceholder className="w-10" />
        </div>
      </div>
      <div className="flex-1 w-full max-w-full ">
        <TextPlaceholder className="w-20 h-5" />
      </div>
    </div>
  );
}

function LeftSectionPlaceholder() {
  return (
    <div className="flex flex-col align-top break-normal w-full py-2">
      <div className="flex pb-2 text-gray-700 font-extrabold text-sm  uppercase tracking-wide ">
        <div className="flex font-semibold">
          <TextPlaceholder className="w-10" />
        </div>
      </div>
      <div className="flex-1 w-full max-w-full">
        <TextPlaceholder className="w-20" />
      </div>
    </div>
  );
}

function DescriptionPlaceholder() {
  return (
    <div className="flex flex-col align-top break-normal w-full py-2">
      <div className="flex pb-2 text-gray-700 font-extrabold text-sm  uppercase tracking-wide ">
        <div className="flex font-semibold">
          <TextPlaceholder className="w-10" />
        </div>
      </div>
      <div className="flex-1 w-full max-w-full">
        <TextPlaceholder className="w-24" />
      </div>
      <div className="flex-1 w-full max-w-full">
        <TextPlaceholder className="w-32" />
      </div>
      <div className="flex-1 w-full max-w-full">
        <TextPlaceholder className="w-40" />
      </div>
      <div className="flex-1 w-full max-w-full">
        <TextPlaceholder className="w-full" />
      </div>
    </div>
  );
}

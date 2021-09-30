import clsx from "clsx";
import { Suspense } from "react";
import ContainerSpinner from "../UIComponents/Spinner/ContainerSpinner";
import TestComponent from "../Components/TestComponent"
export default function Home() {

      return <div
        className={clsx(
          "flex h-full w-full",
          "text-xs md:text-sm max-h-full space-y-1",
          "p-5"
        )}
      >
  
        <div className="flex w-full max-w-5xl mx-auto mt-14">
          <div
            className={clsx(
              "flex flex-col md:flex-row space-x-0 md:space-x-5",
              "space-y-2 md:space-y-0 w-full h-full xl:space-x-10"
            )}
          >
            <div className="flex-1">
                <Suspense fallback={<ContainerSpinner />}>
                  <TestComponent />
                </Suspense>
            </div>
          </div>
        </div>
      </div>
  }
  
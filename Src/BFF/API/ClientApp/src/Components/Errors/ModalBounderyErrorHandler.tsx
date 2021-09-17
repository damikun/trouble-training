
import { faSyncAlt } from "@fortawesome/free-solid-svg-icons";
import clsx from "clsx";
import { ReactComponent as ServerDown } from "../../Images/serverDown.svg";
import StayledButton from "../../UIComponents/Buttons/StayledButton";
import Container from "../../UIComponents/Container/Container";
import ContentContainer from "../../UIComponents/Container/ContentContainer";
import { useBoundaryContext } from "../../UIComponents/ErrorBoundery/ErrorBoundary";

export default function ModalBounderyErrorHandler() {
    const context = useBoundaryContext();
  
    return (
      <ContentContainer className={"flex"}>
        <Container
          flextype="flex-1"
          className="h-full w-full max-h-full max-w-full items-center"
        >
          <div
            className={clsx(
              "flex flex-col relative w-full",
              "mx-auto py-5 px-5 justify-center"
            )}
          >
            <ServerDown className="h-32" />
  
            <div
              className={clsx(
                "flex px-2 flex-col break-words bottom-0 mt-5",
                "whitespace-normal text-center py-2 space-y-2 prose-md"
              )}
            >
              <h3
                className={clsx(
                  "pt-2 text-lg md:text-xl text-center text-gray-400",
                  "break-words leading-tight capitalize font-semibold"
                )}
              >
                Failed to fetch
              </h3>
  
              <div
                className={clsx(
                  "flex font-semibold text-sm md:text-base",
                  "break-words pt-3 justify-center"
                )}
              >
                <StayledButton
                  className={clsx(
                    "flex my-auto text-lg text-white",
                    "w-full md:w-32 py-1"
                  )}
                  variant="secondarygray"
                  onClick={context.reset}
                  name="Retry"
                  selected={false}
                  iconLeft={faSyncAlt}
                >
                  Retry
                </StayledButton>
              </div>
            </div>
          </div>
        </Container>
      </ContentContainer>
    );
  }
  
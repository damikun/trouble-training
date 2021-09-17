import { useTransition, useCallback } from "react";
import Container from "../../UIComponents/Container/Container";
import { ReactComponent as NoAuth } from "../../Images/NoAuth.svg";
import StayledButton from "../../UIComponents/Buttons/StayledButton";
import { useNavigate } from "react-router-dom";
import clsx from "clsx";

export default function FourOhOne() {
  const history = useNavigate();

  const [isPending, startTransition] = useTransition();

  const handleRedirectHome = useCallback(() => {
    startTransition(() => {
      !isPending && history("/");
    });
  }, [startTransition, isPending, history]);

  const handleRedirectBack = useCallback(() => {
    startTransition(() => {
      !isPending && history(-1);
    });
  }, [startTransition, isPending, history]);

  return (
    <div
      className={clsx(
        "flex-1 h-full w-full max-h-full max-w-full",
        "text-xs md:text-sm"
      )}
    >
      <Container
        flextype="flex-1"
        flexwrap="flex-no-wrap"
        className="h-full w-full max-h-full max-w-full items-center"
      >
        <div className="flex p-2 overflow-hidden h-full items-center justify-center">
          <div
            className={clsx(
              "flex flex-row flex-wrap w-full max-w-screen-lg shadow-lg",
              "rounded-lg border border-gray-300 bg-white p-3"
            )}
          >
            <div
              className={clsx(
                "w-full h-auto bg-clip-border hidden md:block",
                "lg:w-1/2 bg-cover rounded-l-lg items-center",
                "object-cover object-bottom"
              )}
            >
              <NoAuth className="buttom-0 max-w-full fill max-h-80 px-5" />
            </div>

            <div
              className={clsx(
                "w-full lg:w-1/2 overflow-hidden bg-white px-1",
                "sm:px-2 md:px-5 pt-5 rounded-lg md:rounded-l-none",
                "my-auto w-full"
              )}
            >
              <div
                className={clsx(
                  "flex px-2 flex-col break-words",
                  "whitespace-normal text-center"
                )}
              >
                <h1
                  className={clsx(
                    "pt-2 text-6xl text-center text-gray-500",
                    "break-words leading-tight capitalize"
                  )}
                >
                  401
                </h1>

                <div className="font-semibold text-base  break-words">
                  <p className="break-normal">Hold up! You are Unauthorised</p>
                </div>

                <p
                  className={clsx(
                    "text-gray-400 text-md md:text-basefont-sans",
                    "break-normal py-3"
                  )}
                >
                  Server detected you do not have right premissions to access
                  this protected resource.
                </p>

                <div className="flex gap-x-3">
                  <div className="text-center my-2 mb-5 h-10 flex-1">
                    <StayledButton
                      onClick={handleRedirectBack}
                      variant="secondarygray"
                      className="flex-1 my-auto w-full  "
                      type="submit"
                    >
                      <div className="mx-auto ">Back</div>
                    </StayledButton>
                  </div>
                  <div className="text-center my-2 mb-5 h-10 flex-1">
                    <StayledButton
                      onClick={handleRedirectHome}
                      variant="secondaryblue"
                      className="flex-1 my-auto w-full "
                      type="submit"
                    >
                      <div className="mx-auto ">Home</div>
                    </StayledButton>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </Container>
    </div>
  );
}

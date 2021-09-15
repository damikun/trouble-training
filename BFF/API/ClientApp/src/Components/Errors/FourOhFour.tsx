import Container from "../../UIComponents/Container/Container";
import { ReactComponent as NotFound } from "../../Images/NotFound.svg";
import StayledButton from "../../UIComponents/Buttons/StayledButton";
import { useNavigate } from "react-router";
import clsx from "clsx";

export default function FourOhFour() {
  const history = useNavigate();

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
        <div className="flex p-2 overflow-hidden h-full ">
          <div
            className={clsx(
              "flex w-full max-w-screen-lg mx-auto my-auto shadow-lg",
              "rounded-lg border border-gray-300 bg-white"
            )}
          >
            <div
              className={clsx(
                "w-full h-auto hidden md:block md:w-1/2 bg-cover",
                "items-center object-cover object-bottom rounded-l-lg"
              )}
            >
              <NotFound className="buttom-0 max-w-full fill max-h-96 px-5" />
            </div>

            <div
              className={clsx(
                "w-full md:w-1/2 overflow-hidden bg-white px-1",
                "sm:px-2 md:px-5 pt-5 rounded-lg md:rounded-l-none",
                "my-auto"
              )}
            >
              <div
                className={clsx(
                  "flex px-2 flex-col break-words whitespace-normal text-center"
                )}
              >
                <h1
                  className={clsx(
                    "pt-2 m-0 text-6xl text-center text-gray-500 break-words",
                    "leading-tight capitalize"
                  )}
                >
                  404
                </h1>
                <h3
                  className={clsx(
                    "pt-2 text-xl md:text-2xl text-center",
                    "text-gray-500 break-words leading-tight capitalize"
                  )}
                >
                  Page Not found
                </h3>

                <div className="font-semibold text-base  break-words">
                  <p className="break-normal">
                    The page you`re looking for was not found.
                  </p>
                </div>

                <p className="text-gray-400 text-md md:text-basefont-sans break-normal py-3">
                  Please make sure the spelling of URL is correct or you have
                  proprer rights to access it. Page mey have been removed or
                  renamed. What you can do? No fear select your option.
                </p>

                <div className="flex gap-x-3">
                  <div className="text-center my-2 mb-5 h-10 flex-1">
                    <StayledButton
                      onClick={() => history(-1)}
                      variant="secondarygray"
                      className="flex-1 my-auto w-full  "
                      type="submit"
                    >
                      <div className="mx-auto ">Back</div>
                    </StayledButton>
                  </div>
                  <div className="text-center my-2 mb-5 h-10 flex-1">
                    <StayledButton
                      onClick={() => history("/")}
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

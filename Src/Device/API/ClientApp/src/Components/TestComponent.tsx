import StayledButton from "../UIComponents/Buttons/StayledButton"

export default function TestComponent() {

    return <div className="flex w-full h-full justify-center content-center items-center">

            <div className="flex flex-col space-y-5 p-5">
                <StayledButton
                    size="large"
                    variant="error">
                        Unauthorised
                </StayledButton>

                <StayledButton
                    size="large"
                    variant="secondarygreen">
                        Authorised
                </StayledButton>
            </div>
        </div>
  }
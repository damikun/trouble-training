import { useMutation } from "react-relay";
import StayledButton from "../UIComponents/Buttons/StayledButton"
import { graphql } from "babel-plugin-relay/macro";
import { useCallback } from "react";
import { HandleErrors } from "../Utils/ErrorHelper";
import { useToast } from "../UIComponents/Toast/ToastProvider";
import { TestComponentTriggerAuthorisedMutation } from "./__generated__/TestComponentTriggerAuthorisedMutation.graphql";
import { TestComponentTriggerUnAuthorisedMutation } from "./__generated__/TestComponentTriggerUnAuthorisedMutation.graphql";

export default function TestComponent() {
    const toast = useToast();

    const [
    commit_authorised,
    authorised_isInFlight,
    ] = useMutation<TestComponentTriggerAuthorisedMutation>(graphql`
    mutation TestComponentTriggerAuthorisedMutation {
        triggerAuthorisedRequest {
            ... on Trigger_AuthorisedPayload {
                errors {
                    ... on IBaseError {
                        message
                    }
                }
            }
        }
    }
    `);

    const [
    commit_unauthorised,
    unauthorised_isInFlight,
    ] = useMutation<TestComponentTriggerUnAuthorisedMutation>(graphql`
    mutation TestComponentTriggerUnAuthorisedMutation {
        triggerUnAuthorisedRequest {
            ... on Trigger_UnAuthorisedPayload {
                    errors {
                    ... on IBaseError {
                        message
                    }
                }
            }
        }
    }
    `);

    const handleAuthorisedRequest = useCallback(() => {

        if(!authorised_isInFlight){

            commit_authorised({
                variables: { },
      
                onError(error) {
                  toast?.pushError("Failed to process mutation");
                  console.log(error);
                },
      
                onCompleted(response) {},
      
                updater(store, response) {
                  HandleErrors(toast, response.triggerAuthorisedRequest?.errors);
                  if (response.triggerAuthorisedRequest?.errors?.length === 0) {
                    toast?.pushSuccess("Request success");
                  }
                },
              });
        }

    }, [authorised_isInFlight,commit_authorised,toast])

    const handleUnAuthorisedRequest = useCallback(() => {

        if(!unauthorised_isInFlight){

            commit_unauthorised({
                variables: { },
      
                onError(error) {
                  toast?.pushError("Failed to process mutation");
                  console.log(error);
                },
      
                onCompleted(response) {},
      
                updater(store, response) {
                  HandleErrors(toast, response.triggerUnAuthorisedRequest?.errors);
                },
              });
        }

    }, [unauthorised_isInFlight,commit_unauthorised,toast])
      
    return <div className="flex w-full h-full justify-center content-center items-center">

            <div className="flex flex-col space-y-5 p-5">

                <StayledButton
                    onClick={handleAuthorisedRequest}
                    size="large"
                    variant="secondarygreen">
                        Authorised
                </StayledButton>
                <StayledButton
                    onClick={handleUnAuthorisedRequest}
                    size="large"
                    variant="error">
                        Unauthorised
                </StayledButton>

            </div>
        </div>
  }
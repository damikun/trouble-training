import clsx from "clsx";
import React, { useTransition, useCallback, useState } from "react";
import { useLazyLoadQuery, useMutation } from "react-relay/hooks";
import { graphql } from "babel-plugin-relay/macro";
import UniversalFormInputSection from "../../../UIComponents/Inputs/UniversalFormInputSection.tsx";
import { HooksNewQuery } from "./__generated__/HooksNewQuery.graphql";
import SectionTitle from "../../../UIComponents/Section/SectionTitle";
import Section from "../../../UIComponents/Section/Section";
import Toggle from "../../../UIComponents/Toggle/Toggle";
import StayledButton from "../../../UIComponents/Buttons/StayledButton";
import { useToast } from "../../../UIComponents/Toast/ToastProvider";
import { useFormik } from "formik";
import {
  HookEventType,
  HooksNewMutation,
} from "./__generated__/HooksNewMutation.graphql";
import { HandleErrors } from "../../../Utils/ErrorHelper";
import { useNavigate } from "react-router";
import { generateErrors, is } from "../../../Utils/Validation";
import { URL_REGEX } from "../../../constants";
import { EVENT_TRIGGERS } from "./HooksShared";
import { useHooksContext } from "../Tabs";

const HooksNewQueryTag = graphql`
  query HooksNewQuery {

    # This is for type demo uncoment for needs
    # This is antipattern but can be used in specific scenarios!
    #
    # __type(name:"HookEventType"){
    #   enumValues{
    #     name
    #     description
    #   }
    # }

      webHookEventsTriggers
  }
`;


function getTriggerArr(map: Map<string, boolean>) {
  const triggers: Array<string> = [];
  map.forEach((value, key) => {
    if (value) triggers.push(key);
  });
  return triggers;
}

export default HooksNew;

function HooksNew() {

  const [triggers, setTriggers] = useState({
    checkedItems: new Map<string, boolean>(),
  });

  const toast = useToast();

  // For introspection demo
  //@ts-ignore
  const data = useLazyLoadQuery<HooksNewQuery>(
    HooksNewQueryTag,
    { },
    {
      fetchPolicy: "store-or-network",
    }
  );

  const navigate = useNavigate();

  const [_,startTransition] = useTransition();

  const handleTriggerChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      const item = e.target.id;
      const isChecked = e.target.checked;

      setTriggers((prevState) => ({
        checkedItems: prevState.checkedItems.set(item, isChecked),
      }));
    },
    [setTriggers]
  );

  const hooksCtx = useHooksContext();

  const [
    commit,
    isInFlight,
  ] = useMutation<HooksNewMutation>(graphql`
    mutation HooksNewMutation(
      $request: CreateWebHookInput
      $connections: [ID!]!
      ) {
      createWebHook(request: $request) {
        ... on CreateWebHookPayload {
          errors {
            ... on IBaseError {
              message
            }
          }
            
          hook    
            @prependNode(
              connections: $connections
              edgeTypeName: "GQL_WebHook"
            ){
              id
              systemid
              webHookUrl
              isActive
          }
        }
      }
    }
  `);

  const [validate, setValidate] = useState(false);

  const formik = useFormik({
    initialValues: {
      hookUrl: "",
      secret: "",
    },
    onSubmit: async (values) => {
        !isInFlight &&
        commit({
          variables: {
            request: {
              hookEvents: getTriggerArr(
                triggers.checkedItems
              ) as HookEventType[],
              webHookUrl: values.hookUrl.trim(),
              secret: values.secret,
              isActive: true,
            },
            connections: hooksCtx?.connection_id
            ? [hooksCtx.connection_id]
            : [],
          },

          onError(error) {
            toast?.pushError("Failed to process mutation");
            console.log(error);
          },

          onCompleted(response) {},

          updater(store, response) {
            HandleErrors(toast, response.createWebHook?.errors);
            if (response.createWebHook?.errors?.length === 0) {
              startTransition(() => {
                navigate(`/Hooks`);
              });
            }
          },

        });

    },


    validate: (values) => {
      return generateErrors(values, {
        hookUrl: [
          is.required(),
          is.minLength(2),
          is.match(() => {
            return values.hookUrl.match(URL_REGEX);
          }, "Invalid URL format"),
        ],
      });
    },

    validateOnChange:validate

  });

  const is_form_error = formik.errors.hookUrl != null || formik.errors.secret != null

  const formikSubmitPreProcessor = useCallback(
    (e?: React.FormEvent<HTMLFormElement> | undefined) => {
      
      setValidate(
        is_form_error);

      formik.handleSubmit(e);
    },
    [formik, is_form_error, formik.errors.hookUrl],
  )

  const formikChangePreProcessor = useCallback(
    (e: React.ChangeEvent<any>) => {
      
      setValidate(
        is_form_error);

      formik.handleChange(e);
    },
    [formik, is_form_error, formik.errors.hookUrl],
  )
  
  return (
    <div
      className={clsx(
        "flex w-full h-full rounded-b-md max-h-full overflow-hidden",
        "relative max-w-full overflow-y-scroll scrollbarwidth",
        "scrollbarhide scrollbarhide2"
      )}
    >
      <div className="absolute w-full align-middle">
        <div className="h-full relative max-w-full flex-col">
          <form
            onSubmit={formikSubmitPreProcessor}
            onChange={formikChangePreProcessor}
            className="flex flex-col max-w-3xl space-y-2"
          >
            <Header />

            <Description />

            <div
              className={clsx(
                "flex flex-col border rounded-md",
                "justify-center bg-white shadow-sm"
              )}
            >
              <div className="flex flex-col w-full h-full p-5">
                <UniversalFormInputSection
                  reserveValiadtionErrorSpace
                  isInFlight={isInFlight}
                  error={formik.errors.hookUrl}
                  value={formik.values.hookUrl}
                  onChange={formik.handleChange}
                  name="URL"
                  placeholder="Payload url"
                  form_id="hookUrl"
                />
                <UniversalFormInputSection
                  reserveValiadtionErrorSpace
                  isInFlight={isInFlight}
                  error={formik.errors.secret}
                  value={formik.values.secret}
                  onChange={formik.handleChange}
                  name="Secret"
                  placeholder="Client secret"
                  form_id="secret"
                />
                <Section>
                  <SectionTitle name="Triggers" />
                  <div className="flex flex-col space-y-1">
                    {EVENT_TRIGGERS.map((enity) => (
                      <div
                        className={clsx(
                          "flex flex-nowrap justify-between",
                          "items-center space-x-5"
                        )}
                      >
                        <div className="flex flex-col">
                          <h6 className="font-bold text-base capitalize">
                            {enity.id}
                          </h6>
                          <p className="text-sm prose-sm">
                            {enity.description}
                          </p>
                        </div>

                        <Toggle
                          disabled={false}
                          checked={triggers.checkedItems.get(enity.id) ?? false}
                          onChange={handleTriggerChange}
                          id={enity.id}
                          name={enity.id}
                          type="checkbox"
                        />
                      </div>
                    ))}
                  </div>
                </Section>
              </div>
              <div className=" justify-start p-5">
                <StayledButton
                  disabled={is_form_error}
                  isloading={isInFlight}
                  variant={!is_form_error? "secondaryblue":"secondarygray"}
                  size="normal"
                  type="submit"
                >
                  Create WebHook
                </StayledButton>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}

///////////////////////////////////////////
///////////////////////////////////////////

function Header() {
  return (
    <div className="flex justify-between flex-nowrap space-x-2">
      <div className="flex font-bold text-gray-800 text-md text-lg px-1 h-8">
        Create WebHook
      </div>
    </div>
  );
}

function Description() {
  return (
    <p className="prose-sm px-1">
      Webhooks allow external services to be notified when certain events
      happen. It send`s the POST request to list of specified URLs with custom
      body content.
    </p>
  );
}

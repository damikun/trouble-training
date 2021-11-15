import clsx from "clsx";
import React, { useTransition, useCallback, useState, useMemo } from "react";
import { useLazyLoadQuery, useMutation } from "react-relay/hooks";
import { useParams } from "react-router-dom";
import { graphql } from "babel-plugin-relay/macro";
import UniversalFormInputSection from "../../../UIComponents/Inputs/UniversalFormInputSection.tsx";
import SectionTitle from "../../../UIComponents/Section/SectionTitle";
import Section from "../../..//UIComponents/Section/Section";
import Toggle from "../../../UIComponents/Toggle/Toggle";
import StayledButton from "../../../UIComponents/Buttons/StayledButton";
import { useToast } from "../../../UIComponents/Toast/ToastProvider";
import { useFormik } from "formik";
import { HandleErrors } from "../../../Utils/ErrorHelper";
import { useNavigate } from "react-router";
import { generateErrors, is } from "../../../Utils/Validation";
import { URL_REGEX } from "../../../constants";
import { EVENT_TRIGGERS } from "./HooksShared";
import {
  HookEventType,
  HooksEditMutation,
} from "./__generated__/HooksEditMutation.graphql";
import { HooksEditQuery } from "./__generated__/HooksEditQuery.graphql";
import RecordNotFound from "../../../UIComponents/RecordNotFound";

const HooksEditQueryTag = graphql`
  query HooksEditQuery($hookid: ID!) {
    webHookById(webhook_id: $hookid) {
      id
      systemid
      webHookUrl
      isActive
      listeningEvents
    }
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

function getTriggersInitState(
  triggers: readonly HookEventType[] | null | undefined
) {
  const map = new Map<string, boolean>();

  triggers?.forEach((element) => {
    map.set(element, true);
  });

  return map;
}



 export default function HooksEdit() {
  const { hookid }: any = useParams();

  const toast = useToast();

  const [memorised_hook_id] = useState<string>(hookid)

  const data = useLazyLoadQuery<HooksEditQuery>(
    HooksEditQueryTag,
    {
      hookid: memorised_hook_id,
    },
    {
      fetchPolicy: "store-or-network",
    }
  );

  const [triggers, setTriggers] = useState({
    checkedItems: getTriggersInitState(data.webHookById?.listeningEvents),
  });

  const navigate = useNavigate();

  const [isFormChanged, setFormChanged] = useState(false);

  const [secretEditing, setSecretEditing] = useState(false);

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

  const handleActivChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      const isChecked = e.target.checked;
      formik.setFieldValue("isActiv", isChecked);
    },
    [setTriggers]
  );

  const [
    commit,
    isInFlight,
  ] = useMutation<HooksEditMutation>(graphql`
    mutation HooksEditMutation($request: UpdateWebHookInput) {
      updateWebHook(request: $request) {
        ... on UpdateWebHookPayload {
          errors {
            ... on IBaseError {
              message
            }
          }
          hook {
            id
            webHookUrl
            isActive
            listeningEvents
          }
        }
      }
    }
  `);

const [validate, setValidate] = useState(false);

  const formik = useFormik({
    initialValues: {
      hookId: data?.webHookById?.systemid as number,
      hookUrl: data.webHookById?.webHookUrl ? data.webHookById?.webHookUrl : "",
      secret: "",
      isActiv: data.webHookById?.isActive ? data.webHookById.isActive : false,
    },
    onSubmit: async (values) => {
      data?.webHookById?.systemid &&
        !isInFlight &&
        commit({
          variables: {
            request: {
              webHookId: values.hookId,
              hookEvents: getTriggerArr(
                triggers.checkedItems
              ) as HookEventType[],
              webHookUrl: values.hookUrl.trim(),
              secret: secretEditing ? values.secret : null,
              isActive: values.isActiv,
            },
          },

          onError(error) {
            toast?.pushError("Failed to process mutation");
            console.log(error);
          },

          onCompleted(response) {},

          updater(store, response) {
            HandleErrors(toast, response.updateWebHook?.errors);
            if (response.updateWebHook?.errors?.length === 0) {
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
            return values?.hookUrl ? values?.hookUrl.match(URL_REGEX) : false;
          }, "Invalid URL format"),
        ],
        hookId: [
          is.match(() => {
            return values?.hookId > 0;
          }, "hook id invalid value"),
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
      
      !isFormChanged && setFormChanged(true)

      setValidate(
        is_form_error);

      formik.handleChange(e);
    },
    [formik, is_form_error, formik.errors.hookUrl,isFormChanged],
  )


  if (!data.webHookById) {
    return (
      <div
        className={clsx(
          "flex w-full h-full max-h-full overflow-hidden",
          "relative max-w-full items-center justify-center"
        )}
      >
        <RecordNotFound message="Hook not found" />
      </div>
    );
  }

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
                <div className="flex justify-between">
                  {!secretEditing ? (
                    <Section>
                      <SectionTitle name="Secret" />

                      <div className="flex flex-wrap h-8 items-center">
                        <p className="pr-1">
                          Click on the link if you wanna edit
                        </p>
                        <a
                          className="font-bold text-blue-500 hover:text-blue-600 cursor-pointer"
                          onClick={() => {
                            setSecretEditing(true);
                          }}
                        >
                          Hook Secret
                        </a>
                      </div>
                    </Section>
                  ) : (
                    <div className="flex space-x-2 relative w-4/5">
                      <div className="flex w-full">
                        <UniversalFormInputSection
                          isInFlight={isInFlight}
                          error={formik.errors.secret}
                          value={formik.values.secret}
                          focusOnMount
                          onChange={formik.handleChange}
                          name="Secret"
                          placeholder="Client secret"
                          form_id="secret"
                        />
                      </div>

                      <Section className="flex max-w-12">
                        <SectionTitle hideBoubleDot name={" "} />
                        <div className="flex items-center h-8">
                          <StayledButton
                            onClick={() => {
                              setSecretEditing(false);
                            }}
                            className="bottom-0"
                            size="normal"
                            variant="primaryblue"
                            type="reset"
                          >
                            Cancle
                          </StayledButton>
                        </div>
                      </Section>
                    </div>
                  )}
                  <div className="max-w-14">
                    <Section>
                      <SectionTitle
                        className="justify-end"
                        hideBoubleDot={true}
                        name="Enable"
                      />
                      <div className="flex justify-end items-center">
                        <Toggle
                          disabled={false}
                          checked={formik.values.isActiv}
                          onChange={handleActivChange}
                          id={"isActiv"}
                          name={"isActiv"}
                          type="checkbox"
                        />
                      </div>
                    </Section>
                  </div>
                </div>

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
              <div
                className={clsx(
                  "justify-start items-center px-5 flex h-14",
                  isFormChanged ? "visible" : "invisible"
                )}
              >
                <StayledButton
                  disabled={is_form_error}
                  isloading={isInFlight}
                  variant={!is_form_error? "secondaryblue":"secondarygray"}
                  size="normal"
                  type="submit"
                >
                  Update WebHook
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
        Edit WebHook
      </div>
    </div>
  );
}

function Description() {
  return (
    <p className="text-xs md:prose-sm px-1">
      Webhooks allow external services to be notified when certain events
      happen. It send`s the POST request to list of specified URLs with custom
      body content.
    </p>
  );
}

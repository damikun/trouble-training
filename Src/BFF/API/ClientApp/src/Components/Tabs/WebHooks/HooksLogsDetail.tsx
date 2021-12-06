import { graphql } from "babel-plugin-relay/macro";
import { useLazyLoadQuery } from "react-relay/hooks";
import ReactJson from "react-json-view";
import { useSearchParams } from "react-router-dom";
import React, { useMemo, useState } from "react";
import ContentContainer from "../../../UIComponents/Container/ContentContainer";
import Section from "../../../UIComponents/Section/Section";
import SectionTitle from "../../../UIComponents/Section/SectionTitle";
import { HooksLogsDetailQuery } from "./__generated__/HooksLogsDetailQuery.graphql";
import clsx from "clsx";
import ModalFormControl from "../../../UIComponents/FormControl";
import CopyToClipboard from "../../../UIComponents/CopyToClipboard/CopyToClipboard";
import { ItemWrapperContainer } from "../../../UIComponents/Container/ItemWrapperContainer";

const HooksLogsDetailQueryTag = graphql`
  query HooksLogsDetailQuery($hook_record_id: ID!) {
    webHookRecord(hook_record_id: $hook_record_id) {
      id
      statusCode
      timestamp
      triggerType
      guid
      result
      webHook {
        id
        systemid
        webHookUrl
      }
      exception
      requestHeaders
      requestBody
      responseBody
    }
  }
`;

function GetJsonData(
  JSON_string: string | null | undefined,
  defaultvalue: string = `{}`
) {
  try {
    return JSON.parse(
      JSON_string !== null && JSON_string !== undefined
        ? JSON_string
        : defaultvalue
    );
  } catch {
    return JSON.parse(defaultvalue);
  }
}

export type HooksLogsDetailProps = {
  onClose: () => void;
};

export default function HooksLogsDetail(
  props: HooksLogsDetailProps
) {
  const [searchParams] = useSearchParams();

  const [hookId] = useState(searchParams.get("hook_id"));

  const enity_detail_data = useLazyLoadQuery<HooksLogsDetailQuery>(
    HooksLogsDetailQueryTag,
    { hook_record_id: hookId ? hookId : "" },
    {
      fetchPolicy: "store-or-network",
    }
  );

  return (
    <ContentContainer
      notFound={enity_detail_data.webHookRecord == null}
      className={"flex-1 md:w-md lg:w-lg w-full"}
      header={<div>WebHook call detail</div>}
    >
      <div className="flex flex-col space-y-1">
        <div className="flex flex-col mt-2">
          <div className="flex flex-row justify-between">
            <Guid guid={enity_detail_data.webHookRecord?.guid} />
            <TimeStampSection
              timeStamp={enity_detail_data.webHookRecord?.timestamp}
            />
          </div>
        </div>
        <div className="flex flex-row justify-between">
          <TriggerType
            trigger_type={enity_detail_data.webHookRecord?.triggerType}
          />
          <StatusCodeSection
            status={enity_detail_data.webHookRecord?.statusCode}
          />
        </div>
        <div className="flex flex-col">
          <JsonRequestHeader
            data={enity_detail_data.webHookRecord?.requestHeaders}
          />
        </div>
        <div className="flex flex-col">
          <JsonRequestBody
            data={enity_detail_data.webHookRecord?.requestBody}
          />
        </div>
        <div className="flex flex-col">
          <JsonResponseBody
            data={enity_detail_data.webHookRecord?.responseBody}
          />
        </div>

        {enity_detail_data.webHookRecord?.exception && (
          <div className="flex flex-col">
            <ExceptionSection
              exception={enity_detail_data.webHookRecord?.exception}
            />
          </div>
        )}
        <div className="flex mt-5 w-full justify-end">
          <ModalFormControl cancleOnly positionMobile="justify-end" />
        </div>
      </div>
    </ContentContainer>
  );
}

/////////////////////////////////////

type GuidProps = {
  guid: string | null | undefined;
};

function Guid({ guid }: GuidProps) {
  return (
    <Section id="__EventSection">
      <SectionTitle name={"Event"} />

      <div className="flex h-10 items-center truncate-1-lines break-all my-auto">
        {guid}
      </div>
    </Section>
  );
}

/////////////////////////////////////

type TriggerTypeProps = {
  trigger_type: any;
};

function TriggerType({ trigger_type }: TriggerTypeProps) {
  return (
    <Section id="__TriggerType">
      <SectionTitle name={"Trigger"} />

      <div className="flex capitalize truncate-1-lines break-all">
        {trigger_type}
      </div>
    </Section>
  );
}

/////////////////////////////////////

type TimeStampSectionProps = {
  timeStamp: string | null | undefined;
};

function TimeStampSection({ timeStamp }: TimeStampSectionProps) {
  return (
    <Section id="__TimeStampSection">
      <SectionTitle hideBoubleDot className="justify-end" name={"TimeStamp"} />
      <div className="flex flex-col max-w-full ml-auto ">
        <div className="float-right max-w-full ml-auto ">
          {timeStamp && new Date(timeStamp).toLocaleDateString()}
        </div>
        <div className="float-right max-w-full ml-auto ">
          {timeStamp && new Date(timeStamp).toLocaleTimeString()}
        </div>
      </div>
    </Section>
  );
}

///////////////////////////////////////////
///////////////////////////////////////////

type StatusCodeSectionProps = {
  status: number | null | undefined;
};

function StatusCodeSection({ status }: StatusCodeSectionProps) {
  const status_style = useMemo(() => {
    if (status) {
      if (status >= 200 && status <= 299) {
        return "text-white bg-green-400";
      } else {
        return "text-white bg-red-400";
      }
    } else {
      return "bg-gray-200 text-gray-700";
    }
  }, []);

  if (!status) {
    return <></>;
  }

  return (
    <Section className="justify-end" id="__StatusCodeSection">
      <SectionTitle
        hideBoubleDot
        className="justify-end"
        name={"Status Code"}
      />
      <div className="flex float-right justify-end w-full">
        <div
          className={clsx(
            "px-1.5 py-0.5 flex leading-none truncate-1-lines",
            "break-all rounded-md font-semibold text-sm text-center",
            status_style
          )}
        >
          {status}
        </div>
      </div>
    </Section>
  );
}

/////////////////////////////////////

type JsonRequestHeaderProps = {
  data: string | null | undefined;
};

function JsonRequestHeader({ data }: JsonRequestHeaderProps) {
  if (data === null || (data && data?.length <= 1)) {
    return <></>;
  }
  const JsonConnectionData = GetJsonData(data);
  return (
    <Section id="__JsonRequestHeader">
      <SectionTitle name={"Request Headers"} />

      <div className="rounded-md p-3 bg-gray-50 shadow-sm border border-gray-300">
        <div className="flex overflow-hidden overflow-y-auto text-xs ">
          <ReactJson
            collapsed={false}
            enableClipboard={false}
            src={JsonConnectionData}
          />
        </div>
      </div>
    </Section>
  );
}

/////////////////////////////////////

type JsonRequestBodyProps = {
  data: string | null | undefined;
};

function JsonRequestBody({ data }: JsonRequestBodyProps) {
  if (data === null || (data && data?.length <= 1)) {
    return <></>;
  }
  const JsonConnectionData = GetJsonData(data);
  return (
    <Section id="__JsonRequestBody">
      <SectionTitle name={"Request Body"} />

      <div className="rounded-md p-3 bg-gray-50 shadow-sm border border-gray-300">
        <div className="flex overflow-hidden overflow-y-auto text-xs ">
          <ReactJson
            collapsed={false}
            enableClipboard={false}
            src={JsonConnectionData}
          />
        </div>
      </div>
    </Section>
  );
}

/////////////////////////////////////

type JsonResponseBodyProps = {
  data: string | null | undefined;
};

function JsonResponseBody({ data }: JsonResponseBodyProps) {
  if (data === null || (data && data?.length <= 1)) {
    return <></>;
  }
  const JsonConnectionData = GetJsonData(data);
  return (
    <Section id="__JsonResponseBody">
      <SectionTitle name={"Response Body"} />

      <div className="rounded-md p-3 bg-gray-50 shadow-sm border border-gray-300">
        <div className="flex overflow-hidden overflow-y-auto text-xs ">
          <ReactJson
            collapsed={false}
            enableClipboard={false}
            src={JsonConnectionData}
          />
        </div>
      </div>
    </Section>
  );
}

/////////////////////////////////////

type ExceptionSectionProps = {
  exception: string | null | undefined;
};

function ExceptionSection({ exception }: ExceptionSectionProps) {
  if (exception === null || (exception && exception?.length <= 1)) {
    return <></>;
  }

  return (
    <Section id="__JsonResponseBody">
      <SectionTitle name={"Response Body"} />

      <ItemWrapperContainer
        turncate="truncate-2-lines"
        className={clsx(
          "border border-gray-200 text-gray-800",
          "text-opacity-50 font-semibold"
        )}
      >
        {exception}
      </ItemWrapperContainer>
    </Section>
  );
}

/////////////////////////////////////

type CopyToClipboardWrapperProps = {
  data?: string | null | undefined;
  children?: React.ReactNode;
};
function CopyToClipboardWrapper({
  data,
  children,
}: CopyToClipboardWrapperProps) {
  return (
    <div className="group relative">
      <CopyToClipboard
        iconOnly
        size="small"
        className="absolute top-0 right-0 -mr-8 -mb-3"
        dataSource={data}
      />
      {children}
    </div>
  );
}

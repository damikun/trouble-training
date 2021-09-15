import clsx from "clsx";
import { graphql } from "babel-plugin-relay/macro";
import React, { useCallback, useEffect, useState } from "react";
import { useFragment, useMutation } from "react-relay/hooks";

import StayledContainerItemWrapper from "../../../UIComponents/ScrollContainter/StayledContainerItemWrapper";
import Section from "../../Shared/Section";
import SectionTitle from "../../Shared/SectionTitle";
import { useToast } from "../../../UIComponents/Toast/ToastProvider";
import { HandleErrors } from "../../../Utils/ErrorHelper";

export type ActionSetStatusProps = {
  // dataRef: ActionSetStatus$key | null;
};

// const ActionSetStatusTag = graphql`
//   fragment ActionSetStatus on GQL_Project
//   @refetchable(
//     queryName: "ActionSetStatusPhoneRefetchQuery"
//   ) {
//     id
//     systemid
//     name
//     ...ActionSetStatus_settings @relay(mask: false)

//     viewerPremissions {
//       project_update
//       project_role
//     }
//   }
// `;

// const ActionSetStatus_settingsTag = graphql`
//   fragment ActionSetStatus_settings on GQL_Project {
//     settings {
//       __id
//       files_update_level
//       dashboardSettings {
//         __id
//         show_info_widget
//         show_milestone_widget
//         show_notes_widget
//       }
//     }
//   }
// `;

// const UpdateProjectAvatarTag = graphql`
//   mutation ActionSetStatusUpdateAvatarMutation(
//     $request: SetProjectDashBoardSettingsInput
//   ) {
//     setProjectDashobardSettimgs(request: $request) {
//       ... on SetProjectDashboardSettingsPayload {
//         errors {
//           ... on IBaseError {
//             message
//           }
//         }
//         project {
//           ...ActionSetStatus_settings
//         }
//       }
//     }
//   }
// `;

export default React.memo(ActionSetStatus);

function ActionSetStatus({
  // dataRef,
}: ActionSetStatusProps) {
  // const data = useFragment<ActionSetStatus$key>(
  //   ActionSetStatusTag,
  //   dataRef
  // );
  // const [
  //   commit,
  //   isInFlight,
  // ] = useMutation<ActionSetStatusUpdateAvatarMutation>(
  //   UpdateProjectAvatarTag
  // );

  // const [state, setstate] = useState(data?.settings.dashboardSettings);

  // useEffect(() => {
  //   setstate(data?.settings.dashboardSettings);
  // }, [data]);

  const toast = useToast();

  // const handleUpdate = useCallback(
  //   (new_state: any) => {
  //     new_state &&
  //       data?.systemid &&
  //       commit({
  //         variables: {
  //           request: {
  //             projectID: data?.systemid as number,
  //             settimgs: {
  //               show_info_widget: new_state.show_info_widget,
  //               show_milestone_widget: new_state.show_milestone_widget,
  //               show_notes_widget: new_state.show_notes_widget,
  //             },
  //           },
  //         },
  //         optimisticUpdater: (store) => {
  //           if (new_state && new_state.__id) {
  //             const record = store.get(new_state.__id);
  //             record?.setValue(new_state.show_info_widget, "show_info_widget");
  //             record?.setValue(
  //               new_state.show_milestone_widget,
  //               "show_milestone_widget"
  //             );
  //             record?.setValue(
  //               new_state.show_notes_widget,
  //               "show_notes_widget"
  //             );
  //           }
  //         },

  //         onError(error) {
  //           toast?.pushError("Failed to process mutation");
  //           console.log(error);
  //         },

  //         onCompleted(data) {
  //           if (data.setProjectDashobardSettimgs?.errors?.length === 0) {
  //           }
  //           HandleErrors(toast, data.setProjectDashobardSettimgs?.errors);
  //         },

  //         updater(store) {},
  //       });
  //   },
  //   [commit, state, data, data?.systemid, toast, HandleErrors]
  // );

  // const handle_info_setting_update = useCallback(
  //   (value) => {
  //     const new_state = {
  //       ...state,
  //       show_info_widget: value,
  //     };

  //     handleUpdate(new_state);
  //   },
  //   [state, handleUpdate]
  // );


  return (
    <Section>
      <SectionTitle variant="lovercase" name="Dashboard widgets" />
      <div
        className={clsx(
          "rounded-md flex flex-col space",
          "max-h-full shadow w-full"
        )}
      >
        Some data
      </div>
    </Section>
  );
}

// import { graphql } from "babel-plugin-relay/macro";
import clsx from "clsx";
// import { useLazyLoadQuery } from "react-relay/hooks";
import { useParams } from "react-router-dom";
import ViewContainer from "../../../UIComponents/ViewContainer/ViewContainer";


// const ProjectSettingsInfoQueryTag = graphql`
//   query ProjectSettingsInfoQuery($projectName: String!) {
//     projectByName(name: $projectName) {
//       id
//       systemid
//       name
//       ...ProjectSettingsInfoAvatarSection
//       ...ProjectSettingsInfoDashboardSection
//     }
//   }
// `;

export default function ProjectSettingsInfo() {
  const { name }: any = useParams();

  // const data = useLazyLoadQuery<ProjectSettingsInfoQuery>(
  //   ProjectSettingsInfoQueryTag,
  //   { projectName: name },
  //   { fetchPolicy: "store-or-network" }
  // );

  return (
    <ViewContainer
      bgcolor="bg-transparent"
      shadow={false}
      border={false}
      padding={false}
      content={
        <div
          className={clsx(
            "flex-1 rounded-b-md max-h-full overflow-hidden",
            "relative max-w-full overflow-y-scroll scrollbarwidth",
            "scrollbarhide scrollbarhide2"
          )}
        >
          <div className="absolute w-full align-middle">
            <div
              className={clsx(
                "h-full relative max-w-full",
                "grid grid-flow-row md:grid-cols-userSettings",
                "md:gap-5 xl:gap-10"
              )}
            >
              Add Info component to this place



            </div>
          </div>
        </div>
      }
    />
  );
}

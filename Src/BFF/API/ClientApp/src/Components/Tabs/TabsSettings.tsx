import  { RouterTabItemType} from "../../UIComponents/RouterTab/RouterTabList";

export const TabsSettings = [
  {
    label: "Welcome",
    path: ``,
  },
  {
    label: "WebHooks",
    path: `Hooks`,
    pattern: "Hooks/*",
  },
  {
    label: "Test @stream",
    path: `stream`,
    pattern: "stream/*",
  },
  {
    label: "Test @stream+@defer",
    path: `defer+stream`,
    pattern: "defer+stream/*",
  },
] as RouterTabItemType[];
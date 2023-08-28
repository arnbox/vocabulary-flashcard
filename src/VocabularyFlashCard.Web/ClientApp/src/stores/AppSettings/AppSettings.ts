import { AppSetttingsStorage } from "./AppSettingsStorage";
import { type IAppSettings, defaultAppSettings } from "./IAppSettings";
import { defineStore } from "pinia";

export const useAppSettingsStore = defineStore("appsettings", {
    state: () => {
        return {
            appSettings: defaultAppSettings
        };
    },

    actions: {
        GetAppSettings() {
            this.appSettings = AppSetttingsStorage.Load();
            return this.appSettings;
        },

        SetAppSettings(appSettings: IAppSettings) {
            this.appSettings = appSettings;
            AppSetttingsStorage.Save(appSettings);
        },

        ClearAppSettings() {
            this.appSettings = structuredClone(defaultAppSettings) as IAppSettings;
            AppSetttingsStorage.Clear();
        },
    },
});

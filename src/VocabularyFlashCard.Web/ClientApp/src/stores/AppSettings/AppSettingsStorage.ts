import { defaultAppSettings, type IAppSettings } from "./IAppSettings";

export class AppSetttingsStorage {
	private static keyName = "appsetttings";

	public static Save(appSettings: IAppSettings) {
		if (appSettings) {
			try {
				localStorage.setItem(this.keyName, JSON.stringify(appSettings));
			} catch (error) {
				console.log("Couldn't save app settings: ", error);
			}
		}
	}

	public static Load() {
		let appSettings = structuredClone(defaultAppSettings) as IAppSettings;
		try {
			const appSettingsString = localStorage.getItem(this.keyName);
			if (appSettingsString) {
				appSettings = JSON.parse(appSettingsString);
			}
		} catch (error) {
			console.log("Couldn't load app settings: ", error);
		}
		return appSettings;
	}

	public static Clear() {
		try {
			localStorage.removeItem(this.keyName);
		} catch (error) {
			console.log("Couldn't clear app settings: ", error);
		}
	}
}

import { type IAuth, defaultAuth } from "@/models/IAuth";

export class LoginStorage {
	private static loignKey = "login";

	public static Save(auth: IAuth) {
		if (auth) {
			try {
				localStorage.setItem(this.loignKey, JSON.stringify(auth));
			} catch (error) {
				console.log("Couldn't save login credentials: ", error);
			}
		}
	}

	public static Load() {
		let auth = structuredClone(defaultAuth) as IAuth;
		try {
			const authString = localStorage.getItem(this.loignKey);
			if (authString) {
				auth = JSON.parse(authString);
			}
		} catch (error) {
			console.log("Couldn't load login credentials: ", error);
		}
		return auth;
	}

	public static Clear() {
		try {
			localStorage.removeItem(this.loignKey);
		} catch (error) {
			console.log("Couldn't clear login credentials: ", error);
		}
	}
}

import { type IAuth, defaultAuth } from "@/models/IAuth";
import { LoginStorage } from "./LoginStorage";
import { defineStore } from "pinia";

export const useLoginStore = defineStore("login", {
	state: () => {
		return {
			loginStatus: structuredClone(defaultAuth) as IAuth,
			redirectPath: "",
			loading: false,
		};
	},
	actions: {
		setLoggedIn(auth: IAuth) {
			this.loginStatus = { ...auth };
			LoginStorage.Save(auth);
		},

		setLoggedOut() {
			this.loginStatus = structuredClone(defaultAuth) as IAuth;
			this.loading = false;
			this.redirectPath = "";
			LoginStorage.Clear();
		},

		authenticated() {
			if (this.loginStatus.isLoggedIn) {
				return true;
			} else {
				const loginStatus = LoginStorage.Load();
				if (loginStatus.isLoggedIn) {
					this.loginStatus = loginStatus;
					return true;
				}
			}
			return false;
		},

		setRedirectPath(redirectPath: string) {
			this.redirectPath = redirectPath;
		},

		getRedirectPath() {
			return this.redirectPath;
		},

		setLoading(loadStatus: boolean) {
			this.loading = loadStatus;
		},

		isLoading() {
			return this.loading;
		},
	},
});

export type LoginStoreType = ReturnType<typeof useLoginStore>;

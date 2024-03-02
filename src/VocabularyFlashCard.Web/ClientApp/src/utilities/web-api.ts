import { AppPaths, AppSetttings } from "./config";
import { type LoginStoreType, useLoginStore } from "@/stores/Login/Login";
import { LoginStorage } from "@/stores/Login/LoginStorage";
import router from "@/router";

export class WebApi {
	private static serverUrl = import.meta.env.VITE_API_ROOT; //process.env.API_ROOT;
	private static loadingHandler: number;
	private static store: LoginStoreType;

	private static buildUrl(url: string) {
		return `${this.serverUrl}${url}`;
	}

	public static async Do<Type>(
		apiMethod: ApiMethod,
		url = "",
		data?: object | null,
		getRawResponse = false,
	): Promise<Type> {
		if (!this.store) {
			this.store = useLoginStore();
		}
		this.showLoading();
		let responseObject;

		try {
			const requestOptions = this.initOptions(apiMethod, data);
			const response = await fetch(this.buildUrl(url), requestOptions);

			if (!response.ok) {
				// Unauthorized
				if (response.status >= 400 && response.status < 500) {
					console.log("Unauthorized, response code:", response.status);
					this.store.setLoggedOut();
					router.push(AppPaths.Login);
				}

				console.log(
					`Response error: ${response.status} ${response.statusText}`,
				);
			}

			// Reponse is not json then return raw response
			const contentType = response.headers.get("content-type");
			if (
				contentType === null ||
				contentType.indexOf("application/json") === -1
			) {
				getRawResponse = true;
			}

			responseObject = getRawResponse ? response : await response.json();
		} catch (err) {
			console.log({ err });
		} finally {
			this.hideLoading();
		}
		return responseObject;
	}

	private static initOptions(apiMethod: ApiMethod, data?: object | null) {
		const header = new Headers();

		const bearer = this.getBearer();
		if (bearer && bearer.length > 0) {
			header.append("Authorization", `Bearer ${bearer}`);
		}

		const options: RequestInit = {
			method: apiMethod,
		};

		if (data) {
			header.append("Content-Type", "application/json");
			options.body = JSON.stringify(data);
		}

		options.headers = header;

		return options;
	}

	private static showLoading() {
		this.loadingHandler = setTimeout(() => {
			this.store.setLoading(true);
		}, AppSetttings.LoadingNotificationDelay);
	}

	private static hideLoading() {
		clearTimeout(this.loadingHandler);
		this.store.setLoading(false);
	}

	private static getBearer() {
		let bearer = this.store.loginStatus.bearer;
		if (!bearer) {
			bearer = LoginStorage.Load()?.bearer ?? "";
		}
		return bearer;
	}
}

export enum ApiMethod {
	Get = "GET",
	Post = "POST",
	Put = "PUT",
	Patch = "PATCH",
	Delete = "DELETE",
}

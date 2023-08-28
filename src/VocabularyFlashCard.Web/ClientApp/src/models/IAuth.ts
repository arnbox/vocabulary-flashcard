export interface IAuth {
	isLoggedIn: boolean;
	email: string;
	id: string;
	bearer: string;
}

export const defaultAuth: IAuth = {
	isLoggedIn: false,
	email: "",
	id: "",
	bearer: "",
};

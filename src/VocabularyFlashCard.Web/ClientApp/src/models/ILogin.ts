export interface ILogin {
	email: string;
	password: string;
}

export const defaultLogin: ILogin = {
	email: "",
	password: "",
};

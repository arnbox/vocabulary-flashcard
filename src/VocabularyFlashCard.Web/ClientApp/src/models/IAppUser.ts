export interface IAppUser {
	id: string;
	userName: string;
	email: string;
	password: string;
}

export const defaultAppUser: IAppUser = {
	id: "",
	userName: "",
	email: "",
	password: "",
};

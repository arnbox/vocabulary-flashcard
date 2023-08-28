export interface IReminder {
	queItem: number;
	group: number;
}

export const defaultReminder: IReminder = {
	queItem: 0,
	group: 1,
};

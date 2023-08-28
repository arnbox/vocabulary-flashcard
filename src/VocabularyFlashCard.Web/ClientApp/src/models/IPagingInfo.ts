export interface IPagingInfo {
	totalItems: number;
	itemsPerPage: number;
	currentPage: number;
	searchPhrase: string;
	isDifficultWordList: boolean;
	totalPages: number;
}

export const defaultPagingInfo: IPagingInfo = {
	totalItems: 0,
	itemsPerPage: 0,
	currentPage: 0,
	searchPhrase: "",
	isDifficultWordList: false,
	totalPages: 0,
};

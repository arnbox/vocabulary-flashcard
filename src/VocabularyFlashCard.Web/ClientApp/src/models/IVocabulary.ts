import type { IVocabularyMedia } from "./IVocabularyMedia";

export interface IVocabulary {
	vocabularyId: number;
	word: string;
	definition: string;
	numRepeat: number;
	groupId: number;
	lastRead: string;
	marked: boolean;
	vocabularyMedia: IVocabularyMedia[];
}

export const defaultVocabulary: IVocabulary = {
	vocabularyId: 0,
	word: "",
	definition: "",
	numRepeat: 0,
	groupId: 0,
	lastRead: "",
	marked: false,
	vocabularyMedia: [],
};

import { MediaCategory } from "./MediaCategory";

export interface IVocabularyMedia {
	vocabularyMediaId: number;
	vocabularyId: number;
	mediaText: string;
	mediaCategory: MediaCategory;
	fileName: string;
	fileData: string;
}

export const defaultVocabularyMedia: IVocabularyMedia = {
	vocabularyMediaId: 0,
	vocabularyId: 0,
	mediaText: "",
	mediaCategory: MediaCategory.Extra,
	fileName: "",
	fileData: "",
};

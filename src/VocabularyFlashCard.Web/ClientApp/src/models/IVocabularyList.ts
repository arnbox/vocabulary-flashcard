import { type IPagingInfo, defaultPagingInfo } from "./IPagingInfo";
import type { IVocabulary } from "./IVocabulary";

export interface IVocabularyList {
	vocabularies: IVocabulary[];
	pagingInfo: IPagingInfo;
}

export const defaultVocabularyList: IVocabularyList = {
	vocabularies: [],
	pagingInfo: defaultPagingInfo,
};

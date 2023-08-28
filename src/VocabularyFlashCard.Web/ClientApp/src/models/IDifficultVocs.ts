import type { IVocabulary } from "./IVocabulary";

export interface IDifficultVocs {
	difficultVocabularies: IVocabulary[];
	markedVocabularies: IVocabulary[];
	lastReadVocabularies: IVocabulary[];
}

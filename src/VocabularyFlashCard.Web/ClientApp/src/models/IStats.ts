import { type IVocabulary, defaultVocabulary } from "./IVocabulary";

export interface IStats {
	groupCount: IGroupCount[];
	totalVocab: number;
	newWords: number;
	longestWord: IVocabulary;
	longestDefinition: IVocabulary;
	oldest: IVocabulary;
}

export interface IGroupCount {
	group: number;
	count: number;
	lastRead: string;
}

export const defaultStats: IStats = {
	groupCount: [],
	totalVocab: 0,
	newWords: 0,
	longestWord: defaultVocabulary,
	longestDefinition: defaultVocabulary,
	oldest: defaultVocabulary,
};

export const defaultGroupCount: IGroupCount = {
	group: 0,
	count: 0,
	lastRead: "",
};

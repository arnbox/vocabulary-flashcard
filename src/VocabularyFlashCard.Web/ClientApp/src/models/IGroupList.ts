export interface IGroupList {
	groupList: IVocabulariesGroup[];
}

export interface IVocabulariesGroup {
	groupId: number;
	vocabularies: IVocabularyItem[];
}

export interface IVocabularyItem {
	id: number;
	word: string;
	lastRead: string;
}

export const defaultGroupList: IGroupList = {
	groupList: [],
};

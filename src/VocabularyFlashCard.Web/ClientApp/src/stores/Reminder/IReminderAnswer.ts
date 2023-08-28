export interface IReminderAnswer {
	vocabularyId: number;
	knowIt: boolean;
	nextVocabularyGroupId: number;
}

export const defaultReminderAnswer: IReminderAnswer = {
	vocabularyId: 0,
	knowIt: false,
	nextVocabularyGroupId: 1,
};

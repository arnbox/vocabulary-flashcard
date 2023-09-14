/* Back-end WepApi endpoints */
export enum ApiUrls {
	Vocabularies = "Vocabularies",
	VocabularyMedia = "VocabularyMedia",
	MarkVocabulary = "Vocabularies/MarkVocabulary",
	NextVocabulary = "VocabularyReminder/NextVocabulary",
	Stats = "VocabularyStats",
	GroupList = "VocabularyGroupList",
	DifficultVocs = "Vocabularies/DifficultVocabularies",
	AllVocabulariesWord = "Vocabularies/VocabulariesWord",
	AppUser = "Users",
	Login = "Auth/Login",
	Logout = "Auth/Logout",
}

/* Application internal route paths */
export enum AppPaths {
	Home = "/",
	Reminder = "/Reminder",
	Stats = "/Stats",
	GroupList = "/GroupList",
	DifficultVocs = "/DifficultVocs",
	AppUser = "/AppUser",
	Login = "/Login",
}

export class AppSetttings {
	// Maximum total number of groups in Flashcard
	static MaxVocabularyGroup = 7;

	// in Millisecond
	static LaodingNotificationDelay = 450;

	// Internal number to define temporary Id for new vocabulary media
	static NewVocabularyMediaIdStart = 1000000000;
}

import { type IVocabulary, defaultVocabulary } from "@/models/IVocabulary";
import { AppSetttings } from "@/utilities/config";
import type { IReminderAnswer } from "./IReminderAnswer";
import { defaultReminder } from "./IReminder";
import { defineStore } from "pinia";

export const useReminderStore = defineStore("reminder", {
	state: () => {
		return {
			progress: defaultReminder,
			currentReminderVoc: defaultVocabulary,
			isInGroupExplorer: false,
			groupInExplorer: 0,
		};
	},
	actions: {
		updateProgress() {
			const p = this.progress;

			if (this.isInGroupExplorer) {
				p.queItem++;
				p.group = this.groupInExplorer === 0 ? 1 : this.groupInExplorer;
				return;
			}

			const numOfVocsToBeReviewedInGroup =
				AppSetttings.MaxVocabularyGroup - p.group + 1;

			if (p.queItem < numOfVocsToBeReviewedInGroup) {
				p.queItem++;
			} else {
				p.group = p.group == AppSetttings.MaxVocabularyGroup ? 1 : p.group + 1;
				p.queItem = 1;
			}
		},

		getRequestForNextVoc(answer: boolean): IReminderAnswer {
			this.updateProgress();
			const vocAnswer: IReminderAnswer = {
				vocabularyId: this.currentReminderVoc.vocabularyId,
				knowIt: answer,
				nextVocabularyGroupId: this.progress.group,
			};
			return vocAnswer;
		},

		setCurrentReminderVoc(voc: IVocabulary) {
			this.updateCurrentGroup(voc.groupId);
			this.currentReminderVoc = voc;
		},

		updateCurrentGroup(group: number) {
			// When no more vocabulary found in that group
			if (group !== this.progress.group) {
				this.progress.group = group;
				this.progress.queItem = 0;
				this.disableExplorer();
			}
			//this.updateProgress();
		},

		enableExplorer(group: number) {
			this.isInGroupExplorer = true;
			this.groupInExplorer = group;
			this.currentReminderVoc = defaultVocabulary;

			this.progress.group = group === 0 ? 1 : group;
			this.progress.queItem = 0;
		},

		disableExplorer() {
			this.isInGroupExplorer = false;
		},
	},
});

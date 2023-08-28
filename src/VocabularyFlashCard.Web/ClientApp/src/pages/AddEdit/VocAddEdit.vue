<script lang="ts">
import {
	BIconJournalCheck,
	BIconJournalMinus,
	BIconJournalText,
	BIconJournals,
	BIconPlusLg,
} from "bootstrap-icons-vue";
import { type IVocabulary, defaultVocabulary } from "../../models/IVocabulary";
import {
	type IVocabularyMedia,
	defaultVocabularyMedia,
} from "../../models/IVocabularyMedia";
import { type PropType, defineComponent } from "vue";
import { AppSetttings } from "../../utilities/config";
import { MediaCategory } from "../../models/MediaCategory";
import MediumAddEdit from "./MediumAddEdit.vue";
import OkCancelButtons from "@/components/OkCancelButtons.vue";

export default defineComponent({
	components: {
		BIconJournalMinus,
		BIconJournalText,
		BIconJournals,
		BIconJournalCheck,
		BIconPlusLg,
		MediumAddEdit,
		OkCancelButtons,
	},
	props: {
		vocab: {
			type: Object as PropType<IVocabulary>,
			require: true,
			default() {
				return { ...defaultVocabulary };
			},
		},
	},

	emits: ["cancelButton", "saveButton"],

	data() {
		return {
			vocabulary: defaultVocabulary,
			newMediumId: AppSetttings.NewVocabularyMediaIdStart,
		};
	},

	mounted() {
		if (this.vocab) {
			this.vocabulary = this.vocab;
		}
	},

	methods: {
		handleDeleteMedium(medium: IVocabularyMedia) {
			const filtred = this.vocabulary.vocabularyMedia.filter(
				(m) => m.vocabularyMediaId !== medium.vocabularyMediaId,
			);
			this.vocabulary.vocabularyMedia = filtred;
		},

		handleAddMedium() {
			this.newMediumId++;

			const newMedium = {
				...defaultVocabularyMedia,
				vocabularyId: this.vocabulary.vocabularyId,
				vocabularyMediaId: this.newMediumId,
				mediaCategory: this.guessMediumCategory(),
			};

			this.vocabulary.vocabularyMedia.push(newMedium);
		},

		guessMediumCategory() {
			const m = this.vocabulary.vocabularyMedia;
			if (m.some((c) => c.mediaCategory === MediaCategory.Main)) {
				return MediaCategory.Extra;
			}
			return MediaCategory.Main;
		},

		isNotNew() {
			return (this.vocabulary?.vocabularyId ?? 0) !== 0;
		},
	},
});
</script>

<template>
	<div class="row">
		<div class="col-lg-4">
			<label for="word" class="form-label">Word:</label>
			<div class="input-group">
				<span class="input-group-text">
					<BIconJournalMinus />
				</span>
				<input
					id="word"
					v-model.trim="vocabulary.word"
					type="text"
					name="word"
					class="form-control"
				/>
			</div>
		</div>
		<div class="col-lg-8">
			<label for="definition" class="form-label">Definition:</label>
			<div class="input-group">
				<span class="input-group-text">
					<BIconJournalText />
				</span>
				<textarea
					id="definition"
					v-model.trim="vocabulary.definition"
					name="definition"
					class="form-control"
					aria-label="Definition"
				/>
			</div>
		</div>
	</div>
	<!-- end of row -->
	<div v-show="isNotNew()" class="row mt-2">
		<!-- Group -->
		<div class="col-lg-4">
			<label class="form-label" for="group">Group:</label>
			<div class="input-group">
				<span class="input-group-text">
					<BIconJournals />
				</span>
				<select
					id="group"
					v-model="vocabulary.groupId"
					class="form-select"
					name="group"
				>
					<option v-for="groupCount in 7" :key="groupCount" :value="groupCount">
						Group {{ groupCount }}
					</option>
				</select>
			</div>
		</div>
		<!-- Is marked -->
		<div class="col-lg-8">
			<label class="form-label" for="marked">Marked:</label>
			<div class="input-group">
				<span class="input-group-text">
					<BIconJournalCheck />
				</span>
				<div class="form-check form-switch ms-2">
					<input
						id="marked"
						v-model="vocabulary.marked"
						class="form-check-input"
						type="checkbox"
						role="switch"
						name="marked"
					/>
					<label class="form-check-label" for="marked">
						{{ vocabulary.marked ? "Marked" : "Unmarked" }}
					</label>
				</div>
			</div>
		</div>
	</div>
	<!-- end of row -->

	<!-- add sound file button -->
	<div class="row m-4">
		<div class="col-lg-2 offset-lg-5">
			<button class="btn btn-primary col-lg-12" @click="handleAddMedium">
				<BIconPlusLg /> Add sound
			</button>
		</div>
	</div>

	<!-- Media files -->
	<div
		v-for="medium in vocabulary.vocabularyMedia"
		:key="medium.vocabularyMediaId"
		class="mt-3"
	>
		<MediumAddEdit
			:voc-medium="medium"
			@delete-sound="(medium: IVocabularyMedia) => handleDeleteMedium(medium)"
		/>
	</div>

	<OkCancelButtons
		@ok-click="$emit('saveButton', vocabulary)"
		@cancel-click="$emit('cancelButton')"
	/>
</template>

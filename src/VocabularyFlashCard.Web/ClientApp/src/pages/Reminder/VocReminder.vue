<script lang="ts">
import { ApiMethod, WebApi } from "@/utilities/web-api";
import { ApiUrls, AppPaths } from "@/utilities/config";
import { type IVocabulary, defaultVocabulary } from "@/models/IVocabulary";
import { BIconBoxArrowInUpRight } from "bootstrap-icons-vue";
import FormatDefinition from "../List/FormatDefinition.vue";
import type { IVocabularyMarked } from "@/models/IVocabularyMarked";
import { MediaCategory } from "@/models/MediaCategory";
import { Ts } from "@/utilities/ts-lib";
import VocMarked from "../List/VocMarked.vue";
import VocMedia from "../List/VocMedia.vue";
import VocMediaSample from "./VocMediaSample.vue";
import { defineComponent } from "vue";
import router from "@/router";
import { useReminderStore } from "@/stores/Reminder/Reminder";

export default defineComponent({
	components: {
		BIconBoxArrowInUpRight,
		VocMedia,
		VocMediaSample,
		FormatDefinition,
		VocMarked,
	},
	setup() {
		const store = useReminderStore();

		return {
			// you can return the whole store instance to use it in the template
			store,
		};
	},

	data() {
		return {
			voc: { ...defaultVocabulary },
			showMeaning: false,
			noButtonText: "No",
			nextText: "Ok, Next",
		};
	},

	computed: {
		sampleMedia() {
			return this.voc.vocabularyMedia.filter(
				(m) => m.mediaCategory === MediaCategory.Sample,
			);
		},
	},

	created() {
		if (this.store.currentReminderVoc.vocabularyId === 0) {
			this.getNextVocabulary(false);
		} else {
			this.voc = this.store.currentReminderVoc;
		}
	},

	methods: {
		async getNextVocabulary(knowIt: boolean) {
			const serverRequest = this.store.getRequestForNextVoc(knowIt);
			const url = `${ApiUrls.NextVocabulary}`;
			const voc = await WebApi.Do<IVocabulary>(
				ApiMethod.Post,
				url,
				serverRequest,
			);

			// Check for new word in explorer mode
			if (
				this.store.isInGroupExplorer &&
				this.store.groupInExplorer === 0 &&
				voc.numRepeat !== 0
			) {
				this.handleExitExplorer();
				return;
			}

			this.showMeaning = false;
			this.store.setCurrentReminderVoc(voc);
			this.voc = voc;
		},

		modifyMarkedVocab(vocMarked: IVocabularyMarked) {
			this.voc.marked = vocMarked.marked;
		},

		handleYesButton() {
			this.getNextVocabulary(true);
		},

		handleNoButton() {
			if (this.noButtonText === this.nextText || this.showMeaning) {
				this.getNextVocabulary(false);
				this.noButtonText = "No";
			} else {
				this.showMeaning = true;
				this.noButtonText = this.nextText;
			}
		},

		disabledYesButton() {
			return !this.showMeaning || this.noButtonText === this.nextText;
		},

		handleShowMeaning() {
			this.showMeaning = true;
		},

		handleExitExplorer() {
			this.store.disableExplorer();
			router.push({ path: AppPaths.GroupList });
		},

		showDateInfo(dateString: string) {
			return Ts.DateInfo(dateString);
		},
	},
});
</script>

<template>
	<!-- word -->
	<div class="row">
		<div class="card">
			<div class="card-body">
				<div>
					<VocMarked
						:marked="voc.marked"
						:vocabulary-id="voc.vocabularyId"
						@marked-vocabulary="(vm) => modifyMarkedVocab(vm)"
					/>
				</div>
				<div class="lead text-center">
					<h1>{{ voc.word }}</h1>
					<VocMedia :media="voc.vocabularyMedia" :auto-play="true" />
				</div>
			</div>
		</div>
	</div>

	<!-- Yes or No buttons -->
	<div class="row">
		<div class="col-lg-3 offset-lg-2 mt-4">
			<button class="btn btn-outline-danger col-12" @click="handleNoButton()">
				{{ noButtonText }}
			</button>
		</div>
		<div class="col-lg-3 offset-lg-2 mt-4">
			<button
				class="btn btn-outline-success col-12"
				:disabled="disabledYesButton()"
				@click="handleYesButton()"
			>
				Yes
			</button>
		</div>
	</div>

	<!-- Meaning -->
	<div class="row mt-4">
		<div class="card">
			<div class="card-body">
				<FormatDefinition v-if="showMeaning" :definition="voc.definition" />
				<div v-else class="col-lg-2 offset-lg-5">
					<button
						class="btn btn-outline-secondary col-12"
						@click="handleShowMeaning()"
						@mouseenter="handleShowMeaning()"
					>
						Show meaning
					</button>
				</div>
			</div>
		</div>
	</div>

	<!-- Sample media -->
	<div v-if="sampleMedia.length" class="row mt-4">
		<div class="card">
			<div class="card-body">
				<h5 class="card-title text-secondary">Samples:</h5>
				<VocMediaSample :media="sampleMedia" />
			</div>
		</div>
	</div>

	<!-- Group Explorer -->
	<div v-if="store.isInGroupExplorer" class="row mt-4">
		<button
			class="btn btn-outline-secondary col-12"
			@click="handleExitExplorer()"
		>
			Exit Explorer <BIconBoxArrowInUpRight />
		</button>
	</div>

	<!-- Vocabulary info -->
	<div class="row mt-4">
		<div class="card">
			<div class="card-body">
				<div class="row">
					<div class="col-md-4">
						<strong class="text-primary">Group: </strong>
						<span>{{ store.progress.group }}</span>
					</div>
					<div class="col-md-4">
						<strong class="text-primary">Item: </strong>
						<span>{{ store.progress.queItem }}</span>
					</div>
					<div class="col-md-4">
						<strong class="text-primary">Id: </strong>
						<span>{{ voc.vocabularyId }}</span>
					</div>
				</div>
				<div class="row">
					<div class="col-md-8">
						<strong class="text-primary">Last read: </strong>
						<span>{{ showDateInfo(voc.lastRead) }}</span>
					</div>
					<div class="col-md-4">
						<strong class="text-primary">Repeats: </strong>
						<span>{{ voc.numRepeat }}</span>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

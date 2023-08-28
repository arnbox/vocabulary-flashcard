<script lang="ts">
import { ApiUrls, AppSetttings } from "../../utilities/config";
import {
	BIconFileArrowUp,
	BIconFileText,
	BIconVolumeUpFill,
} from "bootstrap-icons-vue";
import {
	type IVocabularyMedia,
	defaultVocabularyMedia,
} from "../../models/IVocabularyMedia";
import { type PropType, defineComponent } from "vue";
import { Ts } from "../../utilities/ts-lib";

export default defineComponent({
	components: {
		BIconFileArrowUp,
		BIconFileText,
		BIconVolumeUpFill,
	},
	props: {
		vocMedium: {
			type: Object as PropType<IVocabularyMedia>,
			require: true,
			default: defaultVocabularyMedia,
		},
	},

	emits: ["deleteSound"],

	data() {
		return {
			medium: defaultVocabularyMedia,
		};
	},

	computed: {
		// stringMediaCat() {
		// 	return this.medium.mediaCategory.toString();
		// },

		isNew(): boolean {
			return (
				this.medium.vocabularyMediaId === 0 ||
				this.medium.vocabularyMediaId >= AppSetttings.NewVocabularyMediaIdStart
			);
		},
	},

	mounted() {
		this.medium = this.vocMedium;
	},

	methods: {
		mediaPath(medium: IVocabularyMedia): string {
			if (this.isNew) {
				return "";
			}

			const serverUrl = import.meta.env.VITE_API_ROOT;
			return `${serverUrl}${ApiUrls.VocabularyMedia}/${medium.vocabularyMediaId}`;
		},

		getRadioCategoryName(idCounter = "") {
			const id = this.getMediumId();
			const name = `mediaCategory${id}${idCounter}`;
			return name;
		},

		getMediumId() {
			return this.vocMedium.vocabularyMediaId.toString();
		},

		getFileUploadId() {
			const id = this.getMediumId();
			const name = `fileUpload${id}`;
			return name;
		},

		getAudioElementId(id: string = "") {
			if (!id) {
				id = this.getMediumId();
			}
			const name = `audio${id}`;
			return name;
		},

		deleteMedium() {
			let result = confirm("Are you sure to delete this sound?");
			if (result) {
				this.$emit("deleteSound", this.medium);
			}
		},

		async processFile(event: Event) {
			const el = event?.target as HTMLInputElement;
			const soundFile = el?.files?.item(0) as File;
			if (soundFile) {
				this.medium.fileName = soundFile.name;
				this.medium.fileData = await Ts.FileToBase64(soundFile);
				await this.setSoundToPlayer(el.id, soundFile);
			}
		},

		async setSoundToPlayer(fileUploadId: string, soundFile: File) {
			const id = Ts.extractNumbers(fileUploadId);
			const audioId = this.getAudioElementId(id);
			const audio = document?.querySelector(`#${audioId}`) as HTMLAudioElement;
			if (audio) {
				audio.src = await Ts.FileToBase64(soundFile, false);
			}
		},
	},
});
</script>

<template>
	<div class="card p-1">
		<!-- header (title and delete button) -->
		<div :class="[isNew ? 'text-bg-primary' : 'text-bg-light', 'card-header']">
			<div class="d-flex justify-content-between">
				<div v-if="isNew"><BIconVolumeUpFill /> New sound</div>
				<div v-else>Sound # {{ medium.vocabularyMediaId }}</div>
				<div>
					<button
						type="button"
						class="btn-close"
						aria-label="Close"
						title="Delete sound"
						@click="deleteMedium()"
					/>
				</div>
			</div>
		</div>

		<div class="card-body">
			<div class="row">
				<!-- media preview -->
				<div class="col-lg-4">
					<audio :id="getAudioElementId()" controls :title="medium.fileName">
						<source :src="mediaPath(medium)" type="audio/mpeg" />
						Your browser does not support the audio element.
					</audio>
				</div>

				<!-- file upload -->
				<div class="col-lg-8">
					<div class="input-group">
						<label class="input-group-text" :for="getFileUploadId()">
							<BIconFileArrowUp />
						</label>
						<input
							:id="getFileUploadId()"
							type="file"
							class="form-control"
							@change="processFile"
						/>
					</div>
				</div>
			</div>

			<!-- second row -->
			<div class="row">
				<!-- media category -->
				<div class="col-lg-4 mt-3">
					<div class="form-check form-check-inline">
						<input
							:id="getRadioCategoryName('1')"
							v-model="medium.mediaCategory"
							type="radio"
							class="form-check-input"
							:name="getRadioCategoryName()"
							:value="0"
						/>
						<label class="form-check-label" :for="getRadioCategoryName('1')">
							<sup>
								<span class="text-primary"><BIconVolumeUpFill /></span>
							</sup>
							Main
						</label>
					</div>
					<div class="form-check form-check-inline">
						<input
							:id="getRadioCategoryName('2')"
							v-model="medium.mediaCategory"
							type="radio"
							class="form-check-input"
							:name="getRadioCategoryName()"
							:value="1"
						/>
						<label class="form-check-label" :for="getRadioCategoryName('2')">
							<sup>
								<span class="text-danger"><BIconVolumeUpFill /></span>
							</sup>
							Extra
						</label>
					</div>
					<div class="form-check form-check-inline">
						<input
							:id="getRadioCategoryName('3')"
							v-model="medium.mediaCategory"
							type="radio"
							class="form-check-input"
							:name="getRadioCategoryName()"
							:value="2"
						/>
						<label class="form-check-label" :for="getRadioCategoryName('3')">
							<sup>
								<span class="text-success"><BIconVolumeUpFill /></span>
							</sup>
							Sample
						</label>
					</div>
				</div>

				<!-- media text -->
				<div class="col-lg-8">
					<div class="input-group mt-2">
						<span id="basic-addon1" class="input-group-text">
							<BIconFileText />
						</span>
						<input
							v-model="medium.mediaText"
							type="text"
							class="form-control"
						/>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

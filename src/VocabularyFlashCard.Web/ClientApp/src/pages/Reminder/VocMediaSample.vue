<script lang="ts">
import {
	type IVocabularyMedia,
	defaultVocabularyMedia,
} from "../../models/IVocabularyMedia";
import { type PropType, defineComponent } from "vue";
import { ApiUrls } from "../../utilities/config";
import { BIconVolumeUpFill } from "bootstrap-icons-vue";

export default defineComponent({
	components: { BIconVolumeUpFill },
	props: {
		media: {
			type: Array as PropType<IVocabularyMedia[]>,
			require: true,
			default() {
				return [{ ...defaultVocabularyMedia }];
			},
		},
	},

	methods: {
		getMediaPath(medium: IVocabularyMedia): string {
			const serverUrl = import.meta.env.VITE_API_ROOT;
			return `${serverUrl}${ApiUrls.VocabularyMedia}/${medium.vocabularyMediaId}`;
		},

		getTagMediumId(id: number) {
			return `sm${id}`;
		},

		playSound(audioId: string): void {
			const parent = this.$refs.sampleMediaDiv as HTMLElement;
			const audio = parent?.querySelector(`#${audioId}`) as HTMLAudioElement;
			if (audio && audio.nodeName === "AUDIO") {
				audio.play();
			}
		},
	},
});
</script>

<template>
	<div ref="sampleMediaDiv">
		<div v-for="medium in media" :key="medium.vocabularyMediaId">
			<span
				class="text-success h4 m-1 me-2"
				role="button"
				@click="playSound(getTagMediumId(medium.vocabularyMediaId))"
			>
				<BIconVolumeUpFill />
				<audio :id="getTagMediumId(medium.vocabularyMediaId)" preload="none">
					<source :src="getMediaPath(medium)" type="audio/mpeg" />
				</audio>
			</span>
			<span>{{ medium.mediaText }}</span>
		</div>
	</div>
</template>

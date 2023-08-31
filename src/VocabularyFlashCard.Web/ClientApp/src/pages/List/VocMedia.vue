<script lang="ts">
import {
	type IVocabularyMedia,
	defaultVocabularyMedia,
} from "../../models/IVocabularyMedia";
import { type PropType, defineComponent } from "vue";
import { ApiUrls } from "../../utilities/config";
import { BIconVolumeUpFill } from "bootstrap-icons-vue";
import { MediaCategory } from "../../models/MediaCategory";
import Tooltip from "bootstrap/js/dist/tooltip";

export enum MediaDisplay {
	All,
	Samples,
	WithouSamples,
}

interface IMediaTag extends IVocabularyMedia {
	tagId: string;
	mediaPath: string;
	shortKey: string;
	iconColor: string;
	title?: string;
	bootstrapTooltip?: string;
}

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

		autoPlay: {
			type: Boolean,
			default: false,
		},
	},

	data() {
		return {
			mediaTag: [] as IMediaTag[],
		};
	},

	watch: {
		media: {
			handler() {
				this.buildMediaTag();
			},
		},
	},

	created() {
		this.buildMediaTag();
	},

	beforeUpdate() {
		// to remove possible active tooltips
		this.hideAllTooltips();
	},

	updated() {
		this.checkAutoPlay();
	},

	beforeUnmount() {
		// to remove possible active tooltips
		this.hideAllTooltips();
	},

	mounted() {
		this.checkAutoPlay();
		window.addEventListener("keypress", (e) => {
			this.shortkeyToAudio(e);
		});
	},

	methods: {
		buildMediaTag() {
			this.mediaTag = [];
			return this.media.map((m, i) => {
				this.mediaTag.push({
					...m,
					tagId: this.getTagMediumId(m.vocabularyMediaId),
					mediaPath: this.getMediaPath(m),
					shortKey: `${i + 1}`,
					iconColor: this.iconColor(m.mediaCategory),
					title: m.mediaText.length > 0 ? m.mediaText : undefined,
					bootstrapTooltip: m.mediaText.length > 0 ? "tooltip" : undefined,
				});
			});
		},

		getMediaPath(medium: IVocabularyMedia): string {
			const serverUrl = import.meta.env.VITE_API_ROOT;
			return `${serverUrl}${ApiUrls.VocabularyMedia}/${medium.vocabularyMediaId}`;
		},

		iconColor(mediaCategory: MediaCategory) {
			let color: string;
			switch (mediaCategory) {
				case MediaCategory.Main:
					color = "text-primary";
					break;
				case MediaCategory.Extra:
					color = "text-danger";
					break;
				case MediaCategory.Sample:
					color = "text-success";
					break;
				default:
					color = "";
					break;
			}
			return color;
		},

		getTagMediumId(id: number) {
			return `m${id}`;
		},

		shortkeyToAudio(e: KeyboardEvent) {
			const med = this.mediaTag.find((m) => m.shortKey === e.key);
			if (med) {
				this.playSound(med?.tagId);
				this.showTooltip(med?.tagId);
			}
		},

		playSound(audioId: string): void {
			const parent = this.$refs.mediaDiv as HTMLElement;
			const audio = parent?.querySelector(`#${audioId}`) as HTMLAudioElement;
			if (audio && audio.nodeName === "AUDIO") {
				audio.play();
			}
		},

		async checkAutoPlay() {
			if (!this.$props.autoPlay) {
				return;
			}
			const parent = this.$refs.mediaDiv as HTMLElement;
			const audios = Array.from(parent.querySelectorAll("audio"));
			this.playAllSounds(audios, 0);
		},

		playAllSounds(sounds: HTMLAudioElement[], index: number) {
			const sound = sounds[index];

			if (sound) {
				this.showTooltip(sound.id);
				sound.addEventListener("ended", () => {
					index++;
					this.playAllSounds(sounds, index);
					sound.replaceWith(sound.cloneNode(true)); // To clear all event listener
				});
				sound.play();
			} else {
				this.hideAllTooltips();
			}
		},

		showTooltip(audioId: string) {
			const parent = this.$refs.mediaDiv as HTMLElement;
			const tooltipEl = parent?.querySelector(`span[data-play-id=${audioId}]`);
			if (tooltipEl) {
				const tooltip = Tooltip?.getOrCreateInstance(tooltipEl);
				tooltip?.show();
			}
		},

		hideAllTooltips() {
			const parent = this.$refs.mediaDiv as HTMLElement;
			const tooltipEls = parent?.querySelectorAll(`span[data-play-id]`);
			tooltipEls?.forEach(function (item) {
				const tooltip = Tooltip.getOrCreateInstance(item);
				tooltip.hide();
			});
		},
	},
});
</script>

<template>
	<div ref="mediaDiv">
		<span
			v-for="medium in mediaTag"
			:key="medium.vocabularyMediaId"
			v-tooltip
			:class="`${medium.iconColor} h2 m-1`"
			:title="medium.title"
			:data-bs-toggle="medium.bootstrapTooltip"
			:data-play-id="medium.tagId"
			role="button"
			@click="playSound(medium.tagId)"
		>
			<audio :id="medium.tagId" preload="none">
				<source :src="medium.mediaPath" type="audio/mpeg" />
			</audio>
			<BIconVolumeUpFill />
		</span>
	</div>
</template>

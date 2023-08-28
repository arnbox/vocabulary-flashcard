<script lang="ts">
import { ApiMethod, WebApi } from "../../utilities/web-api";
import { ApiUrls } from "../../utilities/config";
import { BIconStarFill } from "bootstrap-icons-vue";
import type { IVocabularyMarked } from "../../models/IVocabularyMarked";
import { defineComponent } from "vue";

export default defineComponent({
	components: { BIconStarFill },
	props: {
		marked: {
			type: Boolean,
			require: true,
			default: false,
		},
		vocabularyId: {
			type: Number,
			require: true,
			default: 0,
		},
	},

	emits: ["markedVocabulary"],

	computed: {
		getClass() {
			return this.marked ? "text-warning" : "text-secondary";
		},
	},

	methods: {
		async toggleMarked() {
			const markedVoc = await this.saveMarked();
			this.$emit("markedVocabulary", markedVoc);
		},

		async saveMarked() {
			const url = `${ApiUrls.MarkVocabulary}/${this.vocabularyId.toString()}`;
			const vocabularyMarked = await WebApi.Do<IVocabularyMarked>(
				ApiMethod.Post,
				url,
			);
			return vocabularyMarked;
		},
	},
});
</script>

<template>
	<span :class="getClass" role="button" @click="toggleMarked">
		<BIconStarFill />
	</span>
	<!-- <span v-if="marked" class="text-warning">
            <BIconStarFill></BIconStarFill>
        </span>
        <span v-else class="text-secondary">
            <BIconStar></BIconStar>
        </span> -->
</template>

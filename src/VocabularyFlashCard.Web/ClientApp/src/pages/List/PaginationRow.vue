<!-- <script lang="ts">
import { defineComponent, PropType } from "vue";

import { BIconPlusLg } from "bootstrap-icons-vue";

export default defineComponent({
	props: {
		currentPage: {
			type: Number,
			require: true,
			default: 0,
		},
		totalPage: {
			type: Number,
			require: true,
			default: 0,
		},
	},
	emits: ["pageChange"],

	components: {
		BIconPlusLg,
	},
});
</script>

<template>
	<div class="row">
		<div class="col-lg-6">
			<Pagination :currentPage="currentPage" :totalPage="totalPage"
			@pageChange="(page: number) => $emit("pageChange", page)" />
		</div>
		<div class="col-lg-6 text-lg-end">
			<AddVocabularyButton />
		</div>
	</div>
</template> -->

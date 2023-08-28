<script lang="ts">
import { BIconArrowRepeat, BIconCollection } from "bootstrap-icons-vue";
import { defineComponent } from "vue";

export default defineComponent({
	components: { BIconCollection, BIconArrowRepeat },
	props: {
		groupId: {
			type: Number,
			require: true,
			default: 0,
		},
		numRepeat: {
			type: Number,
			require: true,
			default: 0,
		},
	},
});
</script>

<template>
	<div class="">
		<div title="Group">
			<BIconCollection />
			<span class="text-primary ps-1">{{ groupId }}</span>
		</div>
		<div title="Number of repeat">
			<BIconArrowRepeat />
			<span class="text-warning ps-1">{{ numRepeat }}</span>
		</div>
	</div>
</template>

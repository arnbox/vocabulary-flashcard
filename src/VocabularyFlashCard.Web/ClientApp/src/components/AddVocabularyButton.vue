<script lang="ts">
import { BIconPlusLg } from "bootstrap-icons-vue";
import { defineComponent } from "vue";

export default defineComponent({
	components: {
		BIconPlusLg,
	},
	emits: ["addClick"],
});
</script>

<template>
	<button class="btn btn-primary" @click="$emit('addClick')">
		<slot><BIconPlusLg /> Add</slot>
	</button>
</template>

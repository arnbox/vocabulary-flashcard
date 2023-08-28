<!-- eslint-disable vue/no-v-html -->
<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
	props: {
		definition: {
			type: String,
			require: true,
			default: "",
		},
	},
	computed: {
		getFormatted() {
			let def = this.definition;
			def = def.replaceAll("(", "<strong>(");
			def = def.replaceAll(")", ")</strong>");

			def = def.replaceAll("[", '<span class="text-secondary">[');
			def = def.replaceAll("]", "]</span>");

			def = def.replaceAll(";", '<span class="text-danger">;</span>');

			def = def.replaceAll(" - ", '<span class="text-info"> - </span>');

			return def;
		},
	},
});
</script>
<template>
	<span v-html="getFormatted" />
</template>

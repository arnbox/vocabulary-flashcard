<script lang="ts">
import AppHeader from "./AppHeader.vue";
import { AppPaths } from "@/utilities/config";
import { defineComponent } from "vue";

export default defineComponent({
	components: {
		AppHeader,
	},

	computed: {
		currentRoute() {
			return this.$route.name;
		},
	},
	methods: {
		isLoginPage() {
			const loginRoute = AppPaths.Login.replaceAll("/", "");
			return this.currentRoute === loginRoute;
		},
	},
});
</script>

<template>
	<template v-if="isLoginPage()"><router-view /></template>
	<template v-else>
		<AppHeader />
		<!-- top spacer -->
		<h1 class="mb-5">Vocabularies falshcard</h1>

		<div class="row mb-4">
			<router-view />
		</div>

		<!-- Footer -->
		<hr />
		<div class="mb-3">Vocabularies falshcard 2022</div>
	</template>
</template>

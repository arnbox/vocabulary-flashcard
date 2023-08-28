<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
	emits: ["okClick", "cancelClick"],
});
</script>

<template>
	<div class="row m-4">
		<div class="col-lg-3 offset-lg-2 mt-lg-2">
			<button
				class="btn btn-outline-secondary col-12"
				@click="$emit('cancelClick')"
			>
				Cancel
			</button>
		</div>
		<div class="col-lg-3 offset-lg-2 mt-4 mt-lg-2">
			<button class="btn btn-outline-success col-12" @click="$emit('okClick')">
				Save
			</button>
		</div>
	</div>
</template>

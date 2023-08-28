<script lang="ts">
import { type IAppUser, defaultAppUser } from "@/models/IAppUser";
import { type PropType, defineComponent } from "vue";
import OkCancelButtons from "@/components/OkCancelButtons.vue";
export default defineComponent({
	components: { OkCancelButtons },
	props: {
		user: {
			type: Object as PropType<IAppUser>,
			require: true,
			default() {
				return { ...defaultAppUser };
			},
		},
	},

	emits: ["cancelButton", "saveButton"],

	data() {
		return {
			appUser: defaultAppUser,
		};
	},

	mounted() {
		if (this.user) {
			this.appUser = this.user;
		}
	},
});
</script>
<template>
	<div class="row">
		<div class="mb-3">
			<label for="user-name" class="form-label">Username:</label>
			<input
				id="user-name"
				v-model.trim="appUser.userName"
				type="text"
				class="form-control"
			/>
		</div>
		<div class="mb-3">
			<label for="email" class="form-label">Email:</label>
			<input
				id="email"
				v-model.trim="appUser.email"
				type="email"
				class="form-control"
			/>
		</div>
		<div class="mb-3">
			<label for="password" class="form-label"
				>Password:<span class="text-secondary">
					(Optional, in case you'd like to change it)</span
				></label
			>
			<input
				id="password"
				v-model.trim="appUser.password"
				type="password"
				class="form-control"
			/>
		</div>
	</div>
	<OkCancelButtons
		@ok-click="$emit('saveButton', appUser)"
		@cancel-click="$emit('cancelButton')"
	/>
</template>

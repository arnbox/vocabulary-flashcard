<script lang="ts">
import { ApiMethod, WebApi } from "@/utilities/web-api";
import {
	BIconKey,
	BIconPersonCheckFill,
	BIconShieldLock,
} from "bootstrap-icons-vue";
import { ApiUrls } from "@/utilities/config";
import type { IAuth } from "@/models/IAuth";
import { defaultLogin } from "@/models/ILogin";
import { defineComponent } from "vue";
import loginBackground from "@/assets/login-background.avif";
import { useLoginStore } from "@/stores/Login/Login";
export default defineComponent({
	components: { BIconShieldLock, BIconPersonCheckFill, BIconKey },

	setup() {
		const store = useLoginStore();
		return {
			store,
		};
	},

	data() {
		return {
			login: { ...defaultLogin },
			logginMessage: "",
		};
	},

	computed: {
		hasEmailAndPassword() {
			return this.login.email.length > 0 && this.login.password.length > 0;
		},
	},

	created() {
		// Set login page background
		document.body.style.backgroundImage = `url(${loginBackground})`;
		document.body.style.backgroundSize = "cover";
	},

	unmounted() {
		// Remove background from body
		document.body.style.backgroundImage = "none";
		document.body.style.backgroundSize = "auto";
	},

	methods: {
		async handlelogin() {
			const url = `${ApiUrls.Login}`;
			const auth = await WebApi.Do<IAuth>(ApiMethod.Post, url, this.login);

			const isLoggedIn = auth?.isLoggedIn ?? false;
			if (isLoggedIn) {
				const path = this.store.getRedirectPath();
				this.store.setLoggedIn(auth);
				this.store.setRedirectPath("");
				this.$router.push(path);
			} else {
				this.logginMessage = "Email or password is incorrect";
			}
		},
	},
});
</script>

<template>
	<form
		class="d-flex flex-column min-vh-100 justify-content-center align-items-center"
		@submit.prevent="handlelogin()"
	>
		<div class="card col-10 col-sm-8 col-md-5 col-lg-4 col-lx-3 col-xxl-3">
			<div class="card-body">
				<h1 class="text-center"><BIconShieldLock /></h1>
				<h4 class="h4 mb-3 fw-normal text-center">Vocabularies Flashcard</h4>
				<div class="mb-3">
					<label for="email"><BIconPersonCheckFill /> Username:</label>
					<input
						id="email"
						v-model="login.email"
						type="text"
						class="form-control"
					/>
				</div>
				<div class="mb-3">
					<label for="password"><BIconKey /> Password:</label>
					<input
						id="password"
						v-model="login.password"
						type="password"
						class="form-control"
					/>
				</div>
				<div v-if="logginMessage" class="text-danger mb-3">
					{{ logginMessage }}
				</div>
				<div class="mb-3">
					<button
						:disabled="!hasEmailAndPassword"
						class="w-100 btn btn-lg btn-primary"
						type="submit"
					>
						Sign in
						<div v-if="store.isLoading()" class="d-inline">
							...
							<div
								class="spinner-border spinner-border-sm"
								role="status"
								aria-hidden="true"
							></div>
						</div>
					</button>
				</div>
			</div>
		</div>
	</form>
</template>

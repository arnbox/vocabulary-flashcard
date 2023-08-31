<script lang="ts">
import {
	BIconBoxArrowRight,
	BIconCircleHalf,
	BIconFileEarmarkBarGraph,
	BIconFileEarmarkDiff,
	BIconFileEarmarkMedical,
	BIconFileEarmarkPerson,
	BIconFileEarmarkSpreadsheet,
	BIconFileEarmarkText,
	BIconMoonStarsFill,
	BIconPersonFill,
	BIconSunFill,
} from "bootstrap-icons-vue";
import AppLogo from "./AppLogo.vue";
import { AppPaths } from "@/utilities/config";
import Collapse from "bootstrap/js/dist/collapse";
import { ColorTheme } from "@/stores/AppSettings/ColorTheme";
import { defineComponent } from "vue";
import router from "@/router";
import { useLoginStore } from "@/stores/Login/Login";

export default defineComponent({
	components: {
		AppLogo,
		BIconFileEarmarkText,
		BIconCircleHalf,
		BIconMoonStarsFill,
		BIconSunFill,
		BIconFileEarmarkDiff,
		BIconFileEarmarkBarGraph,
		BIconFileEarmarkSpreadsheet,
		BIconFileEarmarkMedical,
		BIconFileEarmarkPerson,
		BIconBoxArrowRight,
		BIconPersonFill,
	},
	setup() {
		const store = useLoginStore();

		return {
			store,
		};
	},
	data() {
		return {
			appPaths: AppPaths,
		};
	},

	mounted() {
		const theme = ColorTheme.GetTheme();
		ColorTheme.HighLightActiveColorTheme(theme);
	},

	methods: {
		handleLogout() {
			this.store.setLoggedOut();
			router.push(AppPaths.Login);
		},

		handleColorChange(colorMode: string) {
			ColorTheme.SetTheme(colorMode);
		},

		closeNavBar(e: Event) {
			const menu = this.$refs.navmenu as HTMLElement;
			const el = e?.target as HTMLElement | null;

			if (menu && el) {
				// Skip if element is dropdown and has submenu
				if (
					!el.classList.contains("dropdown-toggle") &&
					el.dataset.subMenu !== "noclose"
				) {
					const collapse = Collapse.getOrCreateInstance(menu);
					collapse.hide(); // hide opened menu in mobile view
				}
			}
		},
	},
});
</script>

<template>
	<nav class="navbar fixed-top navbar-expand-lg navbar-dark bg-dark">
		<div class="container-fluid">
			<div class="d-flex align-items-center text-secondary me-2">
				<AppLogo />
				<span class="text-secondary ms-2 me-2"> Vocabularies flashcard </span>
				<div
					v-if="store.isLoading()"
					class="spinner-border ms-auto spinner-border-sm d-lg-none"
					role="status"
					aria-hidden="true"
				></div>
			</div>
			<button
				class="navbar-toggler"
				type="button"
				data-bs-toggle="collapse"
				data-bs-target="#navbarNav"
				aria-controls="navbarNav"
				aria-expanded="false"
				aria-label="Toggle navigation"
			>
				<span class="navbar-toggler-icon" />
			</button>
			<div
				id="navbarNav"
				ref="navmenu"
				class="collapse navbar-collapse"
				@click="closeNavBar"
			>
				<ul class="navbar-nav me-auto">
					<li class="nav-item me-lg-2">
						<!-- <a class="nav-link active" aria-current="page" href="#">List</a> -->
						<router-link to="/" class="nav-link d-flex align-items-center">
							<BIconFileEarmarkText />
							<span class="ms-1">List</span>
						</router-link>
					</li>
					<li class="nav-item me-lg-2">
						<router-link
							:to="appPaths.Reminder"
							class="nav-link d-flex align-items-center"
						>
							<BIconFileEarmarkDiff />
							<span class="ms-1">Reminder</span>
						</router-link>
					</li>
					<li class="nav-item me-lg-2">
						<router-link
							:to="appPaths.Stats"
							class="nav-link d-flex align-items-center"
						>
							<BIconFileEarmarkBarGraph />
							<span class="ms-1">Stats</span>
						</router-link>
					</li>
					<li class="nav-item me-lg-2">
						<router-link
							:to="appPaths.GroupList"
							class="nav-link d-flex align-items-center"
						>
							<BIconFileEarmarkSpreadsheet />
							<span class="ms-1">Group list</span>
						</router-link>
					</li>
					<li class="nav-item me-lg-2">
						<router-link
							:to="appPaths.DifficultVocs"
							class="nav-link d-flex align-items-center"
						>
							<BIconFileEarmarkMedical />
							<span class="ms-1">Difficult Words</span>
						</router-link>
					</li>
					<li class="nav-item me-lg-2">
						<router-link
							:to="appPaths.AppUser"
							class="nav-link d-flex align-items-center"
						>
							<BIconFileEarmarkPerson />
							<span class="ms-1">Users</span>
						</router-link>
					</li>
					<li class="nav-item dropdown">
						<a
							class="nav-link dropdown-toggle"
							href="#"
							role="button"
							data-bs-toggle="dropdown"
							aria-expanded="false"
						>
							<BIconCircleHalf />
							Color mode
						</a>
						<ul class="dropdown-menu">
							<li>
								<a
									class="dropdown-item"
									data-color-mode="auto"
									@click="handleColorChange('auto')"
								>
									<BIconCircleHalf /> Auto
								</a>
							</li>
							<li>
								<a
									class="dropdown-item"
									data-color-mode="light"
									@click="handleColorChange('light')"
								>
									<BIconSunFill /> Light
								</a>
							</li>
							<li>
								<a
									class="dropdown-item"
									data-color-mode="dark"
									@click="handleColorChange('dark')"
								>
									<BIconMoonStarsFill /> Dark
								</a>
							</li>
						</ul>
					</li>
				</ul>
				<div
					v-if="store.isLoading()"
					class="nav-item text-secondary ms-auto d-none d-lg-block"
				>
					<span class="me-2">Loading...</span>
					<div
						class="spinner-border ms-auto spinner-border-sm"
						role="status"
						aria-hidden="true"
					></div>
				</div>
				<div class="nav-item dropdown text-secondary ms-lg-5 me-lg-1">
					<a
						class="nav-link dropdown-toggle"
						href="#"
						role="button"
						data-bs-toggle="dropdown"
						aria-expanded="false"
					>
						<BIconPersonFill />
						<span data-sub-menu="noclose" class="ms-1">{{
							store.loginStatus.email
						}}</span>
					</a>
					<ul class="dropdown-menu bg-dark m-0">
						<li>
							<a
								class="dropdown-item bg-dark text-secondary"
								role="button"
								@click="handleLogout()"
							>
								<BIconBoxArrowRight /> Logout
							</a>
						</li>
					</ul>
				</div>
			</div>
		</div>
	</nav>
</template>

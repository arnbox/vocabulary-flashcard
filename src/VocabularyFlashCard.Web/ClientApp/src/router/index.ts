import {
	type RouteLocationNormalizedLoaded,
	createRouter,
	createWebHistory,
} from "vue-router";
import { AppPaths } from "@/utilities/config";
import AppStats from "@/pages/Stats/AppStats.vue";
import AppUser from "@/pages/AppUser/AppUser.vue";
// import { CheckAuthentication } from "./authentication";
import DifficultVocs from "@/pages/DifficultVocs/DifficultVocs.vue";
import GroupList from "@/pages/GroupList/GroupList.vue";
import LoginPage from "@/pages/Login/LoginPage.vue";
import VocList from "@/pages/List/VocList.vue";
import VocReminder from "@/pages/Reminder/VocReminder.vue";

// import Home from '@/components/Home/Home.vue';

const routes = [
	// set List route id params default value to 1
	{
		path: "/:id*",
		name: "List",
		component: VocList,
		props: (route: RouteLocationNormalizedLoaded) => ({
			id: route.params.id || "1",
		}),
	},
	{ path: AppPaths.Reminder, name: "Reminder", component: VocReminder },
	{ path: AppPaths.Stats, name: "Stats", component: AppStats },
	{ path: AppPaths.GroupList, name: "GroupList", component: GroupList },
	{
		path: AppPaths.DifficultVocs,
		name: "DifficultVocs",
		component: DifficultVocs,
	},
	{ path: AppPaths.AppUser, name: "AppUser", component: AppUser },
	{ path: AppPaths.Login, name: "Login", component: LoginPage },
];

const router = createRouter({
	history: createWebHistory(),
	linkActiveClass: "active",
	routes,
});

export default router;

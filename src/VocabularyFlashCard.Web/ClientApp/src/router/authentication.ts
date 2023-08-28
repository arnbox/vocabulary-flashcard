import { AppPaths } from "@/utilities/config";
import type { Router } from "vue-router";
import { useLoginStore } from "@/stores/Login/Login";

export function CheckAuthentication(router: Router) {
	const store = useLoginStore();
	router.beforeEach(function (to, from, next) {
		updateRedirectPath(to.path, store);
		if (to.path !== AppPaths.Login && !store.authenticated()) {
			next({ path: AppPaths.Login });
		} else if (to.path === AppPaths.Login && store.authenticated()) {
			next({ path: store.getRedirectPath() });
		} else {
			next();
		}
	});
}

// Is the path already exist in current defined paths
function isValidPath(path: string) {
	let p: keyof typeof AppPaths;
	for (p in AppPaths) {
		if (AppPaths[p] === path && AppPaths[p] !== AppPaths.Login) return true;
	}
	return false;
}

function updateRedirectPath(path: string, store: any) {
	if (isValidPath(path)) {
		store.setRedirectPath(path);
	} else if (store.getRedirectPath() === "") {
		store.setRedirectPath(AppPaths.Home);
	}
}

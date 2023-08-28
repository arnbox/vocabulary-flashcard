import "@/style/app.scss";
import "bootstrap/dist/js/bootstrap.js";
import { createApp, markRaw } from "vue";
import App from "./App.vue";
import { BootstrapTooltip } from "./components/BootstrapTooltip";
//import { CheckAuthentication } from "./router/authentication";
import { CheckAuthentication } from "./router/authentication";
import { createPinia } from "pinia";
import router from "./router";
import { ColorTheme } from "./stores/AppSettings/ColorTheme";

const pinia = createPinia();

pinia.use(({ store }) => {
	store.router = markRaw(router);
});

const app = createApp(App);
app.use(pinia);
app.use(router);

CheckAuthentication(router);

router.isReady().then(() => {
	app.mount("#app");
});

app.directive("tooltip", BootstrapTooltip);

ColorTheme.InitTheme();

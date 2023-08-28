import Tooltip from "bootstrap/js/dist/tooltip";

export const BootstrapTooltip = {
	mounted(el: HTMLElement) {
		new Tooltip(el);
	},
};

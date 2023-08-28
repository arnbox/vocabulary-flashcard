import { ColorMode } from "./ColorMode";
import { AppSetttingsStorage } from "./AppSettingsStorage";

export class ColorTheme {
    public static GetTheme() {
        let appSettings = AppSetttingsStorage.Load();
        this.#applyColorMode(appSettings?.colorMode);
        return appSettings?.colorMode;
    }

    public static SetTheme(colorMode: string, save = true, highLight = true) {
        this.#applyColorMode(colorMode);

        if (highLight) {
            this.HighLightActiveColorTheme(colorMode);
        }

        if (save) {
            let appSettings = AppSetttingsStorage.Load();
            appSettings.colorMode = colorMode;
            AppSetttingsStorage.Save(appSettings);
        }
    }

    public static InitTheme() {
        this.#registerColorSchemeChangeEvent()
        this.GetTheme();
    }

    // Highlight Active Color Theme in Menu
    public static HighLightActiveColorTheme(colorMode: string) {
        const selectedItem = document.querySelector(`a.dropdown-item[data-color-mode='${colorMode}']`);
        if (selectedItem) {
            // unselect all active items
            document.querySelectorAll("a.dropdown-item[data-color-mode]").forEach(element => {
                element.classList.remove("active");
                element.setAttribute("aria-pressed", "false");
            });

            // select active item
            selectedItem.classList.add("active");
        }
    }

    static #applyColorMode(colorMode: string) {
        switch (colorMode) {
            case ColorMode.Light:
                colorMode = ColorMode.Light;
                break;
            case ColorMode.Dark:
                colorMode = ColorMode.Dark;
                break;
            case ColorMode.Auto:
                colorMode = this.#getSystemColorMode();
                break;
            default:
                colorMode = this.#getSystemColorMode();
                break;
        }

        document.documentElement.dataset.bsTheme = colorMode;
    }

    static #getSystemColorMode(): string {
        return window?.matchMedia?.('(prefers-color-scheme: dark)')?.matches ? ColorMode.Dark : ColorMode.Light;
    }

    static #registerColorSchemeChangeEvent() {
        window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', event => {
            const newColorScheme = event.matches ? ColorMode.Dark : ColorMode.Light;
            const appSettings = AppSetttingsStorage.Load();
            if (appSettings?.colorMode === ColorMode.Auto) {
                this.SetTheme(newColorScheme, false, false);
            }
        });
    }
}
import { URL, fileURLToPath } from "node:url";
import { ViteMinifyPlugin } from "vite-plugin-minify";
import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
export default defineConfig({
    resolve: {
        alias: {
            "@": fileURLToPath(new URL("./src", import.meta.url)),
        },
    },
    css: {
        preprocessorOptions: {
            scss: {
            // additionalData: `@import "bootstrap/scss/bootstrap";`
            },
        },
    },
    plugins: [vue(), ViteMinifyPlugin({})],
});

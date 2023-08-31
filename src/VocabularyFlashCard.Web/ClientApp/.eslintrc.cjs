/* eslint-env node */
require("@rushstack/eslint-patch/modern-module-resolution");

module.exports = {
	root: true,
	extends: [
		"plugin:vue/vue3-recommended",
		"eslint:recommended",
		"@vue/eslint-config-typescript",
		"@vue/eslint-config-prettier/skip-formatting"
	],
	parserOptions: {
		ecmaVersion: "latest",
	},
	rules: {
		"vue/no-v-html": "off",
		"no-duplicate-imports": "error",
		"sort-imports": [
			"warn",
			{
				ignoreCase: false,
				ignoreDeclarationSort: false,
				ignoreMemberSort: false,
				memberSyntaxSortOrder: ["none", "all", "multiple", "single"],
				allowSeparatedGroups: false,
			},
		],
	},
};

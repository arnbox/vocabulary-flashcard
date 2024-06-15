<script lang="ts">
import { ApiMethod, WebApi } from "../../utilities/web-api";
import { type IVocabulary, defaultVocabulary } from "../../models/IVocabulary";
import {
	type IVocabularyList,
	defaultVocabularyList,
} from "../../models/IVocabularyList";
import AddVocabularyButton from "@/components/AddVocabularyButton.vue";
import { ApiUrls } from "../../utilities/config";
import { BIconSearch } from "bootstrap-icons-vue";
import type { IVocabularyMarked } from "../../models/IVocabularyMarked";
import ListPagination from "../../components/ListPagination.vue";
import VocAddEdit from "../AddEdit/VocAddEdit.vue";
import VocTable from "./VocTable.vue";
import { defineComponent } from "vue";
import router from "../../router";

export default defineComponent({
	components: {
		VocTable,
		ListPagination,
		VocAddEdit,
		AddVocabularyButton,
		BIconSearch,
	},
	data() {
		return {
			vocList: structuredClone(defaultVocabularyList) as IVocabularyList,
			vocShowAddEdit: false,
			vocSelected: { ...defaultVocabulary },
			suggestWords: [] as Array<string>,
			searchPhrase: "",
		};
	},

	computed: {
		pageId(): number {
			let page = parseInt(this.$route.params.id as string);
			if (isNaN(page) || page < 1) {
				page = 1;
				// update address of page
				router.replace({ name: "List", params: { id: page } });
			}
			return page;
		},

		currentPage(): number {
			return this.vocList.pagingInfo.currentPage;
		},

		totalPage(): number {
			return this.vocList.pagingInfo.totalPages;
		},
	},

	created() {
		this.updateVocab(this.pageId);
	},

	methods: {
		async getVocabs(page = 1): Promise<IVocabularyList> {
			const query = new URLSearchParams();
			query.append("page", page.toString());
			if (this.searchPhrase) {
				query.append("searchPhrase", this.searchPhrase);
			}

			const url = `${ApiUrls.Vocabularies}?${query.toString()}`;
			const vocs = await WebApi.Do<IVocabularyList>(ApiMethod.Get, url);
			return vocs;
		},

		async updateVocab(page = 1) {
			this.vocList = await this.getVocabs(page);
		},

		async handleSearchFocus() {
			// pre-fetch words to suggest as auto complete
			const url = ApiUrls.AllVocabulariesWord;
			const words = await WebApi.Do<Array<string>>(ApiMethod.Get, url);
			this.suggestWords = words;
		},

		modifyMarkedVocab(vocMarked: IVocabularyMarked) {
			const voc = this.vocList.vocabularies.find(
				(v: IVocabulary) => v.vocabularyId === vocMarked.vocabularyId,
			);
			if (voc) {
				voc.marked = vocMarked.marked;
			}
		},

		editVoc(voc: IVocabulary) {
			this.vocSelected = voc;
			this.vocShowAddEdit = true;
		},

		addVoc() {
			const newVoc = {
				...defaultVocabulary,
				definition: "() []",
				groupId: 1,
				vocabularyMedia: [],
			};
			this.editVoc(newVoc);
		},

		handleSearch() {
			const firstPage = 1;
			this.updateVocab(firstPage);
			// update address of page to the first page
			router.replace({ name: "List", params: { id: firstPage } });
		},

		cancelEdit() {
			this.vocSelected = defaultVocabulary;
			this.vocShowAddEdit = false;
		},

		async saveEdit(voc: IVocabulary) {
			const url = `${ApiUrls.Vocabularies}`;
			await WebApi.Do<IVocabulary>(ApiMethod.Post, url, voc);

			this.updateVocab(this.pageId);
			this.vocShowAddEdit = false;
		},
	},
});
</script>

<template>
	<div v-if="vocShowAddEdit">
		<VocAddEdit
			:vocab="vocSelected as IVocabulary & undefined"
			@cancel-button="cancelEdit()"
			@save-button="(voc: IVocabulary) => saveEdit(voc)"
		/>
	</div>
	<div v-else>
		<div class="row">
			<form class="col-lg-4" @submit.prevent="handleSearch">
				<div class="input-group mb-3">
					<span class="input-group-text">
						<BIconSearch />
					</span>
					<input
						v-model.trim="searchPhrase"
						type="search"
						list="suggest-words"
						class="form-control"
						@focus="handleSearchFocus"
						@input="handleSearch"
					/>
					<datalist id="suggest-words">
						<option
							v-for="word in suggestWords"
							:key="word"
							:value="word"
						></option>
					</datalist>
					<button class="btn btn-outline-secondary" type="submit">
						Search
					</button>
				</div>
			</form>
		</div>
		<div class="row">
			<div class="col-lg-6">
				<ListPagination
					:current-page="currentPage"
					:total-page="totalPage"
					@page-change="(page: number) => updateVocab(page)"
				/>
			</div>
			<div class="col-lg-6 text-lg-end">
				<AddVocabularyButton @add-click="() => addVoc()" />
			</div>
		</div>

		<div class="mt-2 mb-4">
			<VocTable
				:vocs="vocList.vocabularies"
				@marked-vocabulary="(vm) => modifyMarkedVocab(vm)"
				@edit-voc="(formData) => editVoc(formData)"
			/>
		</div>

		<div class="row">
			<div class="col-lg-6">
				<ListPagination
					:current-page="currentPage"
					:total-page="totalPage"
					@page-change="(page: number) => updateVocab(page)"
				/>
			</div>
			<div class="col-lg-6 text-lg-end">
				<AddVocabularyButton @add-click="() => addVoc()" />
			</div>
		</div>
	</div>
</template>

<script lang="ts">
import { ApiMethod, WebApi } from "@/utilities/web-api";
import {
	BIconAsterisk,
	BIconClockHistory,
	BIconStarFill,
} from "bootstrap-icons-vue";
import { type IVocabulary, defaultVocabulary } from "@/models/IVocabulary";
import { ApiUrls } from "@/utilities/config";
import type { IDifficultVocs } from "@/models/IDifficultVocs";
import type { IVocabularyMarked } from "@/models/IVocabularyMarked";
import Tab from "bootstrap/js/dist/tab";
import VocAddEdit from "@/pages/AddEdit/VocAddEdit.vue";
import VocTable from "@/pages/List/VocTable.vue";
import { defineComponent } from "vue";

export default defineComponent({
	components: {
		VocTable,
		VocAddEdit,
		BIconStarFill,
		BIconAsterisk,
		BIconClockHistory,
	},
	data() {
		return {
			difficultVocabularies: [] as IVocabulary[],
			markedVocabularies: [] as IVocabulary[],
			lastReadVocabularies: [] as IVocabulary[],
			vocShowAddEdit: false,
			vocSelected: { ...defaultVocabulary },
		};
	},

	created() {
		this.getVocabs();
	},

	mounted() {
		this.activeBootstrapTabs();
	},

	methods: {
		async getVocabs() {
			const url = `${ApiUrls.DifficultVocs}`;
			const vocs = await WebApi.Do<IDifficultVocs>(ApiMethod.Get, url);
			this.difficultVocabularies = vocs.difficultVocabularies;
			this.markedVocabularies = vocs.markedVocabularies;
			this.lastReadVocabularies = vocs.lastReadVocabularies;
		},

		modifyMarkedVocab(vocMarked: IVocabularyMarked, vocs: IVocabulary[]) {
			const voc = vocs.find(
				(v: IVocabulary) => v.vocabularyId === vocMarked.vocabularyId,
			);
			if (voc) {
				voc.marked = vocMarked.marked;
			}
		},

		cancelEdit() {
			this.vocSelected = defaultVocabulary;
			this.vocShowAddEdit = false;
		},

		async saveEdit(voc: IVocabulary) {
			const url = `${ApiUrls.Vocabularies}`;
			await WebApi.Do<IVocabulary>(ApiMethod.Post, url, voc);

			this.getVocabs();
			this.vocShowAddEdit = false;
		},

		editVoc(voc: IVocabulary) {
			this.vocSelected = voc;
			this.vocShowAddEdit = true;
		},

		activeBootstrapTabs() {
			const parent = this.$refs.mediaDiv as HTMLElement;
			if (parent) {
				const triggerTabList = [].slice.call(
					parent.querySelectorAll("#myTab button"),
				) as HTMLElement[];

				triggerTabList.forEach(function (triggerEl) {
					const tabTrigger = new Tab(triggerEl);

					triggerEl.addEventListener("click", function (event) {
						event.preventDefault();
						tabTrigger.show();
					});
				});
			}
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
		<ul id="myTab" ref="vocTabs" class="nav nav-tabs" role="tablist">
			<!-- difficult vocabularies tab -->
			<li class="nav-item" role="presentation">
				<button
					id="difficult-vocabularies-tab"
					class="nav-link active"
					data-bs-toggle="tab"
					data-bs-target="#difficult-vocabularies"
					type="button"
					role="tab"
					aria-controls="difficult-vocabularies"
					aria-selected="true"
				>
					<BIconAsterisk /> Difficult Vocabularies
				</button>
			</li>
			<!-- marked vocabularies tab -->
			<li class="nav-item" role="presentation">
				<button
					id="marked-vocabularies-tab"
					class="nav-link"
					data-bs-toggle="tab"
					data-bs-target="#marked-vocabularies"
					type="button"
					role="tab"
					aria-controls="marked-vocabularies"
					aria-selected="false"
				>
					<BIconStarFill /> Marked Vocabularies
				</button>
			</li>
			<!-- last read vocabularies tab -->
			<li class="nav-item" role="presentation">
				<button
					id="last-read-vocabularies-tab"
					class="nav-link"
					data-bs-toggle="tab"
					data-bs-target="#last-read-vocabularies"
					type="button"
					role="tab"
					aria-controls="last-read-vocabularies"
					aria-selected="false"
				>
					<BIconClockHistory /> Last Read Vocabularies
				</button>
			</li>
		</ul>
		<div id="myTabContent" class="tab-content">
			<div
				id="difficult-vocabularies"
				class="tab-pane fade show active"
				role="tabpanel"
				aria-labelledby="difficult-vocabularies-tab"
			>
				<h4 class="mt-3 text-secondary text-center">Difficult Vocabularies</h4>
				<VocTable
					:vocs="difficultVocabularies"
					@marked-vocabulary="
						(vm) => modifyMarkedVocab(vm, difficultVocabularies)
					"
					@edit-voc="(formData) => editVoc(formData)"
				/>
			</div>
			<div
				id="marked-vocabularies"
				class="tab-pane fade"
				role="tabpanel"
				aria-labelledby="marked-vocabularies-tab"
			>
				<h4 class="mt-3 text-secondary text-center">Marked Vocabularies</h4>
				<VocTable
					:vocs="markedVocabularies"
					@marked-vocabulary="(vm) => modifyMarkedVocab(vm, markedVocabularies)"
					@edit-voc="(formData) => editVoc(formData)"
				/>
			</div>
			<div
				id="last-read-vocabularies"
				class="tab-pane fade"
				role="tabpanel"
				aria-labelledby="last-read-vocabularies"
			>
				<h4 class="mt-3 text-secondary text-center">Last Read Vocabularies</h4>
				<VocTable
					:vocs="lastReadVocabularies"
					@marked-vocabulary="
						(vm) => modifyMarkedVocab(vm, lastReadVocabularies)
					"
					@edit-voc="(formData) => editVoc(formData)"
				/>
			</div>
		</div>
	</div>
</template>

<script lang="ts">
import { type IVocabulary, defaultVocabulary } from "../../models/IVocabulary";
import { type PropType, defineComponent } from "vue";
import FormatDefinition from "./FormatDefinition.vue";
import type { IVocabularyMarked } from "../../models/IVocabularyMarked";
import VocInfo from "./VocInfo.vue";
import VocMarked from "./VocMarked.vue";
import VocMedia from "./VocMedia.vue";

export default defineComponent({
	components: { VocMedia, VocInfo, VocMarked, FormatDefinition },
	props: {
		vocs: {
			type: Array as PropType<IVocabulary[]>,
			require: true,
			default() {
				return [{ ...defaultVocabulary }];
			},
		},
	},

	emits: ["markedVocabulary", "EditVoc"],

	methods: {
		modifyMarkedVocab(vocMarked: IVocabularyMarked) {
			this.$emit("markedVocabulary", vocMarked);
		},

		editVocab(voc: IVocabulary) {
			this.$emit("EditVoc", voc);
		},
		dateFormatter(date: string) {
			return date.substring(0, 16);
		},
	},
});
</script>
<template>
	<ul class="list-group d-none d-md-flex fw-bold">
		<li class="list-group-item active" aria-current="true">
			<div class="row">
				<div class="col-md-1">Id</div>
				<div class="col-md-2">Word</div>
				<div class="col-md-5">Definition</div>
				<div class="col-md-2">Media</div>
				<div class="col-md-1">LastRead</div>
				<div class="col-md-1">Edit</div>
			</div>
		</li>
	</ul>
	<ul class="list-group">
		<li v-for="voc in vocs" :key="voc.vocabularyId" class="list-group-item">
			<div class="row">
				<div class="col-md-1">
					<div class="float-start h-100 d-inline-block align-middle">
						{{ voc.vocabularyId }}
					</div>
					<div class="float-end h-100 d-inline-block d-flex flex-column">
						<VocInfo :group-id="voc.groupId" :num-repeat="voc.numRepeat" />
					</div>
				</div>

				<div class="col-md-2">
					<span class="fw-bold me-1">{{ voc.word }}</span>
					<VocMarked
						:marked="voc.marked"
						:vocabulary-id="voc.vocabularyId"
						@marked-vocabulary="(vm) => modifyMarkedVocab(vm)"
					/>
				</div>
				<div class="col-md-5">
					<FormatDefinition :definition="voc.definition" />
				</div>
				<div class="col-md-2">
					<template v-if="voc.vocabularyMedia.length > 0">
						<VocMedia :media="voc.vocabularyMedia" />
					</template>
				</div>
				<div class="col-md-1 small">
					{{ dateFormatter(voc.lastRead) }}
				</div>

				<div class="col-md-1">
					<button class="btn btn-outline-secondary" @click="editVocab(voc)">
						Edit
					</button>
				</div>
			</div>
		</li>
	</ul>
</template>

<script lang="ts">
import { ApiMethod, WebApi } from "@/utilities/web-api";
import { ApiUrls, AppPaths } from "@/utilities/config";
import { type IGroupList, defaultGroupList } from "@/models/IGroupList";
import { defineComponent } from "vue";
import router from "@/router";
import { useReminderStore } from "@/stores/Reminder/Reminder";

export default defineComponent({
	setup() {
		const store = useReminderStore();

		return {
			// you can return the whole store instance to use it in the template
			store,
		};
	},

	data() {
		return {
			groupList: { ...defaultGroupList },
		};
	},

	created() {
		this.getGroupList();
	},

	methods: {
		async getGroupList() {
			const url = `${ApiUrls.GroupList}`;
			const groupList = await WebApi.Do<IGroupList>(ApiMethod.Get, url);
			this.groupList = groupList;
		},

		groupHeader(group: number) {
			return group === 0 ? "New words" : `Group ${group}`;
		},

		setExplorerMode(group: number) {
			this.store.enableExplorer(group);
			router.push({ path: AppPaths.Reminder });
		},
	},
});
</script>

<template>
	<div
		v-for="group in groupList.groupList"
		:key="group.groupId"
		class="col-md-3 mt-4"
	>
		<button
			class="btn btn-outline-secondary mb-1"
			@click="setExplorerMode(group.groupId)"
		>
			Explore
			<span class="text-lowercase">{{ groupHeader(group.groupId) }}</span>
		</button>
		<h5
			:class="[
				group.groupId === 0 ? 'bg-info' : 'bg-primary',
				'text-white p-2',
			]"
		>
			{{ groupHeader(group.groupId) }} ({{ group.vocabularies.length }})
		</h5>

		<ul class="arn-group-list">
			<li v-for="voc in group.vocabularies" :key="voc.id">
				<em>{{ voc.id }}</em>
				<b>{{ voc.word }}</b>
				<span>{{ voc.lastRead.slice(0, 19) }}</span>
			</li>
		</ul>
	</div>
</template>

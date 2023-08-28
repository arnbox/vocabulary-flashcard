<script lang="ts">
import { ApiMethod, WebApi } from "@/utilities/web-api";
import { type IStats, defaultStats } from "@/models/IStats";
import { ApiUrls } from "@/utilities/config";
import { Ts } from "@/utilities/ts-lib";
import { defineComponent } from "vue";

export default defineComponent({
	components: {},
	data() {
		return {
			stats: defaultStats,
		};
	},

	created() {
		this.getStats();
	},

	methods: {
		async getStats() {
			const url = `${ApiUrls.Stats}`;
			const stats = await WebApi.Do<IStats>(ApiMethod.Get, url);
			this.stats = stats;
		},

		showDateInfo(dateString: string) {
			return Ts.DateInfo(dateString);
		},
	},
});
</script>

<template>
	<div class="bg-primary text-white p-2">
		<span class="navbar-brand ml-2">Statistics</span>
	</div>
	<table class="table table-bordered">
		<tbody>
			<tr>
				<th scope="row">Total</th>
				<td>{{ stats.totalVocab }}</td>
				<th class="text-center" rowspan="2" scope="column">
					<b>Oldest read</b>
				</th>
			</tr>
			<tr>
				<th scope="row">New Vocabulary</th>
				<td>{{ stats.newWords }}</td>
			</tr>

			<tr v-for="group in stats.groupCount" :key="group.group">
				<th scope="row">Group: {{ group.group }}</th>
				<td>{{ group.count }}</td>
				<td>{{ showDateInfo(group.lastRead) }}</td>
			</tr>

			<tr>
				<th scope="row">Longest Word</th>
				<td colspan="2">
					<dl>
						<dt>
							({{ stats.longestWord.vocabularyId }})
							{{ stats.longestWord.word }},
							<em>Length: {{ stats.longestWord.word.length }}</em>
						</dt>
						<dd class="fontfarsi">
							{{ stats.longestWord.definition }}
						</dd>
					</dl>
				</td>
			</tr>

			<tr>
				<th scope="row">Longest Definition</th>
				<td colspan="2">
					<dl>
						<dt>
							({{ stats.longestDefinition.vocabularyId }})
							{{ stats.longestDefinition.word }},
							<em>Length: {{ stats.longestDefinition.word.length }}</em>
						</dt>
						<dd class="fontfarsi">
							{{ stats.longestDefinition.definition }}
						</dd>
					</dl>
				</td>
			</tr>

			<tr>
				<th scope="row">Oldest Read</th>
				<td colspan="2">
					<dl>
						<dt>
							({{ stats.oldest.vocabularyId }}) {{ stats.oldest.word }}, Group:
							{{ stats.oldest.groupId }}
							<span>{{ showDateInfo(stats.oldest.lastRead) }}</span>
						</dt>
						<dd class="fontfarsi">{{ stats.oldest.definition }}</dd>
					</dl>
				</td>
			</tr>
		</tbody>
	</table>
</template>

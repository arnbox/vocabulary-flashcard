<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
	props: {
		currentPage: {
			type: Number,
			require: true,
			default: 0,
		},
		totalPage: {
			type: Number,
			require: true,
			default: 0,
		},
		nextText: {
			type: String,
			default: "&raquo;",
		},
		previousText: {
			type: String,
			default: "&laquo;",
		},
		middleText: {
			type: String,
			default: "...",
		},
	},

	emits: ["pageChange"],

	computed: {
		Links(): string[] {
			return this.getLinks(this.currentPage, this.totalPage);
		},

		nextPage(): number {
			return Math.min(this.currentPage + 1, this.totalPage);
		},
		previousPage(): number {
			return Math.max(this.currentPage - 1, 1);
		},
	},

	methods: {
		getLinks(current: number, total: number): string[] {
			if (total <= 1) {
				return ["1"];
			}
			const center = [
				current - 2,
				current - 1,
				current,
				current + 1,
				current + 2,
			];
			const filteredCenter = center
				.filter((p) => p > 1 && p < total)
				.map(String); // map(String) to convert to string
			const includeThreeLeft = current === 5;
			const includeThreeRight = current === total - 4;
			const includeLeftDots = current > 5;
			const includeRightDots = current < total - 4;

			if (includeThreeLeft) filteredCenter.unshift("2");
			if (includeThreeRight) filteredCenter.push((total - 1).toString());

			if (includeLeftDots) filteredCenter.unshift(this.middleText);
			if (includeRightDots) filteredCenter.push(this.middleText);

			return ["1", ...filteredCenter, total.toString()];
		},

		getClass(title: string): string {
			let textClass = "page-link ";

			// Active page class (highlight)
			if (title.toString() === this.currentPage?.toString()) {
				textClass += "active ";
			}

			// Disable click for link
			if (
				title === this.middleText ||
				(title === this.nextText && this.currentPage === this.totalPage) ||
				(title === this.previousText && this.currentPage === 1)
			) {
				textClass += "disabled ";
			}

			return textClass.trim();
		},

		clickedPage(event: Event) {
			if (event) {
				let el = event.target as HTMLElement;

				// find element that is link (anchor)
				while (el.nodeName !== "A") {
					el = el.parentNode as HTMLElement;
				}

				const pageNumber = el.dataset.pageNumber;

				this.$emit("pageChange", pageNumber);
			}
		},
	},
});
</script>
<template>
	<nav aria-label="Page navigation example">
		<ul class="pagination">
			<li class="page-item">
				<router-link
					:to="{ name: 'List', params: { id: previousPage } }"
					:class="getClass(previousText)"
					:data-page-number="previousPage"
					@click="clickedPage"
				>
					<span v-html="previousText" />
				</router-link>
			</li>
			<li
				v-for="(num, index) in Links"
				:key="`${num}I${index}`"
				class="page-item"
			>
				<router-link
					:to="{ name: 'List', params: { id: num } }"
					:class="getClass(num)"
					:data-page-number="num"
					@click="clickedPage"
				>
					{{ num }}
				</router-link>
			</li>
			<li class="page-item">
				<router-link
					:to="{ name: 'List', params: { id: nextPage } }"
					:class="getClass(nextText)"
					:data-page-number="nextPage"
					@click="clickedPage"
				>
					<span v-html="nextText" />
				</router-link>
			</li>
		</ul>
	</nav>
</template>

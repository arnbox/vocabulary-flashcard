<script lang="ts">
import { ApiMethod, WebApi } from "@/utilities/web-api";
import { type IAppUser, defaultAppUser } from "@/models/IAppUser";
import AddVocabularyButton from "@/components/AddVocabularyButton.vue";
import { ApiUrls } from "@/utilities/config";
import AppUserAddEdit from "./AppUserAddEdit.vue";
import { defineComponent } from "vue";

export default defineComponent({
	components: { AppUserAddEdit, AddVocabularyButton },
	data() {
		return {
			appUsers: [] as IAppUser[],
			selectedUser: { ...defaultAppUser },
			showEdit: false,
		};
	},
	created() {
		this.getUsers();
	},
	methods: {
		async getUsers() {
			const url = `${ApiUrls.AppUser}`;
			const users = await WebApi.Do<IAppUser[]>(ApiMethod.Get, url);
			this.appUsers = users;
		},

		editUser(user: IAppUser) {
			this.selectedUser = user;
			this.showEdit = true;
		},

		addUser() {
			const newUser = { ...defaultAppUser };
			this.editUser(newUser);
		},

		cancelEdit() {
			this.selectedUser = defaultAppUser;
			this.showEdit = false;
		},

		async saveEdit(user: IAppUser) {
			const url = `${ApiUrls.AppUser}`;
			await WebApi.Do<IAppUser>(ApiMethod.Post, url, user);

			this.getUsers();
			this.showEdit = false;
		},
	},
});
</script>
<template>
	<div v-if="showEdit">
		<AppUserAddEdit
			:user="selectedUser"
			@cancel-button="cancelEdit()"
			@save-button="(user: IAppUser) => saveEdit(user)"
		/>
	</div>
	<div v-else>
		<div class="row mt-0 mb-4 align-items-end">
			<div class="col-lg-12 text-lg-end">
				<AddVocabularyButton @add-click="() => addUser()" />
			</div>
		</div>
		<h4 class="text-secondary text-center">Users</h4>
		<table class="table table-responsive table-striped">
			<thead class="table-dark">
				<tr>
					<th scope="col">#</th>
					<th scope="col">Username</th>
					<th scope="col">Email</th>
					<th scope="col">Edit</th>
				</tr>
			</thead>
			<tbody>
				<tr v-for="(user, index) in appUsers" :key="user.id">
					<td>{{ index + 1 }}</td>
					<td>{{ user.userName }}</td>
					<td>{{ user.email }}</td>
					<td>
						<button class="btn btn-outline-secondary" @click="editUser(user)">
							Edit
						</button>
					</td>
				</tr>
			</tbody>
		</table>
	</div>
</template>

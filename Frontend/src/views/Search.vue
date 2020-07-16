<template>
  <div class="search">
    <!-- <form v-on:submit.prevent="search">
      <div class="input-group">
        <md-field class="input-group-field">
          <label>Search 123</label>
          <md-input v-model="query"></md-input>
        </md-field>
        <div class="input-group-button"><md-button class="md-raised" v-on:click="search"><md-icon>search</md-icon></md-button></div>
      </div>
    </form> -->
    <h2>Groups</h2>
    <md-table>
      <md-table-row>
        <md-table-head>Title</md-table-head>
        <md-table-head>Author</md-table-head>
        <md-table-head>Pub. Year</md-table-head>
        <md-table-head>View</md-table-head>
      </md-table-row>
      <md-table-row v-for="group in groups" v-bind:key="group.key">
        <md-table-cell>{{group.name}}</md-table-cell>
        <md-table-cell>{{group.meetingDate}}</md-table-cell>
        <md-table-cell md-numeric>{{group.location}}</md-table-cell>
        <md-table-cell><md-button v-on:click="viewDetails(group)"><md-icon>visibility</md-icon></md-button></md-table-cell>
      </md-table-row>
    </md-table>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import axios from 'axios';

@Component
export default class Search extends Vue {
  baseUrl = 'https://openlibrary.org';
  groups: any[];
  query = '';

  constructor() {
    super();

    this.groups = [
      {
        'name': 'Tuesday Tennis',
        meetingDate: 'Tue 8PM',
        location: 'STP'
      }
    ]
  }

  created() {
    
  }

  async search() {
    const response = await axios.get(this.baseUrl + `/search.json?name=${this.query}`);
    this.groups = await response.data.docs;
  }

  viewDetails(group: any) {
    this.$router.push({ path: 'details', query: {
      name: group.name,
      authors: group.meetingDate && group.meetingDate.join(', '),
      year: group.first_publish_year,
      cover_id: group.cover_edition_key
    }});
  }
}
</script>

<style>
.input-group {
  margin-top: 1rem;
  display: flex;
  justify-content: center;
}

.input-group-field {
  margin-right: 0;
}

.input-group .input-group-button {
  margin-left: 0;
  border: none;
}

.input-group .md-raised {
  margin-top: 0;
  margin-bottom: 0;
  border-radius: 0;
}
</style>
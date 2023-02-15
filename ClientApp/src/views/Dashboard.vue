<template>
  <v-container fluid class="pt-0">
    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-title>Help Information</v-card-title>
          <v-card-text>
            <p>
              This page displays all published templates available for a Demo
              company. These templates are retrieved through the Get Assets
              endpoint. They can be edited for your company
              <a
                href="https://studio.mycreativeshop.com/templates"
                target="_blank"
                >here</a
              >.
            </p>
            <p>
              <b>Start Designing</b>
              <br />
              When you click on "Start Designing", it will use the "id" from
              the Asset response and pass it to the Get Editor endpoint. This
              will open an Editor with the given template loaded.
            </p>
          </v-card-text>
        </v-card>
      </v-col>
      <v-col cols="12" sm="6" md="4" lg="3">
        <v-card height="100%">
          <v-card-title>
            Blank Editor
          </v-card-title>
          <v-card-text>
            <v-btn block :to="`/editor`">
              Start Designing
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
      <v-col cols="12" sm="6" md="4" lg="3" v-for="(item, i) in templates" :key="i">
        <v-card height="100%">
          <v-card-title>
            {{ item.name }}
          </v-card-title>
          <v-card-text>
            <v-img :src="item.thumbnailUrl" height="150" contain />
          </v-card-text>
          <v-card-text>
            <v-btn block :to="`/editor?templateId=${item.id}`">
              Start Designing
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: "Dashboard",
  components: {},
  data: () => ({
    loading: false,
    templates: [],
  }),
  computed: {},
  mounted() {
    this.$http.get(`/api/templates?userId=testuser`).then((data) => {
      this.templates = data.data.assets;
    });
  },
  methods: {},
};
</script>

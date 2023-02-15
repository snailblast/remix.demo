<template>
  <v-container fluid class="pt-0">
    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-title>Help Information</v-card-title>
          <v-card-text>
            <p>
              This page displays all designs created by a given user in the Demo
              company. These designs are retrieved through the Get Designs
              endpoint.
            </p>
            <p>
              <b>Edit</b>
              <br />
              When you click "Edit", it will use the "id" from the Design
              response and pass it to the Get Editor endpoint. This will open an
              Editor with the given design loaded. This design can then be
              edited and saved in the editor.
            </p>
            <p>
              <b>Delete</b>
              <br />
              When you click "Delete", it will use the "id" from the Design
              response and pass it to the Delete Design endpoint. This will mark
              the design as deleted and it will not longer be accessible.
            </p>
            <p>
              <b>Generate Pdf</b>
              <br />
              When you click "Generate Pdf", it will use the "id" from the
              Design response and pass it to the Get Design Output endpoint.
              This will generate a Pdf with a watermark and open it in a new
              window. This can be changed to generate a file without a watermark
              by specifying a "pdfType" that isn't "proof".
            </p>
          </v-card-text>
        </v-card>
      </v-col>
      <v-col cols="12" sm="6" md="4" lg="3" v-for="(item, i) in designs" :key="i">
        <v-card height="100%">
          <v-card-title>
            {{ item.name }}
          </v-card-title>
          <v-card-text>
            <v-img :src="item.thumbnailUrl" height="150" contain />
          </v-card-text>
          <v-card-text>
            <v-btn block :to="`/editor?designId=${item.id}`"> Edit </v-btn>
            <br />
            <v-btn block @click="deleteDesign(item.id)"> Delete </v-btn>
            <br />
            <v-btn block @click="generatePdf(item.id)"> Generate Pdf </v-btn>
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
    designs: [],
  }),
  computed: {},
  mounted() {
    this.$http.get(`/api/designs?userId=testuser`).then((data) => {
      this.designs = data.data;
    });
  },
  methods: {
    deleteDesign(id) {
      this.$http.delete(`/api/designs/${id}?userId=testuser`).then(() => {
        let index = this.designs.findIndex((x) => x.id == id);
        if (index >= 0) {
          this.designs.splice(index, 1);
        }
      });
    },
    generatePdf(id) {
      this.$http
        .get(`/api/designs/${id}/output/proof?userId=testuser`)
        .then((data) => {
          window.open(data.data.url, "_blank");
        });
    },
  },
};
</script>

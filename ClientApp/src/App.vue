<template>
  <v-app>
    <v-app-bar app>
      MyCreativeShop Remix API Demo
      <v-spacer />
      <v-btn text to="/">Templates</v-btn>
      <v-btn text to="/designs">Designs</v-btn>
      <v-btn
        text
        href="https://studio.mycreativeshop.com/settings/api"
        target="_blank"
        >API Docs</v-btn
      >
    </v-app-bar>
    <v-main>
      <v-container fluid style="height: 100%">
        <router-view :key="$route.fullPath" />
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
export default {
  name: "App",

  components: {},
  data: () => ({}),
  watch: {},
  computed: {},
  methods: {},
  created() {},
  mounted() {},
};
</script>

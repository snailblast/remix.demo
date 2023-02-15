<template>
  <v-row v-if="!loading" style="height: 100%">
    <v-col cols="8">
      <iframe
        v-if="iframeUrl"
        id="iframe"
        ref="iframe"
        :src="iframeUrl"
        frameborder="0"
      />
    </v-col>
    <v-col cols="4" style="calc(height:100vh - 64px);overflow-y:auto">
      <v-card>
        <v-card-title>Help Information</v-card-title>
        <v-card-text>
          <p>
            This page shows how an editor is integrated into a page with an
            iframe. As events from the iframe are sent to the consuming page,
            they are displayed in the table below. Clicking on a row will show
            you the entire event that is sent to the page. For a more in-depth
            look at the events and how they are consumed by a page, click
            <a
              href="https://studio.mycreativeshop.com/settings/api"
              target="_blank"
              >here</a
            >.
          </p>
        </v-card-text>
      </v-card>
      <v-data-table
        :headers="[
          {
            text: 'Events',
            sortable: false,
            value: 'data.type',
          },
          { text: '', value: 'data-table-expand' },
        ]"
        :items="events"
        single-expand
        :expanded.sync="expanded"
        show-expand
        hide-default-footer
        disabled-pagination
        item-key="id"
        @click:row="expandRow"
      >
        <template v-slot:header.data.type="{}">
          <v-row dense>
            <v-col class="my-auto">Events</v-col>
            <v-spacer />
            <v-col cols="auto">
              <v-btn text @click="postChildMessage('get_document-details')">
                Get Document Info
              </v-btn>
            </v-col>
          </v-row>
        </template>
        <template v-slot:expanded-item="{ headers, item }">
          <td :colspan="headers.length" style="overflow-wrap: anywhere">
            {{ item.data }}
          </td>
        </template>
      </v-data-table>
    </v-col>
  </v-row>
</template>

<script>
import { mapGetters } from "vuex";

export default {
  props: {},
  data() {
    return {
      loading: true,
      iframeUrl: "",
      saveDetails: false,
      events: [],
      expanded: [],
      count: 0,
      savedDesignId: null,
    };
  },
  computed: {
    designId() {
      return this.$route.query.designId || this.savedDesignId;
    },
    templateId() {
      return this.$route.query.templateId;
    },
    ...mapGetters({}),
  },
  mounted() {
    this.bindPostMessageHandlers();
    this.loadData();
  },
  beforeDestroy() {
    this.unbindPostMessageHandlers();
  },
  methods: {
    expandRow(val) {
      this.expanded = this.expanded.filter((x) => x.id == val.id).length
        ? []
        : [val];
    },
    bindPostMessageHandlers() {
      this.unbindPostMessageHandlers();
      const eventMethod = window.addEventListener
        ? "addEventListener"
        : "attachEvent";
      const eventer = window[eventMethod];
      const messageEvent =
        eventMethod === "attachEvent" ? "onmessage" : "message";

      // Listen to message from child window
      eventer(messageEvent, this.handlePostMessage, false);
    },
    handlePostMessage(e) {
      // console.log('handlepostmessage-initial', e.data)
      if (!e.data.type) {
        return;
      }
      switch (e.data.type) {
        case "editor_postmessage-ready":
          this.setAppConfig();
          break;
        case "editor_cta-click":
          // handle cta click
          break;
        case "editor_document-details":
          //handle document details
          break;
        case "editor_save-complete":
          this.savedDesignId = e.data.data.designId;
          this.setAppConfig();
          break;
        case "editor_exit":
          //handle document details
          break;
        default:
          return;
      }
      console.log("DEMO handlePostMessage", e.data);
      this.addEventToList(e);
    },
    addEventToList(e) {
      this.events.push({ id: this.count, data: e.data });
      this.count++;
      this.events = this.events.sort(function (a, b) {
        return b.id - a.id;
      });
    },
    unbindPostMessageHandlers() {
      const eventMethod = window.removeEventListener
        ? "removeEventListener"
        : "detachEvent";
      const eventer = window[eventMethod];
      const messageEvent =
        eventMethod === "detachEvent" ? "onmessage" : "message";

      eventer(messageEvent, this.handlePostMessage, false);
    },
    loadData() {
      this.loading = true;
      let params = {
        userId: "testuser",
        templateId: this.templateId || "none",
        designId: this.designId || "none",
      };
      this.$http
        .get(`/api/editor`, { params: params })
        .then((data) => {
          this.iframeUrl = data.data.url;
        })
        .catch((error) => {
          console.log("error", error);
        })
        .finally(() => {
          this.loading = false;
        });
    },
    postChildMessage(type, data) {
      this.$refs.iframe.contentWindow.postMessage({ type, data }, "*");
      console.log("DEMO postChildMessage", type, data);
    },
    exit() {
      this.$router.push({ path: "/" });
    },
    setAppConfig() {
      let showButton = !!this.designId;
      this.postChildMessage("set_app-config", {
        images: {
          enableStockPhotos: false,
          enableBrandImages: false,
          enableUserUploads: true,
        },
        ctaButton: { show: showButton, text: "Download", icon: "download" },
      });
    },
  },
};
</script>

<style lang="scss" scoped>
#iframe {
  // position: absolute;
  left: 0px;
  width: 100%;
  top: 0px;
  height: 100%;
}
</style>

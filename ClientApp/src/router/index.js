import Vue from "vue";
import VueRouter from "vue-router";
import Dashboard from "../views/Dashboard.vue";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    name: "dashboard",
    meta: { title: "Dashboard" },
    component: Dashboard
  },
  {
    path: "/editor",
    name: "editor",
    meta: { title: "Editor" },
    component: () =>
      import(/* webpackChunkName: "editor" */ "../views/Editor.vue")
  },
  {
    path: "/designs",
    name: "designs",
    meta: { title: "Designs" },
    component: () =>
      import(/* webpackChunkName: "designs" */ "../views/Designs.vue")
  }
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
});

export default router;

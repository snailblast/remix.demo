import axios from "axios";

const httpClient = axios.create({
  baseURL: '',
  timeout: 100000
});

export default httpClient;

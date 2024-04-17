import axios, { AxiosRequestConfig } from "axios";

export default class AxiosClient {
  static SUCCESS_STATUSES = [200, 201];
  static SERVER_ERROR = [500];

  api;

  constructor(config?: AxiosRequestConfig) {
    this.api = axios.create(config);
    this.api.defaults.headers.common["Content-Type"] = "application/json";
  }

  get = <T extends {}>(config: any) => {
    return this.api.get<T>(config.url, config.config);
  };

  post = <T extends {}>(config: any) => {
    return this.api.post<T>(config.url, config.data, config.config);
  };

  put = <T extends {}>(config: any) => {
    return this.api.put<T>(config.url, config.data, config.config);
  };

  delete = <T extends {}>(config: any) => {
    return this.api.delete<T>(config.url, config.config);
  };
}

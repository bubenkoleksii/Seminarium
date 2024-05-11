import { getCurrentUserToken } from './auth';
import axios, { AxiosRequestConfig } from 'axios';

const baseUrl = process.env.API_GATEWAY_URL;

async function getHeaders() {
  const token = await getCurrentUserToken();
  const headers: any = { 'Content-type': 'application/json' };

  if (token) {
    headers.Authorization = 'Bearer ' + token.access_token;
  }

  return headers;
}

async function get<T>(url: string) {
  const requestOptions: AxiosRequestConfig = {
    method: 'GET',
    headers: await getHeaders(),
  };

  return (await axios.get<T>(baseUrl + url, requestOptions)).data;
}

export const api = {
  get,
};

import type { ApiError } from '@/shared/types';
import { getCurrentUserToken } from './auth';
import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';

const baseUrl = process.env.API_GATEWAY_URL;

async function getHeaders() {
  const token = await getCurrentUserToken();
  const headers: any = { 'Content-type': 'application/json' };

  if (token) {
    headers.Authorization = 'Bearer ' + token.access_token;
  }

  return headers;
}

async function get(url: string) {
  const requestOptions = {
    method: 'GET',
    headers: await getHeaders(),
  };

  const response = await fetch(baseUrl + url, requestOptions);
  return await handleResponse(response);
}

async function handleResponse(response: Response) {
  const text = await response.text();

  let data;
  try {
    data = JSON.parse(text);
  } catch (error) {
    data = text;
  }

  if (response.ok) {
    return data || response.statusText;
  } else {
    const error: ApiError = {
      title: data.title,
      type: data.type,
      status: data.status,
      detail: data.detail,
      params: data.params,
      message: typeof data === 'string' && data.length > 0
        ? data
        : response.statusText
    }

    return { error };
  }
}

export const api = {
  get,
};

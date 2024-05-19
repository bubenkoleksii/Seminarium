import type { ApiError } from '@/shared/types';
import { getCurrentUserToken } from './auth';

const baseUrl = process.env.API_GATEWAY_URL;

async function get(url: string) {
  const requestOptions = {
    method: 'GET',
    headers: await getHeaders(),
  };

  const response = await fetch(baseUrl + url, requestOptions);
  return await handleResponse(response);
}

async function create(url: string, body: {}) {
  const requestOptions = {
    method: 'POST',
    headers: await getHeaders(),
    body: JSON.stringify(body),
  };

  const response = await fetch(baseUrl + url, requestOptions);
  return await handleResponse(response);
}

async function update(url: string, body: {}) {
  const requestOptions = {
    method: 'PUT',
    headers: await getHeaders(),
    body: JSON.stringify(body),
  };

  const response = await fetch(baseUrl + url, requestOptions);
  return await handleResponse(response);
}

async function partialUpdate(url: string, body: {}) {
  const requestOptions = {
    method: 'PATCH',
    headers: await getHeaders(),
    body: JSON.stringify(body),
  };

  const response = await fetch(baseUrl + url, requestOptions);
  return await handleResponse(response);
}

async function remove(url: string) {
  const requestOptions = {
    method: 'DELETE',
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
      message:
        typeof data === 'string' && data.length > 0
          ? data
          : response.statusText,
    };

    return { error };
  }
}

async function getHeaders() {
  const token = await getCurrentUserToken();
  const headers: any = { 'Content-type': 'application/json' };

  if (token) {
    headers.Authorization = 'Bearer ' + token.access_token;
  }

  return headers;
}

export const api = {
  get,
  create,
  update,
  partialUpdate,
  remove,
};

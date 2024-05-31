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

async function create(
  url: string,
  body: any,
  isFormData = false,
  schoolProfileId = null,
) {
  const requestOptions = {
    method: 'POST',
    headers: await getHeaders(isFormData, schoolProfileId),
    body: isFormData ? body : JSON.stringify(body),
  };

  const response = await fetch(baseUrl + url, requestOptions);
  return await handleResponse(response);
}

async function update(
  url: string,
  body: any,
  isFormData = false,
  schoolProfileId = null,
) {
  const requestOptions = {
    method: 'PUT',
    headers: await getHeaders(isFormData, schoolProfileId),
    body: isFormData ? body : JSON.stringify(body),
  };

  const response = await fetch(baseUrl + url, requestOptions);
  return await handleResponse(response);
}

async function partialUpdate(
  url: string,
  body: any,
  isFormData = false,
  schoolProfileId = null,
) {
  const requestOptions = {
    method: 'PATCH',
    headers: await getHeaders(isFormData, schoolProfileId),
    body: isFormData ? body : JSON.stringify(body),
  };

  const response = await fetch(baseUrl + url, requestOptions);
  return await handleResponse(response);
}

async function remove(url: string, schoolProfileId = null) {
  const requestOptions = {
    method: 'DELETE',
    headers: await getHeaders(false, schoolProfileId),
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

async function getHeaders(isFormData = false, schoolProfileId = null) {
  const token = await getCurrentUserToken();
  const headers: any = {};

  if (token) {
    headers.Authorization = 'Bearer ' + token.access_token;
  }

  if (!isFormData) {
    headers['Content-Type'] = 'application/json';
  }

  if (schoolProfileId) {
    headers['SchoolProfileId'] = schoolProfileId;
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

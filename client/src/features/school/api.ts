'use server';

import { ApiResponse } from '@/shared/types';
import { SchoolResponse, UpdateSchoolRequest } from './types';
import { api } from '@/shared/api';
import {
  removeSchoolRoute,
  getOneSchoolRoute,
  updateSchoolRoute, imageRoute,
} from './constants';

type GetOne = (id: string) => Promise<ApiResponse<SchoolResponse>>;

type Update = (data: UpdateSchoolRequest) => Promise<ApiResponse<SchoolResponse>>;

type UpdateImage = (
  { id, data } : { id: string, data: FormData }
) => Promise<ApiResponse<any>>;

type Remove = (id: string) => Promise<ApiResponse<any>>;

export const getOne: GetOne = (id: string) =>
  api.get(`${getOneSchoolRoute}/${id}`);

export const update: Update = (data: UpdateSchoolRequest) =>
  api.update(updateSchoolRoute, data);

export const updateImage: UpdateImage = ({ id, data }) =>
  api.partialUpdate(`${imageRoute}/${id}`, data, true);

export const remove: Remove = (id: string) =>
  api.remove(`${removeSchoolRoute}/${id}`);

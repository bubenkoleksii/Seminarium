'use server';

import { ApiResponse } from '@/shared/types';
import { SchoolResponse, UpdateSchoolRequest } from './types';
import { api } from '@/shared/api';
import { getOneSchoolRoute, updateSchoolRoute } from './constants';

type GetOne = (id: string) => Promise<ApiResponse<SchoolResponse>>;

type Update = (data: UpdateSchoolRequest) => Promise<ApiResponse<SchoolResponse>>;

export const getOne: GetOne = (id: string) =>
  api.get(`${getOneSchoolRoute}/${id}`);

export const update: Update = (data: UpdateSchoolRequest) =>
  api.update(updateSchoolRoute, data);

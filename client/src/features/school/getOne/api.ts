'use server';

import { ApiResponse } from '@/shared/types';
import { SchoolResponse } from './types';
import { api } from '@/shared/api';
import { getOneSchoolRoute } from '@/features/school/getOne/constants';

type GetOne = (id: string) => Promise<ApiResponse<SchoolResponse>>;

export const getOne: GetOne = (id: string) =>
  api.get(`${getOneSchoolRoute}/${id}`);

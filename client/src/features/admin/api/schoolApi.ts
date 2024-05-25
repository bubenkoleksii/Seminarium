'use server';

import type { ApiResponse } from '@/shared/types';
import type {
  CreateSchoolRequestWithId,
  PagesSchoolsResponse,
  SchoolResponse,
} from '@/features/admin/types/schoolTypes';
import { api } from '@/shared/api';
import { school } from '@/features/admin/routes';

type GetAll = (query?: string) => Promise<ApiResponse<PagesSchoolsResponse>>;
type Create = (
  data: CreateSchoolRequestWithId,
) => Promise<ApiResponse<SchoolResponse>>;

export const getAll: GetAll = (query?: string) =>
  api.get(`${school.getAll}?${query}`);

export const create: Create = (data: CreateSchoolRequestWithId) =>
  api.create(school.create, data);

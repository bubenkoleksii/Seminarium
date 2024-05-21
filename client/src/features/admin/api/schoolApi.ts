'use server';

import type { ApiResponse } from '@/shared/types';
import type { PagesSchoolsResponse } from '@/features/admin/types/schoolTypes';
import { api } from '@/shared/api';
import { school } from '@/features/admin/routes';

type GetAll = (query?: string) => Promise<ApiResponse<PagesSchoolsResponse>>;

export const getAll: GetAll = (query?: string) =>
  api.get(`${school.getAll}?${query}`);

'use server';

import type { ApiResponse } from '@/shared/types';
import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { api } from '@/shared/api';

type Get = () => Promise<ApiResponse<SchoolProfileResponse[]>>;

export const get = () =>
  api.get()
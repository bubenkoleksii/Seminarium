'use server';

import type {
  JoiningRequestResponse,
  PagedJoiningRequests,
} from '@/features/admin/types/joiningRequestTypes';
import type { ApiResponse } from '@/shared/types';
import { api } from '@/shared/api';
import { joiningRequest } from '../routes';

type GetAll = () => Promise<ApiResponse<PagedJoiningRequests>>;
type GetOne = (id: string) => Promise<ApiResponse<JoiningRequestResponse>>;

export const getAll: GetAll = () => api.get(`${joiningRequest.getAll}`);

export const getOne: GetOne = (id: string) =>
  api.get(joiningRequest.getOne(id));

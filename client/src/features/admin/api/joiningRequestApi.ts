'use server';

import type { PagedJoiningRequests } from '../types/joiningRequestTypes';
import { api } from '@/shared/api';
import { joiningRequest } from '../routes';

export const getAll = () => api.get<PagedJoiningRequests>(`${joiningRequest.getAll}`);

export const getOne = (id: string) =>
  api.get(`${joiningRequest.getOne}}/${id}`);

'use server'

import { api } from '@/shared/api';
import { joiningRequest } from '@/features/admin/routes';

export const getAll = () => api.get<any>(`${joiningRequest.getAll}`);

export const getOne = (id: string) =>
  api.get(`${joiningRequest.getOne}}/${id}`);
'use server';

import { api } from '@/shared/api';
import { joiningRequest } from '../routes';

export const getAll = () => api.get(`${joiningRequest.getAll}`);

export const getOne = (id: string) =>
  api.get(joiningRequest.getOne(id));

'use server';

import type {
  JoiningRequestResponse,
  PagedJoiningRequests,
  RejectRequest,
  RejectResponse,
} from '../types/joiningRequestTypes';
import type { ApiResponse } from '@/shared/types';
import { api } from '@/shared/api';
import { joiningRequest } from '../routes';

type GetAll = (query?: string) => Promise<ApiResponse<PagedJoiningRequests>>;
type GetOne = (id: string) => Promise<ApiResponse<JoiningRequestResponse>>;
type Reject = ({
  id: string,
  data: RejectRequest,
}: {
  id: string;
  data: RejectRequest;
}) => Promise<ApiResponse<RejectResponse>>;

export const getAll: GetAll = (query?: string) =>
  api.get(`${joiningRequest.getAll}?${query}`);

export const getOne: GetOne = (id: string) =>
  api.get(joiningRequest.getOne(id));

export const reject: Reject = ({
  id,
  data,
}: {
  id: string;
  data: RejectRequest;
}) => api.partialUpdate(joiningRequest.reject(id), data);

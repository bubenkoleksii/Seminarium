'use server';

import type { ApiResponse } from '@/shared/types';
import type { JoiningRequestResponse } from '@/features/admin/types/joiningRequestTypes';
import type { CreateJoiningRequest } from '@/features/joining-request/types';
import { api } from '@/shared/api';
import { apiCreateJoiningRequestRoute } from '@/features/joining-request/constants';

type Create = (
  data: CreateJoiningRequest,
) => Promise<ApiResponse<JoiningRequestResponse>>;

export const createJoiningRequest: Create = (data: CreateJoiningRequest) =>
  api.create(apiCreateJoiningRequestRoute, data);

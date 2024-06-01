'use server';

import { ApiResponse } from '@/shared/types';
import type {
  CreateGroupRequest,
  GroupResponse,
  PagesGroupsResponse,
} from '@/features/user/types/groupTypes';
import { api } from '@/shared/api';
import { group } from '@/features/user/routes';

type GetAll = ({
  query,
}: {
  query: string;
}) => Promise<ApiResponse<PagesGroupsResponse>>;

type Create = ({
  data,
  schoolProfileId,
}: {
  data: CreateGroupRequest;
  schoolProfileId: string;
}) => Promise<ApiResponse<GroupResponse>>;

export const getAll: GetAll = ({ query }) =>
  api.get(`${group.getAll}/?${query}`);

export const create: Create = ({ data, schoolProfileId }) =>
  api.create(group.create, data, false, schoolProfileId);

'use server';

import { ApiResponse } from '@/shared/types';
import type {
  CreateGroupRequest,
  GroupResponse,
  OneGroupResponse,
  PagesGroupsResponse,
} from '@/features/user/types/groupTypes';
import { api } from '@/shared/api';
import { group } from '@/features/user/routes';

type GetAll = ({
  query,
}: {
  query: string;
}) => Promise<ApiResponse<PagesGroupsResponse>>;

type GetOne = (id: string) => Promise<ApiResponse<OneGroupResponse>>;

type Create = ({
  data,
  schoolProfileId,
}: {
  data: CreateGroupRequest;
  schoolProfileId: string;
}) => Promise<ApiResponse<GroupResponse>>;

type GenerateInvitation = ({
  id,
  schoolProfileId,
}: {
  id: string;
  schoolProfileId?: string;
}) => Promise<ApiResponse<string>>;

export const getAll: GetAll = ({ query }) =>
  api.get(`${group.getAll}/?${query}`);

export const getOne: GetOne = (id: string) => api.get(`${group.getOne}/${id}`);

export const create: Create = ({ data, schoolProfileId }) =>
  api.create(group.create, data, false, schoolProfileId);

export const createClassTeacherInvitation: GenerateInvitation = ({
  id,
  schoolProfileId,
}) =>
  api.create(
    `${group.createClassTeacherInvitation}`,
    { groupId: id },
    false,
    schoolProfileId,
  );

export const createStudentInvitation: GenerateInvitation = ({
  id,
  schoolProfileId,
}) =>
  api.create(
    `${group.createStudentInvitation}`,
    { groupId: id },
    false,
    schoolProfileId,
  );

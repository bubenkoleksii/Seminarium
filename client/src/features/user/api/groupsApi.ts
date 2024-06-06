'use server';

import { ApiResponse } from '@/shared/types';
import type {
  CreateGroupRequest,
  GroupResponse,
  OneGroupResponse,
  PagesGroupsResponse,
  UpdateGroupRequest,
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

type Update = ({
  data,
  schoolProfileId,
}: {
  data: UpdateGroupRequest;
  schoolProfileId: string;
}) => Promise<ApiResponse<GroupResponse>>;

type UpdateImage = ({
  id,
  data,
  schoolProfileId,
}: {
  id: string;
  data: FormData;
  schoolProfileId?: string;
}) => Promise<ApiResponse<any>>;

type RemoveImage = ({
  id,
  schoolProfileId,
}: {
  id: string;
  schoolProfileId?: string;
}) => Promise<ApiResponse<any>>;

type Remove = ({
  id,
  schoolProfileId,
}: {
  id: string;
  schoolProfileId: string;
}) => Promise<ApiResponse<any>>;

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

export const updateImage: UpdateImage = ({ id, data, schoolProfileId }) =>
  api.partialUpdate(group.image(id), data, true, schoolProfileId);

export const removeImage: RemoveImage = ({ id, schoolProfileId }) =>
  api.remove(group.image(id), schoolProfileId);

export const update: Update = ({ data, schoolProfileId }) =>
  api.update(group.update, data, false, schoolProfileId);

export const remove: Remove = ({ id, schoolProfileId }) =>
  api.remove(group.remove(id), schoolProfileId);

'use server';

import type { ApiResponse } from '@/shared/types';
import type {
  CreateSchoolProfileRequest,
  SchoolProfileResponse, UpdateSchoolProfileRequest,
} from '@/features/user/types/schoolProfileTypes';
import { api } from '@/shared/api';
import { schoolProfile } from '@/features/user/routes';

type Get = () => Promise<ApiResponse<SchoolProfileResponse[]>>;

type GetOne = (id: string) => Promise<ApiResponse<SchoolProfileResponse>>;

type Create = (
  data: CreateSchoolProfileRequest,
) => Promise<ApiResponse<SchoolProfileResponse>>;

type Activate = (id: string) => Promise<ApiResponse<SchoolProfileResponse>>;

type GenerateInvitation = ({
  id,
  schoolProfileId,
}: {
  id: string;
  schoolProfileId?: string;
}) => Promise<ApiResponse<string>>;

type Remove = (id: string) => Promise<ApiResponse<any>>;

type Update = ({
  data,
  schoolProfileId
}: {
  data: UpdateSchoolProfileRequest;
  schoolProfileId: string;
}) => Promise<ApiResponse<SchoolProfileResponse>>;

type UpdateImage = ({
  id,
  data,
  schoolProfileId
}: {
  id: string;
  data: FormData;
  schoolProfileId?: string;
}) => Promise<ApiResponse<any>>;

type RemoveImage = ({
  id,
  schoolProfileId
}: {
  id: string;
  schoolProfileId?: string;
}) => Promise<ApiResponse<any>>;

export const get: Get = () => api.get(schoolProfile.get);

export const getOne: GetOne = (id) => api.get(schoolProfile.getOne(id));

export const activate: Activate = (id) =>
  api.partialUpdate(schoolProfile.activate(id), {});

export const createParentInvitation: GenerateInvitation = ({
  id,
  schoolProfileId,
}) =>
  api.create(
    `${schoolProfile.parentInvitation}`,
    { childId: id },
    false,
    schoolProfileId,
  );

export const create: Create = (data: CreateSchoolProfileRequest) =>
  api.create(schoolProfile.create, data);

export const remove: Remove = (id: string) =>
  api.remove(schoolProfile.remove(id));

export const update: Update = ({ data, schoolProfileId }) =>
  api.update(schoolProfile.update, data, false, schoolProfileId);

export const updateImage: UpdateImage = ({ id, data, schoolProfileId }) =>
  api.partialUpdate(schoolProfile.image(id), data, true, schoolProfileId);

export const removeImage: RemoveImage = ({ id, schoolProfileId }) =>
  api.remove(schoolProfile.image(id), schoolProfileId);

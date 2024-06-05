'use server';

import type { ApiResponse } from '@/shared/types';
import type {
  CreateSchoolProfileRequest,
  SchoolProfileResponse,
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
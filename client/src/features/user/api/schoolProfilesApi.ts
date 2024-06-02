'use server';

import type { ApiResponse } from '@/shared/types';
import type { CreateSchoolProfileRequest, SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import { api } from '@/shared/api';
import { schoolProfile } from '@/features/user/routes';

type Get = () => Promise<ApiResponse<SchoolProfileResponse[]>>;

type Create = (data: CreateSchoolProfileRequest) => Promise<ApiResponse<SchoolProfileResponse>>

type Activate = (id: string) => Promise<ApiResponse<SchoolProfileResponse>>;

export const get: Get = () => api.get(schoolProfile.get);

export const activate: Activate = (id) =>
  api.partialUpdate(schoolProfile.activate(id), {});

export const create: Create = (data: CreateSchoolProfileRequest) =>
  api.create(schoolProfile.create, data);
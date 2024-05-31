'use server';

import { ApiResponse } from '@/shared/types';
import { SchoolResponse, UpdateSchoolRequest } from './types';
import { api } from '@/shared/api';
import {
  removeSchoolRoute,
  getOneSchoolRoute,
  updateSchoolRoute,
  imageRoute,
  createInvitationRoute,
} from './constants';

type GetOne = (id: string) => Promise<ApiResponse<SchoolResponse>>;

type Update = ({
  data,
  schoolProfileId,
}: {
  data: UpdateSchoolRequest;
  schoolProfileId?: string;
}) => Promise<ApiResponse<SchoolResponse>>;

type UpdateImage = ({
  id,
  data,
  schoolProfileId,
}: {
  id: string;
  data: FormData;
  schoolProfileId?: string;
}) => Promise<ApiResponse<any>>;

type Remove = (id: string) => Promise<ApiResponse<any>>;

type RemoveImage = ({
  id,
  schoolProfileId,
}: {
  id: string;
  schoolProfileId?: string;
}) => Promise<ApiResponse<any>>;

type GenerateInvitation = ({
  id,
  schoolProfileId,
}: {
  id: string;
  schoolProfileId?: string;
}) => Promise<ApiResponse<string>>;

export const getOne: GetOne = (id: string) =>
  api.get(`${getOneSchoolRoute}/${id}`);

export const update: Update = ({ data, schoolProfileId }) =>
  api.update(updateSchoolRoute, data, false, schoolProfileId);

export const remove: Remove = (id: string) =>
  api.remove(`${removeSchoolRoute}/${id}`);

export const updateImage: UpdateImage = ({ id, data, schoolProfileId }) =>
  api.partialUpdate(`${imageRoute}/${id}`, data, true, schoolProfileId);

export const removeImage: RemoveImage = ({ id, schoolProfileId }) =>
  api.remove(`${imageRoute}/${id}`, schoolProfileId);

export const createInvitation: GenerateInvitation = ({ id, schoolProfileId }) =>
  api.create(
    `${createInvitationRoute}`,
    { schoolId: id },
    false,
    schoolProfileId,
  );

'use server';

import { api } from '@/shared/api';
import type { ApiResponse } from '@/shared/types';
import { groupNotice } from '../routes';
import type {
  CreateGroupNoticeRequest,
  GroupNoticeResponse,
  PagesGroupNoticeResponse,
  UpdateGroupNoticeRequest,
} from '../types/groupNoticesTypes';

type GetAll = ({
  query,
}: {
  query: string;
}) => Promise<ApiResponse<PagesGroupNoticeResponse>>;

type Create = ({
  data,
  schoolProfileId,
}: {
  data: CreateGroupNoticeRequest;
  schoolProfileId: string;
}) => Promise<ApiResponse<GroupNoticeResponse>>;

type Update = ({
  data,
  schoolProfileId,
}: {
  data: UpdateGroupNoticeRequest;
  schoolProfileId: string;
}) => Promise<ApiResponse<GroupNoticeResponse>>;

type ChangeCrucial = ({
  data,
  schoolProfileId,
}: {
  data: {
    id: string;
    isCrucial: boolean;
  };
  schoolProfileId: string;
}) => Promise<ApiResponse<GroupNoticeResponse>>;

type Remove = ({
  id,
  schoolProfileId,
}: {
  id: string;
  schoolProfileId: string;
}) => Promise<ApiResponse<any>>;

export const getAllGroupNotices: GetAll = ({ query }) =>
  api.get(groupNotice.getAll(query));

export const createGroupNotice: Create = ({ data, schoolProfileId }) =>
  api.create(groupNotice.create, data, false, schoolProfileId);

export const updateGroupNotice: Update = ({ data, schoolProfileId }) =>
  api.update(groupNotice.update, data, false, schoolProfileId);

export const changeCrucial: ChangeCrucial = ({ data, schoolProfileId }) =>
  api.partialUpdate(groupNotice.changeCrucial, data, false, schoolProfileId);

export const removeGroupNotice: Remove = ({ id, schoolProfileId }) =>
  api.remove(groupNotice.remove(id), schoolProfileId);

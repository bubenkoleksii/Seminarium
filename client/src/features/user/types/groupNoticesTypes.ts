import type { SchoolProfileResponse } from './schoolProfileTypes';

export type CreateGroupNoticeRequest = {
  groupId: string;
  isCrucial: boolean;
  title: string;
  text?: string;
};

export type UpdateGroupNoticeRequest = {
  id: string;
  groupId: string;
  isCrucial: boolean | string;
  title: string;
  text?: string;
};

export interface GroupNoticeResponse {
  id: string;
  createdAt: string;
  lastUpdatedAt?: string;
  title: string;
  text?: string | null;
  isCrucial: boolean;
  groupId: string;
  authorId?: string | null;
  author?: SchoolProfileResponse | null;
}

export interface PagesGroupNoticeResponse {
  lastNotice?: GroupNoticeResponse | null;
  crucialNotices: GroupNoticeResponse[];
  regularNotices: GroupNoticeResponse[];
  total: number;
  skip: number;
  take: number;
}

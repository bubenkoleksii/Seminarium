import type { SchoolProfileResponse } from '@/features/user/types/schoolProfileTypes';
import type { GroupNoticeResponse } from './groupNoticesTypes';

export interface PagesGroupsResponse {
  entries: GroupResponse[];
  schoolName: string;
  total: number;
  skip: number;
  take: number;
}

export interface GroupResponse {
  id: string;
  schoolId: string;
  createdAt: string;
  lastUpdatedAt?: string;
  name: string;
  studyPeriodNumber: number;
  img?: string;
}

export interface OneGroupResponse extends GroupResponse {
  schoolName: string;
  classTeacherId?: string;
  classTeacher?: SchoolProfileResponse;
  students?: SchoolProfileResponse[];
  lastNotice: GroupNoticeResponse;
}

export interface CreateGroupRequest {
  name: string;
  studyPeriodNumber: number;
}

export interface UpdateGroupRequest {
  id: string;
  name: string;
  studyPeriodNumber: number;
  img?: string | null;
}

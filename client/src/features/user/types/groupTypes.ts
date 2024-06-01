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

export interface CreateGroupRequest {
  name: string;
  studyPeriodNumber: number;
}

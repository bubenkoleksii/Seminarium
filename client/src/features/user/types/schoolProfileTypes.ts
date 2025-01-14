import type { GroupResponse } from '@/features/user/types/groupTypes';
import type { SchoolResponse } from '@/features/school/types';

export interface PagesSchoolProfilesResponse {
  entries: SchoolProfileResponse[];
  total: number;
  skip: number;
  take: number;
}

export type SchoolProfileResponse = {
  id: string;
  userId: string;
  name: string;
  schoolId?: string;
  schoolName?: string;
  school?: SchoolResponse;
  groupId?: string;
  group?: GroupResponse;
  classTeacherGroupId?: string;
  classTeacherGroup?: GroupResponse;
  isActive: boolean;
  createdAt: string;
  lastUpdatedAt?: string;
  type: string;
  phone?: string;
  email?: string;
  img?: string;
  details?: string;
  teacherSubjects?: string;
  teacherExperience?: string;
  teacherEducation?: string;
  teacherQualification?: string;
  teacherLessonsPerCycle?: number;
  studentDateOfBirth?: string;
  studentAptitudes?: string;
  studentIsClassLeader?: boolean;
  studentIsIndividually?: boolean;
  studentHealthGroup?: 'main' | 'special' | 'free' | 'preparatory';
  parentAddress?: string;
  children?: SchoolProfileResponse[];
  parents?: SchoolProfileResponse[];
};

export type CreateSchoolProfileRequest = {
  invitationCode: string;
  name: string;
  phone?: string;
  email?: string;
  details?: string;
  teacherSubjects?: string;
  teacherExperience?: string;
  teacherEducation?: string;
  teacherQualification?: string;
  teacherLessonsPerCycle?: number;
  studentDateOfBirth?: string;
  studentAptitudes?: string;
  studentIsClassLeader?: boolean;
  studentIsIndividually?: boolean;
  studentHealthGroup?: string;
  parentAddress?: string;
};

export type UpdateSchoolProfileRequest = {
  id: string;
  type?: string;
  name: string;
  phone?: string;
  img?: string;
  email?: string;
  details?: string;
  teacherSubjects?: string;
  teacherExperience?: string;
  teacherEducation?: string;
  teacherQualification?: string;
  teacherLessonsPerCycle?: number;
  studentDateOfBirth?: string;
  studentAptitudes?: string;
  studentIsClassLeader?: boolean | string;
  studentIsIndividually?: boolean | string;
  studentHealthGroup?: string;
  parentAddress?: string;
};

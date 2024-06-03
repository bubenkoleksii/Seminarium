import type { GroupResponse } from '@/features/user/types/groupTypes';
import type { SchoolResponse } from '@/features/school/types';

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
  isActive: boolean;
  createdAt: string;
  lastUpdatedAt?: string;
  type: string;
  phone?: string;
  email?: string;
  img?: string;
  details?: string;
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
  children?: SchoolProfileResponse[],
  parents?: SchoolProfileResponse[],
};

export type CreateSchoolProfileRequest = {
  invitationCode: string;
  name: string;
  phone?: string;
  email?: string;
  details?: string;
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
  parentRelationship?: string;
};

export type UpdateSchoolProfileRequest = {
  id: string;
  name: string;
  phone?: string;
  email?: string;
  details?: string;
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

export type SchoolProfileResponse = {
  id: string;
  userId: string;
  name: string;
  schoolId?: string;
  schoolName?: string;
  groupId?: string;
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
  parentRelationship?: string;
};

export type CreateSchoolProfileRequest = {
  invitationCode: string;
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

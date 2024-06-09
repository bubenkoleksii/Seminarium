export type CreateStudyPeriodRequest = {
  startDate: string;
  endDate: string;
};

export type UpdateStudyPeriodRequest = {
  id: string;
  startDate: string;
  endDate: string;
};

export type StudyPeriodResponse = {
  id: string;
  schoolId: string;
  school: SchoolResponse;
  startDate: string;
  endDate: string;
};

interface SchoolResponse {
  id: string;
  joiningRequestId: string;
  createdAt: string;
  lastUpdatedAt?: string;
  registerCode: string;
  name: string;
  shortName?: string;
  gradingSystem: number;
  email?: string;
  phone?: string;
  type: string;
  postalCode: string;
  ownershipType: string;
  studentsQuantity: number;
  region: string;
  territorialCommunity?: string;
  address?: string;
  areOccupied: boolean;
  siteUrl?: string;
  img?: string;
}

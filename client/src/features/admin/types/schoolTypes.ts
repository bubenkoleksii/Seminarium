export interface PagesSchoolsResponse {
  entries: SchoolResponse[];
  total: number;
  skip: number;
  take: number;
}

export interface SchoolResponse {
  id: string;
  joiningRequestId: string;
  createdAt: string;
  lastUpdatedAt?: string;
  registerCode: number;
  name: string;
  shortName?: string;
  gradingSystem: number;
  email?: string;
  phone?: string;
  type: string;
  postalCode: number;
  ownershipType: string;
  studentsQuantity: number;
  region: string;
  territorialCommunity?: string;
  address?: string;
  areOccupied: boolean;
  siteUrl?: string;
  img?: string;
}

export interface CreateSchoolRequest {
  registerCode: number;
  name: string;
  shortName?: string;
  gradingSystem: number;
  type: string;
  postalCode: number;
  ownershipType: string;
  studentsQuantity: number;
  region: string;
  territorialCommunity?: string;
  address?: string;
  areOccupied: boolean;
}

export interface CreateSchoolRequestWithId extends CreateSchoolRequest {
  joiningRequestId: string;
}

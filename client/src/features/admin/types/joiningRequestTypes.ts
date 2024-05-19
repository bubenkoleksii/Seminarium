export interface PagedJoiningRequests {
  entries: JoiningRequestResponse[];
  total: number;
  skip: number;
  take: number;
}

export interface JoiningRequestResponse {
  id: string;
  schoolId?: string;
  createdAt: string;
  lastUpdatedAt?: string;
  registerCode: number;
  name: string;
  requesterEmail: string;
  requesterPhone: string;
  requesterFullName: string;
  shortName?: string | null;
  gradingSystem: number;
  type: string;
  postalCode: number;
  ownershipType: string;
  studentsQuantity: number;
  region: string;
  territorialCommunity?: string | null;
  address?: string | null;
  areOccupied: boolean;
  status: 'created' | 'rejected' | 'approved';
}

export interface RejectRequest {
  message?: string | null;
}

export interface RejectResponse {
  id: string,
  status: 'created' | 'rejected' | 'approved';
}
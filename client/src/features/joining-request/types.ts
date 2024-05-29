export interface CreateJoiningRequest {
  registerCode: number;
  name: string;
  requesterEmail: string;
  requesterPhone: string;
  requesterFullName: string;
  shortName: string | null;
  gradingSystem: number;
  type: string;
  postalCode: string;
  ownershipType: string;
  studentsQuantity: number;
  region: string;
  territorialCommunity: string | null;
  address: string | null;
  areOccupied: boolean;
}

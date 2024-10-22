export type PagesPracticalLessonItemSubmitResponse = {
  entries: GetAllPracticalLessonItemSubmitResponseItem[];
  total: number;
  skip: number;
  take: number;
};

export interface AddPracticalItemSubmitResultsRequest {
  id: string;
  isAccept: boolean;
  text?: string;
  mark?: number;
}

type GetAllPracticalLessonItemSubmitResponseItem = {
  id: string;
  createdAt: string;
  studentId: string;
  studentName?: string | null;
  groupName?: string | null;
  mark?: number | null;
  practicalLessonItemId: string;
  attachments?: string[] | null;
  status: string;
};

export type PracticalLessonItemSubmitResponse = {
  id: string;
  createdAt: string;
  lastUpdatedAt?: string | null;
  studentId: string;
  studentName?: string | null;
  practicalLessonItemId: string;
  text?: string | null;
  teacherComment?: string | null;
  mark?: number | null;
  attachments?: string[] | null;
  status: string;
};

export type CreatePracticalLessonItemSubmitRequest = {
  practicalLessonItemId: string;
  text?: string | null;
};

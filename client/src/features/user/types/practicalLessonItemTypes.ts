import { SchoolProfileResponse } from './schoolProfileTypes';

export interface StudentPracticalLessonItemResponse {
  id: string;
  createdAt: Date;
  deadline: Date;
  title: string;
  lessonTopic: string;
  courseName: string;
  status: string;
}

export interface GetAllStudentPracticalLessonItemsResponse {
  entries: StudentPracticalLessonItemResponse[];
  studentId: string;
  total: number;
  skip: number;
  take: number;
}

export interface CreatePracticalLessonItemRequest {
  lessonId: string;
  deadline?: string | null;
  title: string;
  text?: string;
  attempts?: number;
  allowSubmitAfterDeadline: boolean;
  isArchived: boolean;
}

export interface UpdatePracticalLessonItemRequest {
  id: string;
  deadline?: string;
  title: string;
  text?: string;
  allowSubmitAfterDeadline: boolean;
  lessonId?: string;
  courseId?: string;
}

export interface PracticalLessonItemResponse {
  id: string;
  createdAt: string;
  lastUpdatedAt?: string;
  title?: string;
  text?: string;
  lessonId: string;
  authorId?: string;
  author?: SchoolProfileResponse;
  attachments?: string[];
  attempts?: number;
  allowSubmitAfterDeadline: boolean;
  isArchived: boolean;
}

import { SchoolProfileResponse } from './schoolProfileTypes';

export interface CreatePracticalLessonItemRequest {
  lessonId: string;
  deadline?: string;
  title: string;
  text?: string;
  attempts?: number;
  allowSubmitAfterDeadline: boolean;
  isArchived: boolean;
}

export interface UpdatePracticalLessonItemRequest {
  id: string;
  deadline?: Date;
  title: string;
  text?: string;
  attempts?: number;
  allowSubmitAfterDeadline: boolean;
}

export interface PracticalLessonItemResponse {
  id: string;
  createdAt: Date;
  lastUpdatedAt?: Date;
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

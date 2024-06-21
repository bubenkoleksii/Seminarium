import type { SchoolProfileResponse } from './schoolProfileTypes';

export interface CreateTheoryLessonItemRequest {
  lessonId: string;
  deadline?: string | null;
  title: string;
  text?: string;
  isGraded: boolean;
  isArchived: boolean;
}

export interface UpdateTheoryLessonItemRequest {
  id: string;
  lessonId: string;
  deadline?: string | null;
  title: string;
  text?: string;
  isGraded: boolean;
  isArchived: boolean;
}

export interface TheoryLessonItemResponse {
  id: string;
  createdAt: string;
  lastUpdatedAt?: string;
  title?: string;
  text?: string;
  lessonId: string;
  authorId?: string;
  author?: SchoolProfileResponse;
  attachments?: string[];
  isGraded: boolean;
  isArchived: boolean;
}

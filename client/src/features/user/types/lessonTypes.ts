export interface PagesLessonsResponse {
  entries: LessonResponse[];
  courseId?: string | null;
  total: number;
  skip: number;
  take: number;
}

export interface CreateLessonRequest {
  courseId: string;
  number: number;
  topic: string;
  homework?: string;
  needPracticalItem: boolean;
}

export interface UpdateLessonRequest {
  courseId: string;
  number: number;
  topic: string;
  homework?: string;
}

export interface LessonResponse {
  courseId: string;
  number: number;
  topic: string;
  homework?: string;
  practicalItemsCount: number;
  theoryItemsCount: number;
}

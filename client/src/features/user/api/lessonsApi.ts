'use server';

import { lesson } from '@/features/user/routes';
import type {
  CreateLessonRequest,
  LessonResponse,
  PagesLessonsResponse,
  UpdateLessonRequest,
} from '@/features/user/types/lessonTypes';
import { api } from '@/shared/api';
import { ApiResponse } from '@/shared/types';

type GetAll = ({
  query,
}: {
  query: string;
}) => Promise<ApiResponse<PagesLessonsResponse>>;

type GetOne = (id: string) => Promise<ApiResponse<LessonResponse>>;

type CreateLesson = (
  data: CreateLessonRequest,
) => Promise<ApiResponse<LessonResponse>>;

type UpdateLesson = (
  data: UpdateLessonRequest,
) => Promise<ApiResponse<LessonResponse>>;

type RemoveLesson = (id: string) => Promise<ApiResponse<any>>;

export const getAllLessons: GetAll = ({ query }) =>
  api.get(lesson.getAll(query));

export const getOneLesson: GetOne = (id) => api.get(lesson.getOne(id));

export const createLesson: CreateLesson = (data) =>
  api.create(lesson.create, data, false);

export const updateLesson: UpdateLesson = (data) =>
  api.update(lesson.update, data, false);

export const removeLesson: RemoveLesson = (id) => api.remove(lesson.remove(id));

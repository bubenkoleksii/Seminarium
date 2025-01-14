'use server';

import { practicalLessonItem } from '@/features/user/routes';
import type {
  GetAllStudentPracticalLessonItemsResponse,
  PracticalLessonItemResponse,
  UpdatePracticalLessonItemRequest,
} from '@/features/user/types/practicalLessonItemTypes';
import { api } from '@/shared/api';
import { ApiResponse } from '@/shared/types';

type GetAll = (
  lessonId: string,
) => Promise<ApiResponse<PracticalLessonItemResponse[]>>;

type GetAllStudent = (
  query: string,
) => Promise<ApiResponse<GetAllStudentPracticalLessonItemsResponse>>;

type CreatePracticalLessonItem = (
  data: any,
) => Promise<ApiResponse<PracticalLessonItemResponse>>;

type UpdatePracticalLessonItem = (
  data: UpdatePracticalLessonItemRequest,
) => Promise<ApiResponse<PracticalLessonItemResponse>>;

type RemovePracticalLessonItem = (id: string) => Promise<ApiResponse<any>>;

export const getAllStudentPracticalLessonItems: GetAllStudent = (query) =>
  api.get(practicalLessonItem.getStudentAll(query));

export const getAllPracticalLessonItems: GetAll = (lessonId) =>
  api.get(practicalLessonItem.getAll(lessonId));

export const createPracticalLessonItem: CreatePracticalLessonItem = (data) =>
  api.create(practicalLessonItem.create, data, true);

export const updatePracticalLessonItem: UpdatePracticalLessonItem = (data) =>
  api.update(practicalLessonItem.update, data, false);

export const removePracticalLessonItem: RemovePracticalLessonItem = (id) =>
  api.remove(practicalLessonItem.remove(id));

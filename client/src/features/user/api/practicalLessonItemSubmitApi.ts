'use server';

import { practicalLessonItemSubmit } from '@/features/user/routes';
import type { PracticalLessonItemSubmitResponse } from '@/features/user/types/practicalLessonItemSubmitTypes';
import { api } from '@/shared/api';
import { ApiResponse } from '@/shared/types';

type GetOne = (
  studentId: string,
  itemId: string,
) => Promise<ApiResponse<PracticalLessonItemSubmitResponse>>;

type GetAllSubmits = (
  practicalLessonItemId: string,
  studentId: string,
) => Promise<ApiResponse<PracticalLessonItemSubmitResponse[]>>;

type CreatePracticalLessonItemSubmit = (
  data: any,
) => Promise<ApiResponse<PracticalLessonItemSubmitResponse>>;

type RemovePracticalLessonItemSubmit = (
  id: string,
) => Promise<ApiResponse<any>>;

export const getOnePracticalLessonItemSubmit: GetOne = (studentId, itemId) =>
  api.get(practicalLessonItemSubmit.getOne(studentId, itemId));

export const getAllPracticalLessonItemSubmits: GetAllSubmits = (
  practicalLessonItemId,
  studentId,
) =>
  api.get(practicalLessonItemSubmit.getAll(practicalLessonItemId, studentId));

export const createPracticalLessonItemSubmit: CreatePracticalLessonItemSubmit =
  (data) => api.create(practicalLessonItemSubmit.create, data, true);

export const removePracticalLessonItemSubmit: RemovePracticalLessonItemSubmit =
  (id) => api.remove(practicalLessonItemSubmit.remove(id));

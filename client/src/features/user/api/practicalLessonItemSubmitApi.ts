'use server';

import { practicalLessonItemSubmit } from '@/features/user/routes';
import type {
  AddPracticalItemSubmitResultsRequest,
  PagesPracticalLessonItemSubmitResponse,
  PracticalLessonItemSubmitResponse,
} from '@/features/user/types/practicalLessonItemSubmitTypes';
import { api } from '@/shared/api';
import { ApiResponse } from '@/shared/types';

type GetAll = (
  query: string,
) => Promise<ApiResponse<PagesPracticalLessonItemSubmitResponse>>;

type GetOne = (
  studentId: string,
  itemId: string,
) => Promise<ApiResponse<PracticalLessonItemSubmitResponse>>;

type AddResults = (
  data: AddPracticalItemSubmitResultsRequest,
) => Promise<ApiResponse<any>>;

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

export const getTeacherAllPracticalLessonItemSubmit: GetAll = (query) =>
  api.get(practicalLessonItemSubmit.getTeacherAll(query));

export const getOnePracticalLessonItemSubmit: GetOne = (studentId, itemId) =>
  api.get(practicalLessonItemSubmit.getOne(studentId, itemId));

export const getAllPracticalLessonItemSubmits: GetAllSubmits = (
  practicalLessonItemId,
  studentId,
) =>
  api.get(practicalLessonItemSubmit.getAll(practicalLessonItemId, studentId));

export const addResults: AddResults = (data) =>
  api.partialUpdate(practicalLessonItemSubmit.addResults, data);

export const createPracticalLessonItemSubmit: CreatePracticalLessonItemSubmit =
  (data) => api.create(practicalLessonItemSubmit.create, data, true);

export const removePracticalLessonItemSubmit: RemovePracticalLessonItemSubmit =
  (id) => api.remove(practicalLessonItemSubmit.remove(id));

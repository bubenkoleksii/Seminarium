'use server';

import { theoryLessonItem } from '@/features/user/routes';
import type {
  TheoryLessonItemResponse,
  UpdateTheoryLessonItemRequest
} from '@/features/user/types/theoryLessonItemTypes';
import { api } from '@/shared/api';
import { ApiResponse } from '@/shared/types';

type GetAll = ({
  query,
}: {
  query: string;
}) => Promise<ApiResponse<TheoryLessonItemResponse[]>>;

type CreateTheoryLessonItem = (
  data: any
) => Promise<ApiResponse<TheoryLessonItemResponse>>;

type UpdateTheoryLessonItem = (
  data: UpdateTheoryLessonItemRequest,
) => Promise<ApiResponse<TheoryLessonItemResponse>>;

type RemoveTheoryLessonItem = (id: string) => Promise<ApiResponse<any>>;

export const getAllTheoryLessonItems: GetAll = ({ query }) =>
  api.get(theoryLessonItem.getAll(query));

export const createTheoryLessonItem: CreateTheoryLessonItem = (data) =>
  api.create(theoryLessonItem.create, data, true);

export const updateTheoryLessonItem: UpdateTheoryLessonItem = (data) =>
  api.update(theoryLessonItem.update, data, false);

export const removeTheoryLessonItem: RemoveTheoryLessonItem = (id) =>
  api.remove(theoryLessonItem.remove(id));

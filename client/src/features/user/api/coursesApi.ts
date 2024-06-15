'use server';

import { course } from '@/features/user/routes';
import type {
  CourseResponse,
  CreateCourseRequest,
  PagesCoursesResponse,
  UpdateCourseRequest,
} from '@/features/user/types/courseTypes';
import { api } from '@/shared/api';
import { ApiResponse } from '@/shared/types';

type GetAllCourses = (
  query: string,
) => Promise<ApiResponse<PagesCoursesResponse>>;

type CreateCourse = (
  data: CreateCourseRequest,
) => Promise<ApiResponse<CourseResponse>>;

type UpdateCourse = (
  data: UpdateCourseRequest,
) => Promise<ApiResponse<CourseResponse>>;

type RemoveCourse = (id: string) => Promise<ApiResponse<any>>;

export const getAllCourses: GetAllCourses = (query) =>
  api.get(`${course.getAll}/?${query}`);

export const createCourse: CreateCourse = (data) =>
  api.create(course.create, data, false);

export const updateCourse: UpdateCourse = (data) =>
  api.update(course.update, data, false);

export const removeCourse: RemoveCourse = (id) => api.remove(course.remove(id));

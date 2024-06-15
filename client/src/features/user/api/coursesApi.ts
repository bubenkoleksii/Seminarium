'use server';

import { course } from '@/features/user/routes';
import type {
  AddCourseGroupRequest,
  AddCourseTeacherRequest,
  CourseResponse,
  CreateCourseRequest,
  PagesCoursesResponse,
  UpdateCourseRequest,
} from '@/features/user/types/courseTypes';
import { api } from '@/shared/api';
import { ApiResponse } from '@/shared/types';

type GetOne = (id: string) => Promise<ApiResponse<CourseResponse>>;

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

type AddCourseGroup = (
  data: AddCourseGroupRequest,
) => Promise<ApiResponse<any>>;

type RemoveCourseGroup = ({
  id,
  courseId,
}: {
  id: string;
  courseId: string;
}) => Promise<ApiResponse<any>>;

type AddCourseTeacher = (
  data: AddCourseTeacherRequest,
) => Promise<ApiResponse<any>>;

type RemoveCourseTeacher = ({
  id,
  courseId,
}: {
  id: string;
  courseId: string;
}) => Promise<ApiResponse<any>>;

export const getOneCourse: GetOne = (id) => api.get(course.getOne(id));

export const getAllCourses: GetAllCourses = (query) =>
  api.get(`${course.getAll}/?${query}`);

export const createCourse: CreateCourse = (data) =>
  api.create(course.create, data, false);

export const updateCourse: UpdateCourse = (data) =>
  api.update(course.update, data, false);

export const removeCourse: RemoveCourse = (id) => api.remove(course.remove(id));

export const addCourseGroup: AddCourseGroup = (data) =>
  api.create(course.addGroup, data, false);

export const removeCourseGroup: RemoveCourseGroup = ({ id, courseId }) =>
  api.remove(course.removeGroup(id, courseId));

export const addCourseTeacher: AddCourseTeacher = (data) =>
  api.create(course.addTeacher, data, false);

export const removeCourseTeacher: RemoveCourseTeacher = ({ id, courseId }) =>
  api.remove(course.removeTeacher(id, courseId));

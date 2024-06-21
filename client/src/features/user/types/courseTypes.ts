export interface PagesCoursesResponse {
  entries: CourseResponse[];
  studyPeriodId: string;
  total: number;
  skip: number;
  take: number;
}

export interface CourseResponse {
  id: string;
  studyPeriodId: string;
  createdAt: Date;
  lastUpdatedAt?: Date | null;
  name: string;
  description?: string | null;
  teachers?: CourseTeacherResponse[] | null;
  groups?: CourseGroupResponse[] | null;
}

export interface AddCourseGroupRequest {
  name: string;
  courseId: string;
  schoolId: string;
}

export interface AddCourseTeacherRequest {
  name: string;
  courseId: string;
  schoolId: string;
}

export interface CreateCourseRequest {
  studyPeriodId: string;
  name: string;
  description?: string | null;
}

export interface UpdateCourseRequest {
  id: string;
  studyPeriodId: string;
  name: string;
  description?: string | null;
}

export interface CourseGroupResponse {
  id: string;
  name: string;
}

export interface CourseTeacherResponse {
  id: string;
  name: string;
  isCreator: boolean;
}

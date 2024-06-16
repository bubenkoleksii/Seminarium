export const schoolProfile = {
  get: '/schoolProfile/Get',
  getAllBySchool: (query: string | null | undefined) =>
    `/schoolProfile/GetAllBySchool/?${query}`,
  update: '/schoolProfile/Update',
  image: (id: string) => `/schoolProfile/Image/${id}`,
  getOne: (id: string) => `/schoolProfile/GetOne/${id}`,
  remove: (id: string) => `/schoolProfile/Delete/${id}`,
  create: '/schoolProfile/create',
  parentInvitation: '/schoolProfile/ParentInvitation',
  activate: (id: string) => `/schoolProfile/Activate/${id}`,
};

export const course = {
  getOne: (id: string) => `/course/getOne/${id}`,
  getAll: '/course/getAll',
  create: '/course/create',
  update: '/course/update',
  addGroup: '/course/addGroup',
  addTeacher: '/course/addTeacher',
  remove: (id: string) => `/course/Delete/${id}`,
  removeGroup: (id: string, courseId: string) =>
    `/course/DeleteCourseGroup/${id}/${courseId}`,
  removeTeacher: (id: string, courseId: string) =>
    `/course/DeleteCourseTeacher/${id}/${courseId}`,
};

export const lesson = {
  getOne: (id: string) => `/lesson/getOne/${id}`,
  getAll: (query: string) => `/lesson/getAll/?${query}`,
  create: '/lesson/create',
  update: '/lesson/update',
  remove: (id: string) => `/lesson/Delete/${id}`,
};

export const group = {
  getAll: '/group/getAll',
  getOne: '/group/getOne',
  create: '/group/create',
  update: '/group/update',
  image: (id: string) => `/group/Image/${id}`,
  remove: (id: string) => `/group/Delete/${id}`,
  createClassTeacherInvitation: '/group/classTeacherInvitation',
  createStudentInvitation: '/group/studentInvitation',
};

export const studyPeriods = {
  getAll: '/studyPeriod/getAll',
  create: '/studyPeriod/create',
  update: '/studyPeriod/update',
  remove: (id: string) => `/studyPeriod/delete/${id}`,
};

export const groupNotice = {
  getAll: (query: string | null | undefined) => `/groupNotice/get/?${query}`,
  create: '/groupNotice/create',
  update: '/groupNotice/update',
  changeCrucial: '/groupNotice/crucial',
  remove: (id: string) => `/groupNotice/delete/${id}`,
};

export const groupClientPaths = {
  create: '/u/groups/create',
};

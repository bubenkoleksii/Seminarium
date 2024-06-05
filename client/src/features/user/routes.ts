export const schoolProfile = {
  get: '/schoolProfile/Get',
  getOne: (id: string) => `/schoolProfile/GetOne/${id}`,
  remove: (id: string) => `/schoolProfile/Delete/${id}`,
  create: '/schoolProfile/create',
  parentInvitation: '/schoolProfile/ParentInvitation',
  activate: (id: string) => `/schoolProfile/Activate/${id}`,
};

export const group = {
  getAll: '/group/getAll',
  getOne: '/group/getOne',
  create: '/group/create',
  createClassTeacherInvitation: '/group/classTeacherInvitation',
  createStudentInvitation: '/group/studentInvitation',
};

export const groupClientPaths = {
  create: '/u/groups/create',
};

export const schoolProfile = {
  get: '/schoolProfile/Get',
  create: '/schoolProfile/create',
  activate: (id: string) => `/schoolProfile/Activate/${id}`,
};

export const group = {
  getAll: '/group/getAll',
  getOne: '/group/getOne',
  create: '/group/create',
  createClassTeacherInvitation: '/group/classTeacherInvitation',
  createStudentInvitation: '/group/studentInvitation'
};

export const groupClientPaths = {
  create: '/u/groups/create',
};

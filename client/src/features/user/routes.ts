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

export const groupClientPaths = {
  create: '/u/groups/create',
};

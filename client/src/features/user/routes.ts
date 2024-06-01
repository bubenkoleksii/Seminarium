export const schoolProfile = {
  get: '/schoolProfile/Get',
  activate: (id: string) => `/schoolProfile/Activate/${id}`,
};

export const group = {
  getAll: '/group/getAll',
  create: '/group/create',
};

export const groupClientPaths = {
  create: '/u/groups/create',
}
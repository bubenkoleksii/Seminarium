export const joiningRequest = {
  getAll: `/joiningRequest/getAll`,
  getOne: (id: string) => `/joiningRequest/getOne/${id}`,
  reject: (id: string) => `/joiningRequest/reject/${id}`,
};

export const school = {
  getAll: `/school/getAll`,
}

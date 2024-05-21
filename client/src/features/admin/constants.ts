export const enum CurrentTab {
  Profile = 'profile',
  JoiningRequest = 'joining_request',
  School = 'school',
}

export const enum AdminClientPaths {
  Profile = 'admin',
  JoiningRequests = 'admin/joining_requests',
  Schools = 'admin/schools',
  CreateSchool = 'admin/schools/create'
}

export const adminQueries = {
  getOneJoiningRequest: 'joiningRequest',
  getOneSchool: 'school',
  getAllJoiningRequests: 'joiningRequests',
  getAllSchools: 'schools',
  options: {
    retry: 2,
  },
};

export const adminMutations = {
  rejectJoiningRequest: 'rejectJoiningRequest',
  createSchool: 'createSchool',
  options: {
    retry: 2,
  },
};

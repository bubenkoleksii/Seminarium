export const enum CurrentTab {
  Profile = 'profile',
  JoiningRequest = 'joining_request',
}

export const enum AdminClientPaths {
  Profile = 'admin',
  JoiningRequests = 'admin/joining_requests',
}

export const adminQueries = {
  getOneJoiningRequest: 'joiningRequest',
  getAllJoiningRequests: 'joiningRequests',
  options: {
    retry: 2,
  },
};

export const adminMutations = {
  rejectJoiningRequest: 'rejectJoiningRequest',
  options: {
    retry: 2,
  }
};

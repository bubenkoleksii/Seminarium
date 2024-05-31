export const userQueries = {
  getProfiles: 'getProfiles',
  options: {
    retry: 4,
  },
};

export const userMutations = {
  activateProfile: 'activateProfile',
  options: {
    retry: 3,
  }
};

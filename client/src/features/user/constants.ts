export const enum CurrentTab {
  Profile = 'profile',
  School = 'school',
  Group = 'group',
}

export const userQueries = {
  getGroups: 'getAllGroups',
  options: {
    retry: 4,
  },
};

export const userMutations = {
  activateProfile: 'activateProfile',
  createGroup: 'createGroup',
  options: {
    retry: 3,
  },
};

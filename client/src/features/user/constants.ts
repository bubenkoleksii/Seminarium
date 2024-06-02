export const enum CurrentTab {
  Profile = 'profile',
  School = 'school',
  Group = 'group',
}

export const userQueries = {
  getGroups: 'getAllGroups',
  getOneGroup: 'getOneGroup',
  options: {
    retry: 4,
  },
};

export const userMutations = {
  activateProfile: 'activateProfile',
  createGroup: 'createGroup',
  createClassTeacherInvitation: 'createClassTeacherInvitation',
  createStudentInvitation: 'createStudentInvitation',
  createSchoolProfile: 'createSchoolProfile',
  options: {
    retry: 3,
  },
};

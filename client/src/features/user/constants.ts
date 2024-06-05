export const enum CurrentTab {
  Profile = 'profile',
  School = 'school',
  Group = 'group',
}

export const userQueries = {
  getSchoolProfile: 'getSchoolProfile',
  getGroups: 'getAllGroups',
  getOneGroup: 'getOneGroup',
  options: {
    retry: 4,
  },
};

export const userMutations = {
  activateProfile: 'activateProfile',
  createGroup: 'createGroup',
  deleteProfile: 'deleteProfile',
  createClassTeacherInvitation: 'createClassTeacherInvitation',
  createParentInvitation: 'createParentInvitation',
  createStudentInvitation: 'createStudentInvitation',
  createSchoolProfile: 'createSchoolProfile',
  options: {
    retry: 3,
  },
};

export const studentHealthGroups = {
  free: 'free',
  preparatory: 'preparatory',
  special: 'special',
  main: 'main',
};

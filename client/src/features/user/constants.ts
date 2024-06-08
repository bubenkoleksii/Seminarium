export const enum CurrentTab {
  Profile = 'profile',
  School = 'school',
  Group = 'group',
  Children = 'children',
  SchoolProfiles = 'schoolProfiles',
}

export const userQueries = {
  getSchoolProfile: 'getSchoolProfile',
  getAllSchoolProfilesBySchool: 'getAllSchoolProfilesBySchool',
  getGroups: 'getAllGroups',
  getOneGroup: 'getOneGroup',
  options: {
    retry: 4,
  },
};

export const userMutations = {
  activateProfile: 'activateProfile',
  createGroup: 'createGroup',
  updateGroup: 'updateGroup',
  deleteGroup: 'deleteGroup',
  updateGroupImage: 'updateGroupImage',
  deleteGroupImage: 'deleteGroupImage',
  deleteProfile: 'deleteProfile',
  updateSchoolProfileImage: 'updateSchoolProfileImage',
  deleteSchoolProfileImage: 'deleteSchoolProfileImage',
  createClassTeacherInvitation: 'createClassTeacherInvitation',
  createParentInvitation: 'createParentInvitation',
  createStudentInvitation: 'createStudentInvitation',
  createSchoolProfile: 'createSchoolProfile',
  updateSchoolProfile: 'updateSchoolProfile',
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

export const schoolOnlyProfileTypes = {
  schoolAdmin: 'schoolAdmin',
  teacher: 'teacher',
  classTeacher: 'classTeacher',
  student: 'student',
};

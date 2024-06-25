export const enum CurrentTab {
  Profile = 'profile',
  School = 'school',
  Group = 'group',
  Course = 'course',
  GroupNotices = 'groupNotices',
  Children = 'children',
  SchoolProfiles = 'schoolProfiles',
  StudyPeriods = 'studyPeriods',
  PracticalItems = 'practicalItems',
}

export const userQueries = {
  getSchoolProfile: 'getSchoolProfile',
  getOneCourse: 'getOneCourse',
  getAllSchoolProfilesBySchool: 'getAllSchoolProfilesBySchool',
  getGroups: 'getAllGroups',
  getOneGroup: 'getOneGroup',
  getGroupNotices: 'getGroupNotices',
  getStudyPeriods: 'getStudyPeriods',
  getCourses: 'getCourses',
  options: {
    retry: 4,
  },
};

export const userMutations = {
  activateProfile: 'activateProfile',
  createLesson: 'createLesson',
  updateLesson: 'updateLesson',
  deleteLesson: 'deleteLesson',
  createGroup: 'createGroup',
  updateGroup: 'updateGroup',
  deleteGroup: 'deleteGroup',
  addCourseGroup: 'addCourseGroup',
  deleteCourseGroup: 'deleteCourseGroup',
  addCourseTeacher: 'addCourseTeacher',
  deleteCourseTeacher: 'deleteCourseTeacher',
  createGroupNotice: 'createGroupNotice',
  updateGroupNotice: 'updateGroupNotice',
  deleteGroupNotice: 'deleteGroupNotice',
  changeCrucial: 'changeCrucial',
  createStudyPeriod: 'createStudyPeriod',
  deleteStudyPeriod: 'deleteStudyPeriod',
  updateStudyPeriod: 'updateStudyPeriod',
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
  createCourse: 'createCourse',
  updateCourse: 'updateCourse',
  deleteCourse: 'deleteCourse',
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

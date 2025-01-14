export function buildQueryString(params) {
  let queryString = '';

  for (const key in params) {
    if (
      params.hasOwnProperty(key) &&
      params[key] !== undefined &&
      params[key] !== null &&
      params[key] !== ''
    ) {
      queryString += `${key}=${encodeURIComponent(params[key])}&`;
    }
  }

  queryString = queryString.slice(0, -1);
  return queryString;
}

export const replaceEmptyStringsWithNull = (values) => {
  Object.keys(values).forEach((key) => {
    if (typeof values[key] === 'string' && values[key] === '') {
      values[key] = null;
    }
  });
};

export function classNames(...classes) {
  return classes.filter(Boolean).join(' ');
}

type Status =
  | 'approved'
  | 'ok'
  | 'rejected'
  | 'danger'
  | 'created'
  | 'main'
  | 'special'
  | 'free'
  | 'preparatory';

export function getColorByStatus(status?: Status | null | string) {
  if (status === 'approved' || status === 'ok' || status === 'main') {
    return `text-[#1A9B06FF]`;
  }

  if (status === 'rejected' || status === 'danger' || status === 'free') {
    return `text-[#C00210F2]`;
  }

  if (status === 'created') {
    return `text-purple-950`;
  }

  if (status === 'preparatory') {
    return `text-[#FFD700]`;
  }

  if (status === 'special') {
    return `text-[#FFA500]`;
  }

  return `text-[#2d2d2d]`;
}

export function getDefaultProfileImgByType(type?: string | null) {
  const images = {
    student: '/profile/student.png',
    teacher: '/profile/teacher.png',
    school_admin: '/profile/school_admin.png',
    class_teacher: '/profile/class_teacher.png',
    parent: '/profile/parent.png',
  };

  const defaultImage = '/profile/profile.svg';

  return images[type] || defaultImage;
}

export function truncateString(str, num) {
  if (str.length <= num) {
    return str;
  }
  return str.slice(0, num) + '.';
}

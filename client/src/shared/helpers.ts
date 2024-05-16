export function buildQueryString(params) {
  let queryString = '';

  for (const key in params) {
    if (params.hasOwnProperty(key) && params[key] !== undefined && params[key] !== '') {
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

type Status = 'approved' | 'ok' | 'rejected' | 'danger' | 'created';

export function getColorByStatus(status?: Status | null) {
  if (status === 'approved' || status === 'ok') {
    return `text-[#1A9B06FF]`;
  }

  if (status === 'rejected' || status === 'danger') {
    return `text-[#C00210F2]`;
  }

  if (status === 'created') {
    return `text-purple-950`;
  }

  return `text-[#2d2d2d]`;
}

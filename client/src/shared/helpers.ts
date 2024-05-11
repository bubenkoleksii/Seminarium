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

export function getColorByStatus(status: string) {
  if (status === 'approved') {
    return `text-[#1A9B06FF]`
  }

  if (status === 'rejected') {
    return `text-[#C00210F2]`
  }

  if (status === 'created') {
    return `text-purple-950`
  }

  return `text-[#2d2d2d]`;
}
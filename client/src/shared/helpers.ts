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
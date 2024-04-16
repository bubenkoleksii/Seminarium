export const replaceEmptyStringsWithNull = (values) => {
  Object.keys(values).forEach(key => {
    if (typeof values[key] === 'string' && values[key] === '') {
      values[key] = null;
    }
  });
};
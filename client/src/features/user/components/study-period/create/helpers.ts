import { format, getDay, subDays } from 'date-fns';

export const getStartDate = () => {
  const now = new Date();
  const currentYear = now.getFullYear();
  const startDate = new Date(currentYear, 8, 1);
  return format(startDate, 'yyyy-MM-dd');
};

export const getEndDate = () => {
  const now = new Date();
  const nextYear = now.getFullYear() + 1;
  const endDate = new Date(nextYear, 4, 31);
  const dayOfWeek = getDay(endDate);
  const lastFriday =
    dayOfWeek === 5 ? endDate : subDays(endDate, dayOfWeek + 2);
  return format(lastFriday, 'yyyy-MM-dd');
};

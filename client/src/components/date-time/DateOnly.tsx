'use client';

import { FC } from 'react';
import Moment from 'react-moment';
import { useLocale } from 'next-intl';
import 'moment/locale/uk';
import 'moment/locale/ro';
import 'moment/locale/hu';

interface DateOnlyProps {
  date: string;
  showDayOfWeek?: boolean;
}

const DateOnly: FC<DateOnlyProps> = ({ date, showDayOfWeek = false }) => {
  const activeLocale = useLocale();
  const dateLocale = activeLocale === 'crh' ? 'en-us' : activeLocale;
  const format = showDayOfWeek ? 'ddd, DD.MM.YY' : 'DD.MM.YY';

  return (
    <Moment locale={dateLocale} format={format}>
      {date}
    </Moment>
  );
};

export { DateOnly };

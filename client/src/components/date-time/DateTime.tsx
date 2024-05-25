'use client';

import { FC } from 'react';
import Moment from 'react-moment';
import { useLocale } from 'next-intl';
import 'moment/locale/uk';
import 'moment/locale/ro';
import 'moment/locale/hu';

interface DateTimeProps {
  date: string;
}

const DateTime: FC<DateTimeProps> = ({ date }) => {
  const activeLocale = useLocale();
  const dateLocale = activeLocale === 'crh' ? 'en-us' : activeLocale;

  return (
    <Moment locale={dateLocale} local format="ddd, DD.MM.YY HH:mm">
      {date}
    </Moment>
  );
};

export { DateTime };

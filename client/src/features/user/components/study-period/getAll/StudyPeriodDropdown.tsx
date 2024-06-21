'use client';

import { DateOnly } from '@/components/date-time';
import { getAllStudyPeriods } from '@/features/user/api/studyPeriodApi';
import { userQueries } from '@/features/user/constants';
import { StudyPeriodResponse } from '@/features/user/types/studyPeriodTypes';
import { ApiResponse } from '@/shared/types';
import { useQuery } from '@tanstack/react-query';
import { Dropdown } from 'flowbite-react';
import { useTranslations } from 'next-intl';
import { FC, useEffect, useState } from 'react';

type StudyPeriodsDropdownProps = {
  defaultPeriodId?: string;
  onSelect: (id: string) => void;
};

const StudyPeriodsDropdown: FC<StudyPeriodsDropdownProps> = ({
  defaultPeriodId,
  onSelect,
}) => {
  const t = useTranslations('StudyPeriod');

  const { data, isLoading } = useQuery<ApiResponse<StudyPeriodResponse[]>>({
    queryKey: [userQueries.getStudyPeriods],
    retry: userQueries.options.retry,
    queryFn: () => getAllStudyPeriods(),
  });

  const [selectedPeriod, setSelectedPeriod] =
    useState<StudyPeriodResponse | null>(
      defaultPeriodId
        ? data?.find((period) => period.id === defaultPeriodId) || null
        : null,
    );

  useEffect(() => {
    setSelectedPeriod(
      defaultPeriodId
        ? data?.find((period) => period.id === defaultPeriodId) || null
        : null,
    );
  }, [data, defaultPeriodId]);

  const handleSelectChange = (period: StudyPeriodResponse) => {
    setSelectedPeriod(period);
    onSelect(period.id);
  };

  if (isLoading) return null;

  return (
    <Dropdown
      fullSized
      className="border focus:outline-none"
      label={
        selectedPeriod ? (
          <>
            <DateOnly date={selectedPeriod.startDate} /> -{' '}
            <DateOnly date={selectedPeriod.endDate} />
          </>
        ) : (
          `${t('select')}`
        )
      }
      dismissOnClick={false}
    >
      {data &&
        data.map((period) => (
          <Dropdown.Item
            key={period.id}
            onClick={() => handleSelectChange(period)}
            className="cursor-pointer focus:outline-none"
          >
            <span>
              <DateOnly date={period.startDate} /> -{' '}
              <DateOnly date={period.endDate} />
            </span>
          </Dropdown.Item>
        ))}
    </Dropdown>
  );
};

export { StudyPeriodsDropdown };

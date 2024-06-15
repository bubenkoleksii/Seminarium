'use client';

import { SearchInput } from '@/components/search-input';
import { useTranslations } from 'next-intl';
import { FC, useState } from 'react';

type CreateCourseGroupProps = {
  courseId: string;
  schoolId: string;
};

const CreateCourseGroup: FC<CreateCourseGroupProps> = ({
  courseId,
  schoolId,
}) => {
  const t = useTranslations('Course');

  const [search, setSearch] = useState<string>('');

  const handleSearch = (text) => {
    console.log(text);
  };

  return (
    <div className="p-3">
      <SearchInput
        maxLength={200}
        value={search}
        placeholder={t('placeholders.search')}
        onSubmit={handleSearch}
      />
    </div>
  );
};

export { CreateCourseGroup };


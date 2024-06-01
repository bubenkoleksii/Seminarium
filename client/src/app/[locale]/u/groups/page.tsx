import { FC } from 'react';
import { Groups } from '@/features/user';

type Props = {
  searchParams: {
    studyPeriodNumber: string;
    name: string;
    take: string;
    page: string;
  };
};

const GroupsPage: FC<Props> = ({ searchParams }) => {
  return (
    <div className="p-3">
      <Groups
        studyPeriodNumber={searchParams.studyPeriodNumber ? Number(searchParams.studyPeriodNumber) : null}
        searchParameter={searchParams.name}
        limitParameter={searchParams.take ? Number(searchParams.take) : null}
        pageParameter={searchParams.page ? Number(searchParams.page) : null}
      />
    </div>
  );
};

export default GroupsPage;

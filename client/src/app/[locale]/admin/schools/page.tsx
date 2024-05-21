import { FC } from 'react';
import { Schools } from '@/features/admin';

type Props = {
  searchParams: {
    region: string;
    sortByDateAsc: string;
    schoolName: string;
    take: string;
    page: string;
  };
};

const SchoolsPage: FC<Props> = ({ searchParams }) => {
  return (
    <div className="p-3">
      <Schools
        regionParameter={searchParams.region}
        sortByDateAscParameter={searchParams.sortByDateAsc}
        searchParameter={searchParams.schoolName}
        limitParameter={searchParams.take ? Number(searchParams.take) : null}
        pageParameter={searchParams.page ? Number(searchParams.page) : null}
      />
    </div>
  );
};

export default SchoolsPage;

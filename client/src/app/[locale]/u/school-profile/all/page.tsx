import { FC } from 'react';
import { GetAllSchoolProfiles } from '@/features/user';

type Props = {
  searchParams: {
    group: string;
    type: string;
    name: string;
    take: string;
    page: string;
  };
};

const AllSchoolProfilesPage: FC<Props> = ({ searchParams }) => {
  return (
    <div className="p-3">
      <GetAllSchoolProfiles
        groupParameter={searchParams.group}
        typeParameter={searchParams.type}
        searchParameter={searchParams.name}
        limitParameter={searchParams.take ? Number(searchParams.take) : null}
        pageParameter={searchParams.page ? Number(searchParams.page) : null}
      />
    </div>
  );
};

export default AllSchoolProfilesPage;

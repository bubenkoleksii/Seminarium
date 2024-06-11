import { GetAllGroupNotices } from '@/features/user';
import { FC } from 'react';

type GroupNoticePageProps = {
  params: {
    groupId: string;
  };
  searchParams: {
    myOnly: string;
    search: string;
    take: string;
    page: string;
  };
};

const GroupNoticesPage: FC<GroupNoticePageProps> = ({
  params,
  searchParams,
}) => {
  return (
    <div className="p-3">
      <GetAllGroupNotices
        groupId={params.groupId}
        searchParameter={searchParams.search}
        myOnlyParameter={
          searchParams.myOnly ? Boolean(searchParams.myOnly) || false : false
        }
        limitParameter={searchParams.take ? Number(searchParams.take) : null}
        pageParameter={searchParams.page ? Number(searchParams.page) : null}
      />
    </div>
  );
};

export default GroupNoticesPage;

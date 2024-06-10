import { CreateGroupNoticeForm } from '@/features/user';
import { FC } from 'react';

type GroupNoticePageProps = {
  params: {
    groupId: string;
  };
};

const GroupNoticesPage: FC<GroupNoticePageProps> = ({ params }) => {
  return (
    <div className="p-3">
      <CreateGroupNoticeForm groupId={params.groupId} />
    </div>
  );
};

export default GroupNoticesPage;

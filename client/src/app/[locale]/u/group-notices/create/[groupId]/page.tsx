import { CreateGroupNoticeForm } from '@/features/user';
import { FC } from 'react';

type CreateGroupNoticePageProps = {
  params: {
    groupId: string;
  };
};

const GroupNoticesPage: FC<CreateGroupNoticePageProps> = ({ params }) => {
  return (
    <div className="p-3">
      <CreateGroupNoticeForm groupId={params.groupId} />
    </div>
  );
};

export default GroupNoticesPage;

import { UpdateGroupNoticeForm } from '@/features/user';
import type { UpdateGroupNoticeRequest } from '@/features/user/types/groupNoticesTypes';
import { FC } from 'react';

type UpdateGroupNoticePageProps = {
  params: {
    id: string;
  };
  searchParams: UpdateGroupNoticeRequest;
};

const UpdateGroupNoticePage: FC<UpdateGroupNoticePageProps> = ({
  params,
  searchParams,
}) => {
  return (
    <div className="p-3">
      <UpdateGroupNoticeForm
        id={params.id}
        noticeRequest={searchParams}
      />
    </div>
  );
};

export default UpdateGroupNoticePage;

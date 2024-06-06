import { FC } from 'react';
import { UpdateGroupRequest } from '@/features/user/types/groupTypes';
import { UpdateGroupForm } from '@/features/user';

type UpdateGroupPageProps = {
  params: {
    id: string;
  };
  searchParams: UpdateGroupRequest;
};

const UpdateGroupPage: FC<UpdateGroupPageProps> = ({
  params,
  searchParams,
}) => {
  return (
    <div className="p-3">
      <UpdateGroupForm id={params.id} group={searchParams} />
    </div>
  );
};

export default UpdateGroupPage;

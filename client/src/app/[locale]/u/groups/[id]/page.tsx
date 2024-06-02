import { FC } from 'react';
import { Group } from '@/features/user';

type GroupPageProps = {
  params: {
    id: string;
  }
}

const GroupPage: FC<GroupPageProps> = ({ params }) => {
  return (
    <div className="p-3">
      <Group id={params.id} />
    </div>
  );
};

export default GroupPage;

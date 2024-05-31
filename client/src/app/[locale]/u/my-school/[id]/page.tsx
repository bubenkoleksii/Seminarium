import { FC } from 'react';
import { School } from '@/features/school';

type Props = {
  params: {
    id: string;
  };
};

const MySchool: FC<Props> = ({ params }) => {
  return (
    <div className="p-3">
      <School id={params.id} />
    </div>
  );
};

export default MySchool;

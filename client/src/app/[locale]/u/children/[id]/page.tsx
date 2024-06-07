import { Children } from '@/features/user';
import { FC } from 'react';

type ChildrenPageProps = {
  params: {
    id: string;
  };
};

const ChildrenPage: FC<ChildrenPageProps> = ({ params }) => {
  return (
    <div className="p-3">
      <Children id={params.id} />
    </div>
  );
};

export default ChildrenPage;

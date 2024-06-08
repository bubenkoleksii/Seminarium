import { FC } from 'react';
import { SchoolProfileDetails } from '@/features/user';

type SchoolProfilePageProps = {
  params: {
    id: string;
  };
};

const SchoolProfilePage: FC<SchoolProfilePageProps> = ({ params }) => {
  return (
    <div className="p-3">
      <SchoolProfileDetails id={params.id} />
    </div>
  );
};

export default SchoolProfilePage;

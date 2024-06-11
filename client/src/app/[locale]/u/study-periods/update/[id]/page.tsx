import { UpdateStudyPeriodForm } from '@/features/user';
import { UpdateStudyPeriodRequest } from '@/features/user/types/studyPeriodTypes';
import { FC } from 'react';

type UpdateStudyPeriodPageProps = {
  params: {
    id: string;
  };
  searchParams: UpdateStudyPeriodRequest;
};

const UpdateStudyPeriodPage: FC<UpdateStudyPeriodPageProps> = ({
  params,
  searchParams,
}) => {
  return (
    <div className="p-3">
      <UpdateStudyPeriodForm id={params.id} studyPeriod={searchParams} />
    </div>
  );
};

export default UpdateStudyPeriodPage;

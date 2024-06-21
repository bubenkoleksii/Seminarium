import { PracticalLessonItemSubmit } from '@/features/user';
import { FC } from 'react';

type GetOnePracticalItemSubmitPageProps = {
  searchParams: {
    itemId: string;
    studentId: string;
  };
};

const GetOnePracticalItemSubmitPage: FC<GetOnePracticalItemSubmitPageProps> = ({
  searchParams,
}) => {
  return (
    <div className="p-3">
      <PracticalLessonItemSubmit
        itemId={searchParams.itemId}
        studentId={searchParams.studentId}
      />
    </div>
  );
};

export default GetOnePracticalItemSubmitPage;

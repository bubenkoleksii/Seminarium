import { AcceptPracticalLessonItemSubmitForm } from '@/features/user';
import { FC } from 'react';

type AcceptPracticalItemSubmitPageProps = {
  searchParams: {
    submitId: string;
    itemId: string;
    studentId: string;
  };
};

const AcceptPracticalItemSubmitPage: FC<AcceptPracticalItemSubmitPageProps> = ({
  searchParams,
}) => {
  return (
    <div className="p-3">
      <AcceptPracticalLessonItemSubmitForm
        submitId={searchParams.submitId}
        studentId={searchParams.studentId}
        itemId={searchParams.itemId}
      />
    </div>
  );
};

export default AcceptPracticalItemSubmitPage;

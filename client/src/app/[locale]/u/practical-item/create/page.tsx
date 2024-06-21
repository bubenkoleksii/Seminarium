import { CreatePracticalLessonItemForm } from '@/features/user';
import { FC } from 'react';

type CreatePracticalItemPageProps = {
  searchParams: {
    lessonId: string;
    courseId: string;
  };
};

const CreatePracticalItemPage: FC<CreatePracticalItemPageProps> = ({
  searchParams,
}) => {
  return (
    <div className="p-3">
      <CreatePracticalLessonItemForm
        lessonId={searchParams.lessonId}
        courseId={searchParams.courseId}
      />
    </div>
  );
};

export default CreatePracticalItemPage;

import { CreateTheoryLessonItemForm } from '@/features/user';
import { FC } from 'react';

type CreateTheoryItemPageProps = {
  searchParams: {
    lessonId: string;
    courseId: string;
  };
};

const CreateTheoryItemPage: FC<CreateTheoryItemPageProps> = ({
  searchParams,
}) => {
  return (
    <div className="p-3">
      <CreateTheoryLessonItemForm
        lessonId={searchParams.lessonId}
        courseId={searchParams.courseId}
      />
    </div>
  );
};

export default CreateTheoryItemPage;

import { CreateLessonForm } from '@/features/user';
import { FC } from 'react';

type CreateLessonPageProps = {
  params: {
    courseId: string;
  };
};

const CreateLessonPage: FC<CreateLessonPageProps> = ({ params }) => {
  return (
    <div className="p-3">
      <CreateLessonForm courseId={params.courseId} />
    </div>
  );
};

export default CreateLessonPage;

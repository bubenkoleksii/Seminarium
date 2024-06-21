import { UpdateLessonForm } from '@/features/user';
import type { UpdateLessonRequest } from '@/features/user/types/lessonTypes';
import { FC } from 'react';

type UpdateLessonPageProps = {
  params: {
    courseId: string;
  };
  searchParams: UpdateLessonRequest;
};

const UpdateLessonPage: FC<UpdateLessonPageProps> = ({
  params,
  searchParams,
}) => {
  return (
    <div className="p-3">
      <UpdateLessonForm courseId={params.courseId} lesson={searchParams} />
    </div>
  );
};

export default UpdateLessonPage;

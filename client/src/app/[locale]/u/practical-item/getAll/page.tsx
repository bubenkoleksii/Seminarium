import { GetAllPracticalLessonItems } from '@/features/user';
import { FC } from 'react';

type GetAllPracticalItemsPageProps = {
  searchParams: {
    lessonId: string;
    courseId: string;
  };
};

const GetAllPracticalItemsPage: FC<GetAllPracticalItemsPageProps> = ({
  searchParams,
}) => {
  return (
    <div className="p-3">
      <GetAllPracticalLessonItems
        lessonId={searchParams.lessonId}
        courseId={searchParams.courseId}
      />
    </div>
  );
};

export default GetAllPracticalItemsPage;
